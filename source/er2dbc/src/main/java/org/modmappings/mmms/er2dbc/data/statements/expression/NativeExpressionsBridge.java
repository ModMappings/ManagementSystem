package org.modmappings.mmms.er2dbc.data.statements.expression;

import org.springframework.data.relational.core.sql.Expressions;

public class NativeExpressionsBridge {

    private NativeExpressionsBridge() {
        throw new IllegalStateException("Can not instantiate an instance of: NativeExpressionsBridge. This is a utility class");
    }

    public static org.springframework.data.relational.core.sql.Expression just(final String sql) {
        return Expressions.just(sql);
    }
}
