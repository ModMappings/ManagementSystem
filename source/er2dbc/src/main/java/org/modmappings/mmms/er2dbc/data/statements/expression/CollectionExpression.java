package org.modmappings.mmms.er2dbc.data.statements.expression;

import java.util.Arrays;
import java.util.Collection;
import java.util.stream.Collectors;

public class CollectionExpression implements Expression {
    private final Collection<Expression> expressions;

    public CollectionExpression(final Expression... expressions) {
        this(Arrays.asList(expressions));
    }

    public CollectionExpression(final Collection<Expression> expressions) {
        this.expressions = expressions;
        for (final Expression expression : expressions) {
            if (expression.isCollection())
                throw new IllegalArgumentException("Can not create a collection in a collection!");
        }
    }

    public Collection<Expression> getExpressions() {
        return expressions;
    }

    @Override
    public boolean isCollection() {
        return true;
    }

    @Override
    public Expression dealias() {
        return new CollectionExpression(this.expressions.stream().map(Expression::dealias).collect(Collectors.toList()));
    }
}
