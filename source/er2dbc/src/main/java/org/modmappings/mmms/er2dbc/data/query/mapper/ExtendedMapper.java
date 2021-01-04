package org.modmappings.mmms.er2dbc.data.query.mapper;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.*;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.mapper.bound.BoundExpression;
import org.modmappings.mmms.er2dbc.relational.core.sql.DistinctExpression;
import org.modmappings.mmms.er2dbc.relational.core.sql.IMatchFormatter;
import org.modmappings.mmms.er2dbc.relational.core.sql.Match;
import org.springframework.data.mapping.context.MappingContext;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.dialect.BindMarker;
import org.springframework.data.r2dbc.dialect.BindMarkers;
import org.springframework.data.r2dbc.dialect.Bindings;
import org.springframework.data.r2dbc.dialect.MutableBindings;
import org.springframework.data.r2dbc.mapping.SettableValue;
import org.springframework.data.r2dbc.query.BoundAssignments;
import org.springframework.data.r2dbc.query.BoundCondition;
import org.springframework.data.r2dbc.query.UpdateMapper;
import org.springframework.data.relational.core.mapping.RelationalPersistentEntity;
import org.springframework.data.relational.core.mapping.RelationalPersistentProperty;
import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.util.ClassTypeInformation;
import org.springframework.data.util.TypeInformation;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;
import org.springframework.util.StringUtils;

import java.util.*;

public class ExtendedMapper extends UpdateMapper {

    private final MappingContext<? extends RelationalPersistentEntity<?>, RelationalPersistentProperty> mappingContext;
    private final IMatchFormatter matchFormatter;
    private ExtendedStatementMapper statementMapper = null;

    /**
     * Creates a new {@link ExtendedMapper} with the given {@link R2dbcConverter}.
     *
     * @param converter      must not be {@literal null}.
     * @param matchFormatter
     */
    public ExtendedMapper(final R2dbcConverter converter, final IMatchFormatter matchFormatter) {
        super(converter);

        this.mappingContext = (MappingContext) converter.getMappingContext();
        this.matchFormatter = matchFormatter;
    }

    /**
     * Map a {@link ColumnBasedCriteria} object to {@link BoundCondition} and consider value/{@code NULL} {@link Bindings}.
     *
     * @param markers  bind markers object, must not be {@literal null}.
     * @param criteria on definition to map, must not be {@literal null}.
     * @param table    must not be {@literal null}.
     * @param entity   related {@link RelationalPersistentEntity}, can be {@literal null}.
     * @return the mapped {@link BoundAssignments}.
     */
    public BoundCondition getMappedObject(final BindMarkers markers, final ColumnBasedCriteria criteria, final Table table,
                                          @Nullable final RelationalPersistentEntity<?> entity,
                                          final Map<String, String> aliasing) {

        Assert.notNull(markers, "BindMarkers must not be null!");
        Assert.notNull(criteria, "Criteria must not be null!");
        Assert.notNull(table, "Table must not be null!");

        ColumnBasedCriteria current = criteria;
        final MutableBindings bindings = new MutableBindings(markers);

        // reverse unroll criteria chain
        final Map<ColumnBasedCriteria, ColumnBasedCriteria> forwardChain = new HashMap<>();

        while (current.hasPrevious()) {
            forwardChain.put(current.getPrevious(), current);
            current = current.getPrevious();
        }

        // perform the actual mapping
        Condition mapped = getCondition(current, bindings, table, entity, aliasing);
        while (forwardChain.containsKey(current)) {

            final ColumnBasedCriteria nextCriteria = forwardChain.get(current);

            if (nextCriteria.getCombinator() == ColumnBasedCriteria.Combinator.AND) {
                mapped = mapped.and(getCondition(nextCriteria, bindings, table, entity, aliasing));
            }

            if (nextCriteria.getCombinator() == ColumnBasedCriteria.Combinator.OR) {
                mapped = mapped.or(getCondition(nextCriteria, bindings, table, entity, aliasing));
            }

            current = nextCriteria;
        }

        return new BoundCondition(bindings, mapped);
    }

