package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.modmappings.mmms.er2dbc.relational.core.sql.SelectList;
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class SelectListVisitor extends TypedSubtreeVisitor<SelectList> implements PartRenderer {

    private final RenderContext context;
    private final StringBuilder builder = new StringBuilder();
    private final RenderTarget target;
    private boolean requiresComma = false;
    private boolean insideFunction = false; // this is hackery and should be fix with a proper visitor for
    // subelements.


    SelectListVisitor(RenderContext context, RenderTarget target) {
        this.context = context;
        this.target = target;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(Visitable segment) {

        if (requiresComma) {
            builder.append(", ");
            requiresComma = false;
        }
        if (segment instanceof SimpleFunction) {
            builder.append(((SimpleFunction) segment).getFunctionName()).append("(");
            insideFunction = true;
        }

        return super.enterNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(SelectList segment) {

        target.onRendered(builder);
        return super.leaveMatched(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveNested(Visitable segment) {

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
        } else if (segment instanceof AsteriskFromTable) {
            // the toString of AsteriskFromTable includes the table name, which would cause it to appear twice.
            builder.append("*");
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