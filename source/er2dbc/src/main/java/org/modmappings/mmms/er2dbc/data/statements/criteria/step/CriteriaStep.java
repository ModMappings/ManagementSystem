package org.modmappings.mmms.er2dbc.data.statements.criteria.step;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expression;

import java.util.Collection;

/**
 * Interface declaring terminal builder methods to build a {@link ColumnBasedCriteria}.
 */
public interface CriteriaStep {

    /**
     * Creates a {@link ColumnBasedCriteria} using equality.
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria is(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using equality (is not).
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria not(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code IN}.
     *
     * @param rights must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria in(Expression... rights);

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code IN}.
     *
     * @param rights must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria in(Collection<Expression> rights);

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code NOT IN}.
     *
     * @param rights must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria notIn(Expression... rights);

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code NOT IN}.
     *
     * @param rights must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria notIn(Collection<Expression> rights);

    /**
     * Creates a {@link ColumnBasedCriteria} using less-than ({@literal <}).
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria lessThan(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using less-than or equal to ({@literal <=}).
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria lessThanOrEquals(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using greater-than({@literal >}).
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria greaterThan(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using greater-than or equal to ({@literal >=}).
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria greaterThanOrEquals(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code LIKE}.
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria like(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code MATCH}.
     *
     * @param right must not be {@literal null}.
     * @return
     */
    ColumnBasedCriteria matches(Expression right);

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code IS NULL}.
     *
     * @return
     */
    ColumnBasedCriteria isNull();

    /**
     * Creates a {@link ColumnBasedCriteria} using {@code IS NOT NULL}.
     *
     * @return
     */
    ColumnBasedCriteria isNotNull();
}