    /**
     * Remaps a ER2DBC DSL join type to a R2DBC DSL join type
     *
     * @param type The ER2DBC join type.
     * @return The remapped join type.
     */
    public Join.JoinType getMappedObject(final JoinSpec.JoinType type) {
        return Join.JoinType.valueOf(type.name());
    }

    public BoundExpression getMappedObject(final org.modmappings.mmms.er2dbc.data.statements.expression.Expression expression,
                                           final Table defaultTable,
                                           final MutableBindings bindings,
                                           final Map<String, String> aliasing) {
        if (expression.isNative())
            return new BoundExpression(Bindings.empty(), ((NativeExpression) expression).getSqlExpression());

        if (expression.isValue())
            return new BoundExpression(bindings, convertNoneCollectiveExpression(expression, null, defaultTable, bindings, aliasing));

        if (expression.isAliased()) {
            final org.modmappings.mmms.er2dbc.data.statements.expression.AliasedExpression aliasedExpression = (org.modmappings.mmms.er2dbc.data.statements.expression.AliasedExpression) expression;
            final BoundExpression boundExpression = this.getMappedObject(aliasedExpression.getOther(), defaultTable, bindings, aliasing);
            return new BoundExpression(boundExpression.getBindings(), new org.modmappings.mmms.er2dbc.relational.core.sql.AliasedExpression(boundExpression.getExpression(), aliasedExpression.getAlias()));
        }

        if (expression.isDistinctOn()) {
            final org.modmappings.mmms.er2dbc.data.statements.expression.DistinctOnExpression distinctOnExpression = (DistinctOnExpression) expression;
            final List<Expression> expressions = new ArrayList<>();
            Bindings internalBindings = Bindings.empty();

            for (final org.modmappings.mmms.er2dbc.data.statements.expression.Expression source : distinctOnExpression.getSource()) {
                final BoundExpression boundExpression = this.getMappedObject(source, defaultTable, bindings, aliasing);
                internalBindings = internalBindings.and(boundExpression.getBindings());
                expressions.add(boundExpression.getExpression());
            }
            return new BoundExpression(internalBindings, new org.modmappings.mmms.er2dbc.relational.core.sql.DistinctOnExpression(expressions.toArray(Expression[]::new)));
        }

        if (expression.isDistinct()) {
            final org.modmappings.mmms.er2dbc.data.statements.expression.DistinctExpression distinctExpression = (org.modmappings.mmms.er2dbc.data.statements.expression.DistinctExpression) expression;
            final BoundExpression boundExpression = getMappedObject(distinctExpression.getSource(), defaultTable, bindings, aliasing);
            return new BoundExpression(boundExpression.getBindings(), new DistinctExpression(boundExpression.getExpression()));
        }

        if (expression.isFunction()) {
            final FunctionExpression functionExpression = (FunctionExpression) expression;
            final List<Expression> expressions = new ArrayList<>();
            Bindings internalBindings = Bindings.empty();

            for (final org.modmappings.mmms.er2dbc.data.statements.expression.Expression arg : functionExpression.getArgs()) {
                final BoundExpression boundExpression = this.getMappedObject(arg, defaultTable, bindings, aliasing);
                internalBindings = internalBindings.and(boundExpression.getBindings());
                expressions.add(boundExpression.getExpression());
            }
            return new BoundExpression(internalBindings, SimpleFunction.create(functionExpression.getFunctionName(), expressions));
        }

        if (!expression.isReference())
            throw new IllegalArgumentException("Can not map none reference expression to column");

        final ReferenceExpression referenceExpression = (ReferenceExpression) expression;
        return new BoundExpression(Bindings.empty(), Column.create(referenceExpression.getColumnName(), StringUtils.isEmpty(referenceExpression.getTableName()) ? defaultTable : Table.create(referenceExpression.getTableName())));
    }

