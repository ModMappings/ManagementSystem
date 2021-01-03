package org.modmappings.mmms.er2dbc.data.statements.expression;

import java.util.List;

public class DistinctOnExpression implements Expression {

    private final List<Expression> source;

    public DistinctOnExpression(final List<Expression> source) {
        this.source = source;
    }

    public List<Expression> getSource() {
        return source;
    }

    @Override
    public boolean isDistinctOn() {
        return true;
    }

    @Override
    public Expression dealias() {
        return this;
    }
}
