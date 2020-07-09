package org.modmappings.mmms.er2dbc.data.statements.expression;

public class ValueExpression implements Expression {
    private final Object value;

    public ValueExpression(final Object value) {
        this.value = value;
    }

    public Object getValue() {
        return value;
    }

    @Override
    public boolean isValue() {
        return true;
    }

    @Override
    public Expression dealias() {
        return this;
    }
}
