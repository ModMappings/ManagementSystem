package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Aliased;
import org.springframework.data.relational.core.sql.Expression;

public class AliasedExpression extends AbstractSegment implements Aliased, Expression {

    private final Expression expression;
    private final String alias;

    public AliasedExpression(Expression expression, String alias) {

        super(expression);

        this.expression = expression;
        this.alias = alias;
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Aliased#getAlias()
     */
    @Override
    public String getAlias() {
        return alias;
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public String toString() {
        return " AS " + alias;
    }
}
