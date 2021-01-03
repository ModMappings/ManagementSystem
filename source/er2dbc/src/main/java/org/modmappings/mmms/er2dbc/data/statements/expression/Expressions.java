package org.modmappings.mmms.er2dbc.data.statements.expression;

import java.util.Arrays;
import java.util.Collection;
import java.util.stream.Collectors;

public final class Expressions {

    private Expressions() {
        throw new IllegalStateException("Tried to initialize: Expressions but this is a Utility class.");
    }

    public static Expression parameter(final Object value) {
        if (value == null)
            return Expression.NULL;

        return new ValueExpression(value);
    }

    public static Expression parameter(final Object... values) {
        return parameter(Arrays.asList(values));
    }

    public static Expression parameter(final Collection<?> values) {
        return new CollectionExpression(values.stream().map(Expressions::parameter).collect(Collectors.toSet()));
    }

    public static Expression parameter(final String name, final Object value) {
        if (value == null)
            return Expression.NULL;

        return new ValueExpression(value, name);
    }

    public static Expression parameter(final String name, final Object... values) {
        return parameter(name, Arrays.asList(values));
    }

    public static Expression reference(final String columnName) {
        return new ReferenceExpression("", columnName);
    }

    public static Expression reference(final String tableName, final String columnName) {
        return new ReferenceExpression(tableName, columnName);
    }

    public static Expression just(final String sql) {
        return spring(NativeExpressionsBridge.just(sql));
    }

    public static Expression spring(final org.springframework.data.relational.core.sql.Expression sqlExpression) {
        return new NativeExpression(sqlExpression);
    }

    public static Expression distinct(final Expression expression) {
        return new DistinctExpression(expression);
    }

    public static Expression aliased(final Expression inner, final String alias) {
        return new AliasedExpression(inner, alias);
    }

    public static Expression invoke(final String name, final Expression... args) {
        return new FunctionExpression(name, args);
    }

    public static Expression NULL() {
        return Expression.NULL;
    }
}
