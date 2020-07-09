package org.modmappings.mmms.er2dbc.data.statements.expression;

public class AliasedExpression implements Expression {
    private final Expression other;
    private final String alias;

    public AliasedExpression(final Expression other, final String alias) {
        this.other = other;
        this.alias = alias;
    }

    public Expression getOther() {
        return other;
    }

    public String getAlias() {
        return alias;
    }

    @Override
    public boolean isAliased() {
        return true;
    }

    @Override
    public Expression dealias() {
        return other.dealias();
    }
}
