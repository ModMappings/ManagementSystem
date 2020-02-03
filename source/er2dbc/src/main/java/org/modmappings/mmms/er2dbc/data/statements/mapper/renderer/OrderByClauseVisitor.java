package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Column;
import org.springframework.data.relational.core.sql.OrderByField;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class OrderByClauseVisitor extends TypedSubtreeVisitor<OrderByField> implements PartRenderer {

    private final RenderContext context;

    private final StringBuilder builder = new StringBuilder();
    private boolean first = true;

    OrderByClauseVisitor(RenderContext context) {
        this.context = context;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterMatched(OrderByField segment) {

        if (!first) {
            builder.append(", ");
        }
        first = false;

        return super.enterMatched(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(OrderByField segment) {

        OrderByField field = segment;

        if (field.getDirection() != null) {
            builder.append(" ") //
                    .append(field.getDirection());
        }

        return Delegation.leave();
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveNested(Visitable segment) {

        if (segment instanceof Column) {
            builder.append(context.getNamingStrategy().getReferenceName(((Column) segment)));
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