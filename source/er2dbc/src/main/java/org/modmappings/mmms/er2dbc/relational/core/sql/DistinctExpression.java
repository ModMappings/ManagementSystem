package org.modmappings.mmms.er2dbc.relational.core.sql;


import org.springframework.data.relational.core.sql.Expression;

public class DistinctExpression extends AbstractSegment implements Expression {

    public DistinctExpression(final Expression inner) {
        super(
                inner
        );
    }

    @Override
    public String toString() {
        return "DISTINCT " + getChildren()[0].toString();
    }
}
