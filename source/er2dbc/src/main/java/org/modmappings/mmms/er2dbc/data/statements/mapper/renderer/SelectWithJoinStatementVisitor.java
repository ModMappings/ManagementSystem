package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.modmappings.mmms.er2dbc.relational.core.sql.*;
import org.springframework.data.relational.core.sql.OrderByField;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.*;

public class SelectWithJoinStatementVisitor extends DelegatingVisitor implements PartRenderer {
    private final RenderContext context;
    private final SelectRenderContext selectRenderContext;

    private StringBuilder builder = new StringBuilder();
    private StringBuilder selectList = new StringBuilder();
    private StringBuilder from = new StringBuilder();
    private StringBuilder join = new StringBuilder();
    private StringBuilder where = new StringBuilder();
    private StringBuilder orderBy = new StringBuilder();

    private ExpressionListVisitor selectListVisitor;
    private ExpressionListVisitor orderByClauseVisitor;
    private FromClauseVisitor fromClauseVisitor;
    private WhereClauseVisitor whereClauseVisitor;

    SelectWithJoinStatementVisitor(final RenderContext context) {

        this.context = context;
        this.selectRenderContext = context.getSelect();
        this.selectListVisitor = new ExpressionListVisitor(context, selectList::append);
        this.orderByClauseVisitor = new ExpressionListVisitor(context, orderBy::append);
        this.fromClauseVisitor = new FromClauseVisitor(context, it -> {

            if (from.length() != 0) {
                from.append(", ");
            }

            from.append(it);
        });

        this.whereClauseVisitor = new WhereClauseVisitor(context, where::append);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.render.DelegatingVisitor#doEnter(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    public DelegatingVisitor.Delegation doEnter(final Visitable segment) {

        if (segment instanceof ExpressionList) {
            final ExpressionList expressionList = (ExpressionList) segment;
            if (expressionList.getMode() == ExpressionList.Mode.SELECT)
                return DelegatingVisitor.Delegation.delegateTo(selectListVisitor);
            else
                return Delegation.delegateTo(orderByClauseVisitor);
        }

        if (segment instanceof OrderByField) {
            return DelegatingVisitor.Delegation.delegateTo(orderByClauseVisitor);
        }

        if (segment instanceof From) {
            return DelegatingVisitor.Delegation.delegateTo(fromClauseVisitor);
        }

        if (segment instanceof Join) {
            return DelegatingVisitor.Delegation.delegateTo(new JoinVisitor(context, it -> {

                if (join.length() != 0) {
                    join.append(' ');
                }

                join.append(it);
            }));
        }

        if (segment instanceof Where) {
            return DelegatingVisitor.Delegation.delegateTo(whereClauseVisitor);
        }

        return DelegatingVisitor.Delegation.retain();
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.render.DelegatingVisitor#doLeave(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    public DelegatingVisitor.Delegation doLeave(final Visitable segment) {

        if (segment instanceof Select) {

            final Select select = (Select) segment;

            builder.append("SELECT ");

            if (select.isDistinct()) {
                builder.append("DISTINCT ");
            }

            builder.append(selectList);
            builder.append(selectRenderContext.afterSelectList().apply(select));

            if (from.length() != 0) {
                builder.append(" FROM ").append(from);
            }

            if (join.length() != 0) {
                builder.append(' ').append(join);
            }

            if (where.length() != 0) {
                builder.append(" WHERE ").append(where);
            }

            final CharSequence orderBy = orderByClauseVisitor.getRenderedPart();
            if (orderBy.length() != 0) {
                builder.append(" ORDER BY ").append(orderBy);
            }

            builder.append(selectRenderContext.afterOrderBy(orderBy.length() != 0).apply(select));

            return DelegatingVisitor.Delegation.leave();
        }

        return DelegatingVisitor.Delegation.retain();
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.render.PartRenderer#getRenderedPart()
     */
    @Override
    public CharSequence getRenderedPart() {
        return builder;
    }
}
