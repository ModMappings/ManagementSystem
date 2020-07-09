package org.modmappings.mmms.er2dbc.data.statements.criteria.step;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.CollectionExpression;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expression;
import org.springframework.util.Assert;

import java.util.Collection;

/**
 * Default {@link CriteriaStep} implementation.
 */
public class DefaultCriteriaStep implements CriteriaStep {

    private final ColumnBasedCriteria.Type type;
    private final Expression left;

    public DefaultCriteriaStep(final ColumnBasedCriteria.Type type, final Expression left) {
        this.type = type;
        this.left = left;
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#is(java.lang.Object)
     */
    @Override
    public ColumnBasedCriteria is(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.EQ, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#not(java.lang.Object)
     */
    @Override
    public ColumnBasedCriteria not(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.NEQ, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#in(java.lang.Object[])
     */
    @Override
    public ColumnBasedCriteria in(final Expression... rights) {
        final Expression right;

        Assert.notNull(rights, "rights must not be null!");
        if (rights.length == 1 && rights[1].isCollection()) {
            right = rights[1];
        }
        else
        {
            right = new CollectionExpression(rights);
        }
        return createCriteria(ColumnBasedCriteria.Comparator.IN, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#in(java.util.Collection)
     */
    @Override
    public ColumnBasedCriteria in(final Collection<Expression> rights) {

        Assert.notNull(rights, "rights must not be null!");
        Assert.noNullElements(rights.toArray(), "rights must not contain a null value!");

        return createCriteria(ColumnBasedCriteria.Comparator.IN, new CollectionExpression(rights));
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#notIn(java.lang.Object[])
     */
    @Override
    public ColumnBasedCriteria notIn(final Expression... rights) {
        final Expression right;

        Assert.notNull(rights, "rights must not be null!");
        if (rights.length == 1 && rights[1].isCollection()) {
            right = rights[1];
        }
        else
        {
            right = new CollectionExpression(rights);
        }

        return createCriteria(ColumnBasedCriteria.Comparator.NOT_IN, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#notIn(java.util.Collection)
     */
    @Override
    public ColumnBasedCriteria notIn(final Collection<Expression> rights) {

        Assert.notNull(rights, "rights must not be null!");
        Assert.noNullElements(rights.toArray(), "rights must not contain a null value!");

        return createCriteria(ColumnBasedCriteria.Comparator.NOT_IN, new CollectionExpression(rights));
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#lessThan(java.lang.Object)
     */
    @Override
    public ColumnBasedCriteria lessThan(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.LT, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#lessThanOrEquals(java.lang.Object)
     */
    @Override
    public ColumnBasedCriteria lessThanOrEquals(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.LTE, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#greaterThan(java.lang.Object)
     */
    @Override
    public ColumnBasedCriteria greaterThan(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.GT, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#greaterThanOrEquals(java.lang.Object)
     */
    @Override
    public ColumnBasedCriteria greaterThanOrEquals(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.GTE, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#like(java.lang.Object)
     */
    @Override
    public ColumnBasedCriteria like(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.LIKE, right);
    }

    @Override
    public ColumnBasedCriteria matches(final Expression right) {

        Assert.notNull(right, "right must not be null!");

        return createCriteria(ColumnBasedCriteria.Comparator.MATCH, right);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#isNull()
     */
    @Override
    public ColumnBasedCriteria isNull() {
        return createCriteria(ColumnBasedCriteria.Comparator.IS_NULL, Expression.NULL);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#isNotNull()
     */
    @Override
    public ColumnBasedCriteria isNotNull() {
        return createCriteria(ColumnBasedCriteria.Comparator.IS_NOT_NULL, Expression.NULL);
    }

    protected ColumnBasedCriteria createCriteria(final ColumnBasedCriteria.Comparator comparator, final Expression right) {
        return new ColumnBasedCriteria(type, this.left, comparator, right);
    }
}
