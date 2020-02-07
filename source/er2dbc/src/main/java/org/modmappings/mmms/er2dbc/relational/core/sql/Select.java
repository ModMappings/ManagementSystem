package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.*;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.OptionalLong;

/**
 *
 */
public class Select implements org.springframework.data.relational.core.sql.Select {
    private final boolean distinct;
    private final SelectList selectList;
    private final From from;
    private final long limit;
    private final long offset;
    private final List<Join> joins;
    @Nullable
    private final Where where;
    private final List<OrderByField> orderBy;

    public Select(final boolean distinct, final List<Expression> selectList, final List<Table> from, final long limit, final long offset,
                  final List<Join> joins, @Nullable final Condition where, final List<OrderByField> orderBy) {

        this.distinct = distinct;
        this.selectList = new SelectList(new ArrayList<>(selectList));
        this.from = new From(new ArrayList<>(from));
        this.limit = limit;
        this.offset = offset;
        this.joins = new ArrayList<>(joins);
        this.orderBy = Collections.unmodifiableList(new ArrayList<>(orderBy));
        this.where = where != null ? new Where(where) : null;
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Select#getOrderBy()
     */
    @Override
    public List<OrderByField> getOrderBy() {
        return this.orderBy;
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Select#getLimit()
     */
    @Override
    public OptionalLong getLimit() {
        return limit == -1 ? OptionalLong.empty() : OptionalLong.of(limit);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Select#getOffset()
     */
    @Override
    public OptionalLong getOffset() {
        return offset == -1 ? OptionalLong.empty() : OptionalLong.of(offset);
    }

    @Override
    public boolean isDistinct() {
        return distinct;
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Visitable#visit(org.springframework.data.relational.core.sql.Visitor)
     */
    @Override
    public void visit(final Visitor visitor) {

        Assert.notNull(visitor, "Visitor must not be null!");

        visitor.enter(this);

        selectList.visit(visitor);
        from.visit(visitor);
        joins.forEach(it -> it.visit(visitor));

        visitIfNotNull(where, visitor);

        orderBy.forEach(it -> it.visit(visitor));

        visitor.leave(this);
    }

    private void visitIfNotNull(@Nullable final Visitable visitable, final Visitor visitor) {

        if (visitable != null) {
            visitable.visit(visitor);
        }
    }
}
