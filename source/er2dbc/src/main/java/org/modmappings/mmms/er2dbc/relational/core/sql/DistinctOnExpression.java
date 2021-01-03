package org.modmappings.mmms.er2dbc.relational.core.sql;


import org.springframework.data.relational.core.sql.Expression;

import java.util.Arrays;
import java.util.stream.Collectors;

public class DistinctOnExpression extends AbstractSegment implements Expression {

    public DistinctOnExpression(final Expression... inner) {
        super(
                inner
        );
    }

    @Override
    public String toString() {
        return String.format("DISTINCT ON (%s)", Arrays.stream(getChildren()).map(Object::toString).collect(Collectors.joining(", ")));
    }
}
