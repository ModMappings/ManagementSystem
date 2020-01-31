package org.modmappings.mmms.er2dbc.data.statements.criteria;

import org.springframework.data.r2dbc.query.Criteria;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import java.util.Arrays;
import java.util.Collection;
import java.util.stream.Collectors;

/**
 * Central class for creating queries. It follows a fluent API style so that you can easily chain together multiple
 * criteria. Static import of the {@code Criteria.property(…)} method will improve readability as in
 * {@code where(property(…).is(…)}.
 *
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

    private ColumnBasedCriteria(Type type, Expression leftExpression, Comparator comparator, Expression rightExpression) {
        this(null, Combinator.INITIAL, type, leftExpression, comparator, rightExpression);
    }

    private ColumnBasedCriteria(@Nullable ColumnBasedCriteria previous, Combinator combinator, Type type, Expression leftExpression, Comparator comparator,
                                Expression rightExpression) {

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
    public static CriteriaStep where(Expression left) {
        Assert.notNull(left, "left must not be null or empty!");

        return new DefaultCriteriaStep(Type.WHERE, left);
    }

    /**
     * Static factory method to create a Criteria using the provided {@code left}-Expression.
     *
     * @param left Must not be {@literal null} or empty.
     * @return a new {@link CriteriaStep} object to complete the first {@link ColumnBasedCriteria}.
     */
    public static CriteriaStep on(Expression left) {
        Assert.notNull(left, "left must not be null or empty!");

        return new DefaultCriteriaStep(Type.ON, left);
    }

    /**
     * Create a new {@link ColumnBasedCriteria} and combine it with {@code AND} using the provided {@code left}-Expression name.
     *
     * @param left Must not be {@literal null} or empty.
     * @return a new {@link CriteriaStep} object to complete the next {@link ColumnBasedCriteria}.
     */
    public CriteriaStep and(Expression left) {

        Assert.notNull(left, "left must not be null!");

        return new DefaultCriteriaStep(type, left) {
            @Override
            protected ColumnBasedCriteria createCriteria(Comparator comparator, Expression right) {
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
    public CriteriaStep or(Expression left) {

        Assert.notNull(left, "left name must not be null!");

        return new DefaultCriteriaStep(type, left) {
            @Override
            protected ColumnBasedCriteria createCriteria(Comparator comparator, Expression right) {
                return new ColumnBasedCriteria(ColumnBasedCriteria.this, Combinator.OR, type, left, comparator, right);
            }
        };
    }

    public static Expression parameter(Object value) {
        if (value == null)
            return Expression.NULL;

        return new ValueExpression(value);
    }

    public static Expression parameter(Object... values)
    {
        return parameter(Arrays.asList(values));
    }

    public static Expression parameter(Collection<?> values)
    {
        return new CollectionExpression(values.stream().map(ColumnBasedCriteria::parameter).collect(Collectors.toSet()));
    }

    public static Expression reference(String columnName)
    {
        return new ColumnBasedCriteria.ReferenceExpression("", columnName);
    }

    public static Expression reference(String tableName, String columnName)
    {
        return new ColumnBasedCriteria.ReferenceExpression(tableName, columnName);
    }

    public static Expression spring(org.springframework.data.relational.core.sql.Expression sqlExpression)
    {
        return new NativeExpression(sqlExpression);
    }

    public static Expression NULL()
    {
        return Expression.NULL;
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
    public
    Combinator getCombinator() {
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

    /**
     * Default {@link CriteriaStep} implementation.
     */
    static class DefaultCriteriaStep implements CriteriaStep {

        private final Type type;
        private final Expression left;

        public DefaultCriteriaStep(Type type, Expression left) {
            this.type = type;
            this.left = left;
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#is(java.lang.Object)
         */
        @Override
        public ColumnBasedCriteria is(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.EQ, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#not(java.lang.Object)
         */
        @Override
        public ColumnBasedCriteria not(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.NEQ, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#in(java.lang.Object[])
         */
        @Override
        public ColumnBasedCriteria in(Expression... rights) {
            Expression right;

            Assert.notNull(rights, "rights must not be null!");
            if (rights.length == 1 && rights[1].isCollection()) {
                right = rights[1];
            }
            else
            {
                right = new CollectionExpression(rights);
            }
            return createCriteria(Comparator.IN, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#in(java.util.Collection)
         */
        @Override
        public ColumnBasedCriteria in(Collection<Expression> rights) {

            Assert.notNull(rights, "rights must not be null!");
            Assert.noNullElements(rights.toArray(), "rights must not contain a null value!");

            return createCriteria(Comparator.IN, new CollectionExpression(rights));
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#notIn(java.lang.Object[])
         */
        @Override
        public ColumnBasedCriteria notIn(Expression... rights) {
            Expression right;

            Assert.notNull(rights, "rights must not be null!");
            if (rights.length == 1 && rights[1].isCollection()) {
                right = rights[1];
            }
            else
            {
                right = new CollectionExpression(rights);
            }

            return createCriteria(Comparator.NOT_IN, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#notIn(java.util.Collection)
         */
        @Override
        public ColumnBasedCriteria notIn(Collection<Expression> rights) {

            Assert.notNull(rights, "rights must not be null!");
            Assert.noNullElements(rights.toArray(), "rights must not contain a null value!");

            return createCriteria(Comparator.NOT_IN, new CollectionExpression(rights));
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#lessThan(java.lang.Object)
         */
        @Override
        public ColumnBasedCriteria lessThan(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.LT, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#lessThanOrEquals(java.lang.Object)
         */
        @Override
        public ColumnBasedCriteria lessThanOrEquals(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.LTE, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#greaterThan(java.lang.Object)
         */
        @Override
        public ColumnBasedCriteria greaterThan(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.GT, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#greaterThanOrEquals(java.lang.Object)
         */
        @Override
        public ColumnBasedCriteria greaterThanOrEquals(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.GTE, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#like(java.lang.Object)
         */
        @Override
        public ColumnBasedCriteria like(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.LIKE, right);
        }

        @Override
        public ColumnBasedCriteria matches(Expression right) {

            Assert.notNull(right, "right must not be null!");

            return createCriteria(Comparator.MATCH, right);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#isNull()
         */
        @Override
        public ColumnBasedCriteria isNull() {
            return createCriteria(Comparator.IS_NULL, Expression.NULL);
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.query.Criteria.CriteriaStep#isNotNull()
         */
        @Override
        public ColumnBasedCriteria isNotNull() {
            return createCriteria(Comparator.IS_NOT_NULL, Expression.NULL);
        }

        protected ColumnBasedCriteria createCriteria(Comparator comparator, Expression right) {
            return new ColumnBasedCriteria(type, this.left, comparator, right);
        }
    }

    /**
     * Represents the left or right hand aside of the criteria.
     */
    public interface Expression {
        static Expression NULL = new Expression() {
            @Override
            public boolean isNull() {
                return true;
            }
        };

        default boolean isValue() { return false; }
        default boolean isReference() { return false; }
        default boolean isNull() { return false; }
        default boolean isCollection() { return false; }
        default boolean isNative() { return false; }
    }

    public static class ValueExpression implements Expression {
        private final Object value;

        ValueExpression(Object value) {
            this.value = value;
        }

        public Object getValue() {
            return value;
        }

        @Override
        public boolean isValue() {
            return true;
        }
    }

    public static class ReferenceExpression implements Expression {
        private final String tableName;
        private final String columnName;

        ReferenceExpression(String tableName, String columnName) {
            this.tableName = tableName;
            this.columnName = columnName;
        }

        public String getTableName() {
            return tableName;
        }

        public String getColumnName() {
            return columnName;
        }

        @Override
        public boolean isReference() {
            return true;
        }
    }

    public static class CollectionExpression implements Expression {
        private final Collection<Expression> expressions;

        CollectionExpression(Expression... expressions) {
            this(Arrays.asList(expressions));
        }

        CollectionExpression(Collection<Expression> expressions) {
            this.expressions = expressions;
            for (Expression expression : expressions) {
                if (expression.isCollection())
                    throw new IllegalArgumentException("Can not create a collection in a collection!");
            }
        }

        public Collection<Expression> getExpressions() {
            return expressions;
        }

        @Override
        public boolean isCollection() {
            return true;
        }
    }

    public static class NativeExpression implements Expression {
        private final org.springframework.data.relational.core.sql.Expression sqlExpression;

        public NativeExpression(org.springframework.data.relational.core.sql.Expression sqlExpression) {
            this.sqlExpression = sqlExpression;
        }

        public org.springframework.data.relational.core.sql.Expression getSqlExpression() {
            return sqlExpression;
        }

        @Override
        public boolean isNative() {
            return true;
        }
    }
}
