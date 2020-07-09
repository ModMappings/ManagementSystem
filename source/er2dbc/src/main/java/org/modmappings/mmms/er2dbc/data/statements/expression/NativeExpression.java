package org.modmappings.mmms.er2dbc.data.statements.expression;

public class NativeExpression implements Expression {
    private final org.springframework.data.relational.core.sql.Expression sqlExpression;

    public NativeExpression(final org.springframework.data.relational.core.sql.Expression sqlExpression) {
        this.sqlExpression = sqlExpression;
    }

    public org.springframework.data.relational.core.sql.Expression getSqlExpression() {
        return sqlExpression;
    }

    @Override
    public boolean isNative() {
        return true;
    }

    @Override
    public Expression dealias() {
        return this;
    }
}