    public Condition getCondition(final ColumnBasedCriteria criteria,
                                  final MutableBindings bindings,
                                  final Table table,
                                  @Nullable final RelationalPersistentEntity<?> entity,
                                  final Map<String, String> aliasing) {

        final Expression left = convertNoneCollectiveExpression(criteria.getLeftExpression(), criteria.getRightExpression(), table, bindings, aliasing);
        if (left == null)
            throw new IllegalArgumentException("Can not create a condition from a criteria which has a null or collective left side.");

        if (criteria.getComparator().equals(ColumnBasedCriteria.Comparator.IS_NULL)) {
            return Conditions.isNull(left);
        }

        if (criteria.getComparator().equals(ColumnBasedCriteria.Comparator.IS_NOT_NULL)) {
            return Conditions.isNull(left).not();
        }

        if (criteria.getComparator() == ColumnBasedCriteria.Comparator.NOT_IN || criteria.getComparator() == ColumnBasedCriteria.Comparator.IN) {
            final Collection<Expression> right = convertCollectiveExpression(criteria.getRightExpression(), criteria.getLeftExpression(), table, bindings, aliasing);

            Condition condition = Conditions.in(left, right);

            if (criteria.getComparator() == ColumnBasedCriteria.Comparator.NOT_IN) {
                condition = condition.not();
            }

            return condition;
        }

        final Expression right = convertNoneCollectiveExpression(criteria.getRightExpression(), criteria.getLeftExpression(), table, bindings, aliasing);
        if (right == null)
            throw new IllegalArgumentException("Can not use a null or collective right value, if you are not doing an is null or is in compare!");

        switch (criteria.getComparator()) {
            case EQ:
                return Conditions.isEqual(left, right);
            case NEQ:
                return Conditions.isNotEqual(left, right);
            case LT:
                return Conditions.isLess(left, right);
            case LTE:
                return Conditions.isLessOrEqualTo(left, right);
            case GT:
                return Conditions.isGreater(left, right);
            case GTE:
                return Conditions.isGreaterOrEqualTo(left, right);
            case LIKE:
                return Conditions.like(left, right);
            case MATCH:
                return Match.create(left, right, matchFormatter);
            default:
                throw new UnsupportedOperationException("Comparator " + criteria.getComparator() + " not supported");
        }
    }

    private Expression convertNoneCollectiveExpression(final org.modmappings.mmms.er2dbc.data.statements.expression.Expression expression,
                                                       final org.modmappings.mmms.er2dbc.data.statements.expression.Expression otherExpression,
                                                       final Table defaultTable,
                                                       final MutableBindings bindings,
                                                       final Map<String, String> aliasing) {
        if (expression.isNative())
            return ((NativeExpression) expression).getSqlExpression();

        if (expression.isNull())
            return SQL.nullLiteral();

        if (expression.isReference()) {
            final ReferenceExpression referenceExpression = (ReferenceExpression) expression;
            Table table = defaultTable;
            if (!StringUtils.isEmpty(referenceExpression.getTableName())) {
                if (aliasing.containsKey(referenceExpression.getTableName())) {
                    table = Table.aliased(aliasing.get(referenceExpression.getTableName()), referenceExpression.getTableName());
                } else {
                    table = Table.create(referenceExpression.getTableName());
                }
            }

            return Column.create(referenceExpression.getColumnName(), table);
        }

        if (expression.isValue()) {

            final ValueExpression valueExpression = (ValueExpression) expression;
            TypeInformation<?> actualType = ClassTypeInformation.OBJECT;
            Class<?> typeHint = actualType.getType();
            Object mappedValue = convertValue(valueExpression.getValue(), actualType);

            if (otherExpression != null && otherExpression.isReference()) {
                final ReferenceExpression referenceExpression = (ReferenceExpression) otherExpression;

                final Field propertyField = createPropertyField(referenceExpression.getTableName(), referenceExpression.getColumnName(), this.mappingContext, aliasing);
                actualType = propertyField.getTypeHint().getRequiredActualType();

                if (valueExpression.getValue() instanceof SettableValue) {
                    final SettableValue settableValue = (SettableValue) valueExpression.getValue();
                    mappedValue = convertValue(settableValue.getValue(), propertyField.getTypeHint());
                    typeHint = getTypeHint(mappedValue, actualType.getType(), settableValue);
                }
            }

            final BindMarker bindMarker = bindings.nextMarker(valueExpression.getName());
            return bind(mappedValue, typeHint, bindings, bindMarker);
        }

        if (expression.isFunction()) {
            final FunctionExpression functionExpression = (FunctionExpression) expression;
            final List<Expression> expressions = new ArrayList<>();

            for (final org.modmappings.mmms.er2dbc.data.statements.expression.Expression arg : functionExpression.getArgs()) {
                final BoundExpression boundExpression = this.getMappedObject(arg, defaultTable, bindings, aliasing);
                expressions.add(boundExpression.getExpression());
            }
            return SimpleFunction.create(functionExpression.getFunctionName(), expressions);
        }



        return null;
    }

