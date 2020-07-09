package org.modmappings.mmms.er2dbc.data.statements.criteria;

import org.modmappings.mmms.er2dbc.data.statements.criteria.step.CriteriaStep;
import org.modmappings.mmms.er2dbc.data.statements.criteria.step.DefaultCriteriaStep;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expression;
import org.springframework.data.r2dbc.query.Criteria;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

/**
 * Central class for creating queries. It follows a fluent API style so that you can easily chain together multiple
 * criteria. Static import of the {@code Criteria.property(…)} method will improve readability as in
 * {@code where(property(…).is(…)}.
 * <p>
 * Note this class is in function identical to the {@link Criteria} in R2DBC, however this class
 * keeps track of which table the reference column in the criteria is part of, allowing it to be used
 * in joined sql statements.
 *
 * @author Mark Paluch
 * @author Oliver Drotbohm
 * @author Marc Hermans
 */
public class ColumnBasedCriteria {

    @Nullable
    private final ColumnBasedCriteria previous;
    private final Combinator combinator;
    private final Type type;

    private final Expression leftExpression;
    private final Comparator comparator;
    private final Expression rightExpression;

    public ColumnBasedCriteria(final Type type, final Expression leftExpression, final Comparator comparator, final Expression rightExpression) {
        this(null, Combinator.INITIAL, type, leftExpression, comparator, rightExpression);
    }

    public ColumnBasedCriteria(@Nullable final ColumnBasedCriteria previous, final Combinator combinator, final Type type, final Expression leftExpression, final Comparator comparator,
                               final Expression rightExpression) {

        this.previous = previous;
        this.combinator = combinator;
        this.type = type;
        this.leftExpression = leftExpression;
        this.comparator = comparator;
        this.rightExpression = rightExpression;
    }

    /**
     * Static factory method to create a Criteria using the provided {@code left}-Expression.
     *
     * @param left Must not be {@literal null} or empty.
     * @return a new {@link CriteriaStep} object to complete the first {@link ColumnBasedCriteria}.
     */
    public static CriteriaStep where(final Expression left) {
        Assert.notNull(left, "left must not be null or empty!");

        return new DefaultCriteriaStep(Type.WHERE, left);
    }

    /**
     * Static factory method to create a Criteria using the provided {@code left}-Expression.
     *
     * @param left Must not be {@literal null} or empty.
     * @return a new {@link CriteriaStep} object to complete the first {@link ColumnBasedCriteria}.
     */
    public static CriteriaStep on(final Expression left) {
        Assert.notNull(left, "left must not be null or empty!");

        return new DefaultCriteriaStep(Type.ON, left);
    }

    /**
     * Create a new {@link ColumnBasedCriteria} and combine it with {@code AND} using the provided {@code left}-Expression name.
     *
     * @param left Must not be {@literal null} or empty.
     * @return a new {@link CriteriaStep} object to complete the next {@link ColumnBasedCriteria}.
     */
    public CriteriaStep and(final Expression left) {

        Assert.notNull(left, "left must not be null!");

        return new DefaultCriteriaStep(type, left) {
            @Override
            protected ColumnBasedCriteria createCriteria(final Comparator comparator, final Expression right) {
                return new ColumnBasedCriteria(ColumnBasedCriteria.this, Combinator.AND, type, left, comparator, right);
            }
        };
    }

    /**
     * Create a new {@link ColumnBasedCriteria} and combine it with {@code OR} using the provided {@code left}-Expression name.
     *
     * @param left Must not be {@literal null} or empty.
     * @return a new {@link CriteriaStep} object to complete the next {@link ColumnBasedCriteria}.
     */
    public CriteriaStep or(final Expression left) {

        Assert.notNull(left, "left name must not be null!");

        return new DefaultCriteriaStep(type, left) {
            @Override
            protected ColumnBasedCriteria createCriteria(final Comparator comparator, final Expression right) {
                return new ColumnBasedCriteria(ColumnBasedCriteria.this, Combinator.OR, type, left, comparator, right);
            }
        };
    }

    /**
     * @return the previous {@link ColumnBasedCriteria} object. Can be {@literal null} if there is no previous {@link ColumnBasedCriteria}.
     * @see #hasPrevious()
     */
    @Nullable
    public ColumnBasedCriteria getPrevious() {
        return previous;
    }

    /**
     * @return {@literal true} if this {@link ColumnBasedCriteria} has a previous one.
     */
    public boolean hasPrevious() {
        return previous != null;
    }

    /**
     * @return {@link Combinator} to combine this criteria with a previous one.
     */
    public Combinator getCombinator() {
        return combinator;
    }

    /**
     * @return The type of the criteria.
     */
    public Type getType() {
        return type;
    }

    /**
     * @return the left expression.
     */
    public Expression getLeftExpression() {
        return leftExpression;
    }

    /**
     * @return {@link Comparator}.
     */
    public Comparator getComparator() {
        return comparator;
    }

    /**
     * @return The right expression.
     */
    public Expression getRightExpression() {
        return rightExpression;
    }

    public enum Type {
        WHERE, ON;
    }

    public enum Comparator {
        EQ, NEQ, LT, LTE, GT, GTE, IS_NULL, IS_NOT_NULL, LIKE, NOT_IN, IN, MATCH
    }

    public enum Combinator {
        INITIAL, AND, OR;
    }

}
