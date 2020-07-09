package org.modmappings.mmms.er2dbc.data.statements.expression;

public class ValueExpression implements Expression {
    private final Object value;
    private final String name;

    public ValueExpression(final Object value) {
        this.value = value;
        this.name = "";
    }

    public ValueExpression(final Object value, final String name) {
        this.value = value;
        this.name = name;
    }

    public Object getValue() {
        return value;
    }

    @Override
    public boolean isValue() {
        return true;
    }

    public String getName() {
        return name;
    }

    @Override
    public Expression dealias() {
        return this;
    }
}