    private Collection<Expression> convertCollectiveExpression(final org.modmappings.mmms.er2dbc.data.statements.expression.Expression expression, final org.modmappings.mmms.er2dbc.data.statements.expression.Expression otherExpression, final Table defaultTable, final MutableBindings bindings,
                                                               final Map<String, String> aliasing) {
        if (expression.isCollection()) {
            final CollectionExpression collectionExpression = (CollectionExpression) expression;
            final List<Expression> expressions = new ArrayList<>(collectionExpression.getExpressions().size());

            for (final org.modmappings.mmms.er2dbc.data.statements.expression.Expression collectionExpressionExpression : collectionExpression.getExpressions()) {
                if (collectionExpressionExpression.isCollection())
                    return null;

                expressions.add(convertNoneCollectiveExpression(collectionExpressionExpression, otherExpression, defaultTable, bindings, aliasing));
            }

            return expressions;
        }

        return null;
    }

    public Field createPropertyField(
            final String tableName,
            final String key,
            final MappingContext<? extends RelationalPersistentEntity<?>, RelationalPersistentProperty> mappingContext,
            final Map<String, String> aliasing
    ) {
        return StringUtils.isEmpty(tableName) ? new Field(key) : new ExtendedMetadataBackedField(key, findEntityFromTableName(tableName, aliasing), mappingContext);
    }

    public RelationalPersistentEntity<?> findEntityFromTableName(final String table, final Map<String, String> aliasing) {
        for (final RelationalPersistentEntity<?> persistentEntity : this.mappingContext.getPersistentEntities()) {
            if (persistentEntity.getTableName().equals(table))
                return persistentEntity;
        }

        if (aliasing.containsKey(table)) {
            return findEntityFromTableName(aliasing.get(table), aliasing);
        }

        throw new IllegalArgumentException("Unknown table name: " + table);
    }

    public Class<?> getTypeHint(final Object mappedValue, final Class<?> propertyType, final SettableValue settableValue) {

        if (mappedValue == null || propertyType.equals(Object.class)) {
            return settableValue.getType();
        }

        if (mappedValue.getClass().equals(settableValue.getValue().getClass())) {
            return settableValue.getType();
        }

        return propertyType;
    }

    public Expression bind(@Nullable final Object mappedValue, final Class<?> valueType, final MutableBindings bindings,
                           final BindMarker bindMarker) {

        if (mappedValue != null) {
            bindings.bind(bindMarker, mappedValue);
        } else {
            bindings.bindNull(bindMarker, valueType);
        }

        return SQL.bindMarker(bindMarker.getPlaceholder());
    }

    public ExtendedStatementMapper getStatementMapper() {
        return statementMapper;
    }

    public void setStatementMapper(final ExtendedStatementMapper statementMapper) {
        this.statementMapper = statementMapper;
    }

    public static class ExtendedMetadataBackedField extends MetadataBackedField {

        public ExtendedMetadataBackedField(final String name, final RelationalPersistentEntity<?> entity, final MappingContext<? extends RelationalPersistentEntity<?>, RelationalPersistentProperty> context) {
            super(name, entity, context);
        }
    }
}
