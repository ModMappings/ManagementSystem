package org.modmappings.mmms.er2dbc.data.statements.mapper.bound;

import org.modmappings.mmms.er2dbc.relational.core.sql.OrderBy;
import org.springframework.data.r2dbc.dialect.Bindings;

import java.util.List;

public class BoundSortSpec {

    private final Bindings bindings;
    private final List<OrderBy> orderBys;

    public BoundSortSpec(final Bindings bindings, final List<OrderBy> orderBys) {
        this.bindings = bindings;
        this.orderBys = orderBys;
    }

    public Bindings getBindings() {
        return bindings;
    }

    public List<OrderBy> getOrderBys() {
        return orderBys;
    }
}
