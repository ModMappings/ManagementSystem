package org.modmappings.mmms.er2dbc.data.statements.sort;

import org.modmappings.mmms.er2dbc.data.statements.expression.Expression;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.springframework.data.domain.Sort;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.stream.Collectors;

public class SortSpec {

    public static final SortSpec UNSORTED = new SortSpec(Collections.emptyList());

    private final List<Order> components;

    private SortSpec(final List<Order> components) {
        this.components = components;
    }

    public static SortSpec sort(final Sort sort) {
        if (sort.isUnsorted())
            return UNSORTED;

        final List<Order> components = sort.stream().map(order -> new Order(Order.Direction.fromJPA(order.getDirection()),
                Expressions.reference(order.getProperty()))).collect(Collectors.toList());
        return new SortSpec(components);
    }

    public static SortSpec sort(final Order order) {
        return new SortSpec(Collections.singletonList(order));
    }

    public static SortSpec sort(final Order.Direction direction, final Expression expression)
    {
        return sort(new Order(direction, expression));
    }

    public SortSpec and(final Order order) {
        final List<Order> newComponents = new ArrayList<>(this.components);
        newComponents.add(order);

        return new SortSpec(newComponents);
    }

    public SortSpec and(final Order.Direction direction, final Expression expression) {
        return and(new Order(direction, expression));
    }

    public List<Order> getComponents() {
        return components;
    }

    public boolean isUnsorted() {
        return this.components.isEmpty();
    }

    public static class Order {
        private final Direction direction;
        private final Expression expression;

        private Order(final Direction direction, final Expression expression) {
            this.direction = direction;
            this.expression = expression;
        }

        public static Order desc(final Expression expression)
        {
            return new Order(Direction.DESC, expression);
        }

        public static Order asc(final Expression expression)
        {
            return new Order(Direction.ASC, expression);
        }

        public Direction getDirection() {
            return direction;
        }

        public Expression getExpression() {
            return expression;
        }

        public enum Direction {
            DESC,
            ASC;

            public static Direction fromJPA(Sort.Direction sortDirection) {
                if (sortDirection.isAscending())
                    return ASC;

                return DESC;
            }
        }
    }
}
