package org.modmappings.mmms.er2dbc.relational.core.sql;


import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.Expressions;
import org.springframework.data.relational.core.sql.Segment;
import org.springframework.util.StringUtils;

import java.util.Arrays;

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
