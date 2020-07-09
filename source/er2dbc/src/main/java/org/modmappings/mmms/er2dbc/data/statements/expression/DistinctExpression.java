package org.modmappings.mmms.er2dbc.data.statements.expression;

public class DistinctExpression implements Expression {
    private final Expression source;

    public DistinctExpression(final Expression source) {
        this.source = source;
    }

    public Expression getSource() {
        return source;
    }

    @Override
    public boolean isDistinct() {
        return true;
    }

    @Override
    public Expression dealias() {
        return new DistinctExpression(source.dealias());
    }
}
