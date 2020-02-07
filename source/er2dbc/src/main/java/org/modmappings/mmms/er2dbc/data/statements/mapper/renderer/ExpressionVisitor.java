package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.*;
import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.data.relational.core.sql.render.RenderNamingStrategy;
import org.springframework.lang.Nullable;

public class ExpressionVisitor extends TypedSubtreeVisitor<Expression> implements PartRenderer {

    private final RenderContext context;

    private CharSequence value = "";
    private @Nullable
    PartRenderer partRenderer;

    ExpressionVisitor(final RenderContext context) {
        this.context = context;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterMatched(final Expression segment) {

        if (segment instanceof SubselectExpression) {

            final SelectWithJoinStatementVisitor visitor = new SelectWithJoinStatementVisitor(context);
            partRenderer = visitor;
            return Delegation.delegateTo(visitor);
        }

        if (segment instanceof Column) {

            final RenderNamingStrategy namingStrategy = context.getNamingStrategy();
            final Column column = (Column) segment;

            value = namingStrategy.getReferenceName(column.getTable()) + "." + namingStrategy.getReferenceName(column);
        } else if (segment instanceof BindMarker) {

            if (segment instanceof Named) {
                value = ((Named) segment).getName();
            } else {
                value = segment.toString();
            }
        } else if (segment instanceof Literal) {
            value = segment.toString();
        }

        return Delegation.retain();
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(final Visitable segment) {

        if (segment instanceof Condition) {
            final ConditionVisitor visitor = new ConditionVisitor(context);
            partRenderer = visitor;
            return Delegation.delegateTo(visitor);
        }

        return super.enterNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(final Expression segment) {

        if (partRenderer != null) {
            value = partRenderer.getRenderedPart();
            partRenderer = null;
        }

        return super.leaveMatched(segment);
    }

    /*
     * (non-Javadoc)
     * @see PartRenderer#getRenderedPart()
     */
    @Override
    public CharSequence getRenderedPart() {
        return value;
    }
}
