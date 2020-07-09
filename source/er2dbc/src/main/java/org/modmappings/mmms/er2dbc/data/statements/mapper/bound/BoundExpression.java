package org.modmappings.mmms.er2dbc.data.statements.mapper.bound;

import org.springframework.data.r2dbc.dialect.Bindings;
import org.springframework.data.relational.core.sql.Expression;

public class BoundExpression {

    private final Bindings bindings;
    private final Expression expression;

    public BoundExpression(final Bindings bindings, final Expression expression) {
        this.bindings = bindings;
        this.expression = expression;
    }

    public Bindings getBindings() {
        return bindings;
    }

    public Expression getExpression() {
        return expression;
    }
}
