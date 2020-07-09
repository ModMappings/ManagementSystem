package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.modmappings.mmms.er2dbc.data.statements.sort.SortSpec;
import org.springframework.data.domain.Sort;
import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.Segment;
import org.springframework.util.StringUtils;

public class OrderBy extends AbstractSegment implements Expression {

    private final Direction direction;

    public OrderBy(final Expression on, final Direction direction) {
        super(on);
        this.direction = direction;
    }

    public Direction getDirection() {
        return direction;
    }

    public enum Direction implements Segment {
        DESC,
        ASC;

        public static Direction fromJPA(final Sort.Direction direction) {
            if (direction.isAscending())
                return ASC;

            return DESC;
        }

        public static Direction fromSpec(final SortSpec.Order.Direction spec) {
            if (spec == SortSpec.Order.Direction.ASC)
                return ASC;

            return DESC;
        }

        @Override
        public String toString() {
            return this.name();
        }
    }

    @Override
    public String toString() {
        return StringUtils.arrayToDelimitedString(getChildren(), " ");
    }
}
