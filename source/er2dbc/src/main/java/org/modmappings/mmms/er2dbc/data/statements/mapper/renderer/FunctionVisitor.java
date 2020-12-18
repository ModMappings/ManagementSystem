package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.modmappings.mmms.er2dbc.relational.core.sql.AliasedExpression;
import org.modmappings.mmms.er2dbc.relational.core.sql.DistinctExpression;
import org.modmappings.mmms.er2dbc.relational.core.sql.ExpressionList;
import org.modmappings.mmms.er2dbc.relational.core.sql.OrderBy;
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.relational.core.sql.render.RenderContext;

import java.util.HashMap;
import java.util.Map;

public class FunctionVisitor extends TypedSubtreeVisitor<SimpleFunction> implements PartRenderer {

    private final RenderContext context;
    private final StringBuilder builder = new StringBuilder();
    private boolean requiresComma = false;
    private boolean insideFunction = false; // this is hackery and should be fix with a proper visitor for
    // subelements.

    private final Map<Visitable, PartRenderer> renderers = new HashMap<>();

    public FunctionVisitor(final RenderContext context) {
        super();
        this.context = context;
    }

    @Override
    Delegation enterMatched(final SimpleFunction segment) {
        builder.append(segment.getFunctionName()).append("(");
        return super.enterMatched(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(final Visitable segment) {

        if (requiresComma) {
            builder.append(", ");
            requiresComma = false;
        }
        if (segment instanceof SimpleFunction) {
            builder.append(((SimpleFunction) segment).getFunctionName()).append("(");
            insideFunction = true;
        }
        if (segment instanceof DistinctExpression) {
            builder.append("DISTINCT ");
        }

        return super.enterNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(final SimpleFunction segment) {
        this.builder.append(")");
        return super.leaveMatched(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveNested(final Visitable segment) {

        if (segment instanceof Table) {
            builder.append(context.getNamingStrategy().getReferenceName((Table) segment)).append('.');
        }

        if (segment instanceof SimpleFunction) {

            builder.append(")");
            if (segment instanceof Aliased) {
                builder.append(" AS ").append(((Aliased) segment).getAlias());
            }

            insideFunction = false;
            requiresComma = true;
        } else if (segment instanceof AsteriskFromTable) {
            builder.append("*");
            requiresComma = true;
        } else if (segment instanceof Column) {

            builder.append(context.getNamingStrategy().getName((Column) segment));
            if (segment instanceof Aliased && !insideFunction) {
                builder.append(" AS ").append(((Aliased) segment).getAlias());
            }
            requiresComma = true;
        } else if (segment instanceof AliasedExpression) {
            builder.append(" AS ").append(((AliasedExpression) segment).getAlias());
        } else if (segment instanceof DistinctExpression) {
            //NOOP;
        } else if (segment instanceof BindMarker) {

            if (segment instanceof Named) {
                builder.append(((Named) segment).getName());
            } else {
                builder.append(segment.toString());
            }
        } else if (segment instanceof OrderBy) {
            builder.append(" ").append(((OrderBy) segment).getDirection());
        } else if (segment instanceof Expression) {
            builder.append(segment.toString());
        }

        return super.leaveNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see PartRenderer#getRenderedPart()
     */
    @Override
    public CharSequence getRenderedPart() {
        return builder;
    }
}
