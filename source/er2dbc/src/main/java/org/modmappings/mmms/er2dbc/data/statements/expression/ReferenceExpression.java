package org.modmappings.mmms.er2dbc.data.statements.expression;

public class ReferenceExpression implements Expression {
    private final String tableName;
    private final String columnName;

    public ReferenceExpression(final String tableName, final String columnName) {
        this.tableName = tableName;
        this.columnName = columnName;
    }

    public String getTableName() {
        return tableName;
    }

    public String getColumnName() {
        return columnName;
    }

    @Override
    public boolean isReference() {
        return true;
    }

    @Override
    public Expression dealias() {
        return this;
    }
}
