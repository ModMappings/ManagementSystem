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

    private SelectListVisitor selectListVisitor;
    private OrderByClauseVisitor orderByClauseVisitor;
    private FromClauseVisitor fromClauseVisitor;
    private WhereClauseVisitor whereClauseVisitor;

    SelectWithJoinStatementVisitor(RenderContext context) {

        this.context = context;
        this.selectRenderContext = context.getSelect();
        this.selectListVisitor = new SelectListVisitor(context, selectList::append);
        this.orderByClauseVisitor = new OrderByClauseVisitor(context);
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
    public DelegatingVisitor.Delegation doEnter(Visitable segment) {

        if (segment instanceof SelectList) {
            return DelegatingVisitor.Delegation.delegateTo(selectListVisitor);
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
    public DelegatingVisitor.Delegation doLeave(Visitable segment) {

        if (segment instanceof Select) {

            Select select = (Select) segment;

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

            CharSequence orderBy = orderByClauseVisitor.getRenderedPart();
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
