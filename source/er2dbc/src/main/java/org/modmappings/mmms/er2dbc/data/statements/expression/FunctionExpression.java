package org.modmappings.mmms.er2dbc.data.statements.expression;

import java.util.Arrays;
import java.util.Collection;

public class FunctionExpression implements Expression {
    private final String functionName;
    private final Collection<Expression> args;

    public FunctionExpression(final String functionName, final Expression... args) {
        this(functionName, Arrays.asList(args));
    }

    public FunctionExpression(final String functionName, final Collection<Expression> args) {
        this.functionName = functionName;
        this.args = args;
    }

    public String getFunctionName() {
        return functionName;
    }

    public Collection<Expression> getArgs() {
        return args;
    }

    @Override
    public boolean isFunction() {
        return true;
    }

    @Override
    public Expression dealias() {
        return this;
    }
}
