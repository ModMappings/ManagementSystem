package org.modmappings.mmms.er2dbc.data.query.mapper;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
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
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.util.ClassTypeInformation;
import org.springframework.data.util.TypeInformation;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;
import org.springframework.util.ClassUtils;
import org.springframework.util.StringUtils;

import java.util.*;

public class ExtendedMapper extends UpdateMapper {

    private final MappingContext<? extends RelationalPersistentEntity<?>, RelationalPersistentProperty> mappingContext;
    private final IMatchFormatter matchFormatter;

    /**
     * Creates a new {@link ExtendedMapper} with the given {@link R2dbcConverter}.
     *
     * @param converter must not be {@literal null}.
     * @param matchFormatter
     */
    public ExtendedMapper(R2dbcConverter converter, IMatchFormatter matchFormatter) {
        super(converter);

        this.mappingContext = (MappingContext) converter.getMappingContext();
        this.matchFormatter = matchFormatter;
    }

    /**
     * Map a {@link ColumnBasedCriteria} object to {@link BoundCondition} and consider value/{@code NULL} {@link Bindings}.
     *
     * @param markers bind markers object, must not be {@literal null}.
     * @param criteria on definition to map, must not be {@literal null}.
     * @param table must not be {@literal null}.
     * @param entity related {@link RelationalPersistentEntity}, can be {@literal null}.
     * @return the mapped {@link BoundAssignments}.
     */
    public BoundCondition getMappedObject(BindMarkers markers, ColumnBasedCriteria criteria, Table table,
                                          @Nullable RelationalPersistentEntity<?> entity) {

        Assert.notNull(markers, "BindMarkers must not be null!");
        Assert.notNull(criteria, "Criteria must not be null!");
        Assert.notNull(table, "Table must not be null!");

        ColumnBasedCriteria current = criteria;
        MutableBindings bindings = new MutableBindings(markers);

        // reverse unroll criteria chain
        Map<ColumnBasedCriteria, ColumnBasedCriteria> forwardChain = new HashMap<>();

        while (current.hasPrevious()) {
            forwardChain.put(current.getPrevious(), current);
            current = current.getPrevious();
        }

        // perform the actual mapping
        Condition mapped = getCondition(current, bindings, table, entity);
        while (forwardChain.containsKey(current)) {

            ColumnBasedCriteria nextCriteria = forwardChain.get(current);

            if (nextCriteria.getCombinator() == ColumnBasedCriteria.Combinator.AND) {
                mapped = mapped.and(getCondition(nextCriteria, bindings, table, entity));
            }

            if (nextCriteria.getCombinator() == ColumnBasedCriteria.Combinator.OR) {
                mapped = mapped.or(getCondition(nextCriteria, bindings, table, entity));
            }

            current = nextCriteria;
        }

        return new BoundCondition(bindings, mapped);
    }

    /**
     * Remaps a ER2DBC DSL join type to a R2DBC DSL join type
     * @param type The ER2DBC join type.
     * @return The remapped join type.
     */
    public Join.JoinType getMappedObject(JoinSpec.JoinType type) {
        return Join.JoinType.valueOf(type.name());
    }

    public Expression getMappedObject(ColumnBasedCriteria.Expression expression, Table defaultTable)
    {
        if (expression.isNative())
            return ((ColumnBasedCriteria.NativeExpression) expression).getSqlExpression();

        if (!expression.isReference())
            throw new IllegalArgumentException("Can not map none reference expression to column");

        final ColumnBasedCriteria.ReferenceExpression referenceExpression = (ColumnBasedCriteria.ReferenceExpression) expression;
        return Column.create(referenceExpression.getColumnName(), StringUtils.isEmpty(referenceExpression.getTableName()) ? defaultTable : Table.create(referenceExpression.getTableName()));
    }

    public Condition getCondition(ColumnBasedCriteria criteria, MutableBindings bindings, Table table,
                                   @Nullable RelationalPersistentEntity<?> entity) {

        final Expression left = convertNoneCollectiveExpression(criteria.getLeftExpression(), criteria.getRightExpression(), table, bindings);
        if (left == null)
            throw new IllegalArgumentException("Can not create a condition from a criteria which has a null or collective left side.");

        if (criteria.getComparator().equals(ColumnBasedCriteria.Comparator.IS_NULL)) {
            return Conditions.isNull(left);
        }

        if (criteria.getComparator().equals(ColumnBasedCriteria.Comparator.IS_NOT_NULL)) {
            return Conditions.isNull(left).not();
        }

        if (criteria.getComparator() == ColumnBasedCriteria.Comparator.NOT_IN || criteria.getComparator() == ColumnBasedCriteria.Comparator.IN) {
            final Collection<Expression> right = convertCollectiveExpression(criteria.getRightExpression(), criteria.getLeftExpression(), table, bindings);

            Condition condition = Conditions.in(left, right);

            if (criteria.getComparator() == ColumnBasedCriteria.Comparator.NOT_IN) {
                condition = condition.not();
            }

            return condition;
        }

        final Expression right = convertNoneCollectiveExpression(criteria.getRightExpression(), criteria.getLeftExpression(), table, bindings);
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

    private Expression convertNoneCollectiveExpression(ColumnBasedCriteria.Expression expression, ColumnBasedCriteria.Expression otherExpression, Table defaultTable, MutableBindings bindings)
    {
        if (expression.isNative())
            return ((ColumnBasedCriteria.NativeExpression) expression).getSqlExpression();

        if (expression.isNull())
            return SQL.nullLiteral();

        if (expression.isReference())
        {
            final ColumnBasedCriteria.ReferenceExpression referenceExpression = (ColumnBasedCriteria.ReferenceExpression) expression;
            return Column.create(referenceExpression.getColumnName(), StringUtils.isEmpty(referenceExpression.getTableName()) ? defaultTable : Table.create(referenceExpression.getTableName()));
        }

        if (expression.isValue())
        {

            if (otherExpression.isReference())
            {
                Object mappedValue = null;
                Class<?> typeHint;

                final ColumnBasedCriteria.ReferenceExpression referenceExpression = (ColumnBasedCriteria.ReferenceExpression) otherExpression;

                final Field propertyField = createPropertyField(referenceExpression.getTableName(), referenceExpression.getColumnName(), this.mappingContext);
                TypeInformation<?> actualType = propertyField.getTypeHint().getRequiredActualType();

                final ColumnBasedCriteria.ValueExpression valueExpression = (ColumnBasedCriteria.ValueExpression) expression;
                if (valueExpression.getValue() instanceof SettableValue) {
                    SettableValue settableValue = (SettableValue) valueExpression.getValue();
                    mappedValue = convertValue(settableValue.getValue(), propertyField.getTypeHint());
                    typeHint = getTypeHint(mappedValue, actualType.getType(), settableValue);

                } else {
                    mappedValue = convertValue(valueExpression.getValue(), propertyField.getTypeHint());
                    typeHint = actualType.getType();
                }

                BindMarker bindMarker = bindings.nextMarker("");
                return bind(mappedValue, typeHint, bindings, bindMarker);
            }
        }

        return null;
    }

    private Collection<Expression> convertCollectiveExpression(ColumnBasedCriteria.Expression expression, ColumnBasedCriteria.Expression otherExpression, Table defaultTable, MutableBindings bindings) {
        if (expression.isCollection())
        {
            final ColumnBasedCriteria.CollectionExpression collectionExpression = (ColumnBasedCriteria.CollectionExpression) expression;
            final List<Expression> expressions = new ArrayList<>(collectionExpression.getExpressions().size());

            for (ColumnBasedCriteria.Expression collectionExpressionExpression : collectionExpression.getExpressions()) {
                if (collectionExpressionExpression.isCollection())
                    return null;

                expressions.add(convertNoneCollectiveExpression(collectionExpressionExpression, otherExpression, defaultTable, bindings));
            }

            return expressions;
        }

        return null;
    }

    public Field createPropertyField(
                              String tableName,
                              String key,
                              MappingContext<? extends RelationalPersistentEntity<?>, RelationalPersistentProperty> mappingContext) {
        return StringUtils.isEmpty(tableName) ? new Field(key) : new ExtendedMetadataBackedField(key, findEntityFromTableName(tableName), mappingContext);
    }

    public RelationalPersistentEntity<?> findEntityFromTableName(String table) {
        for (RelationalPersistentEntity<?> persistentEntity : this.mappingContext.getPersistentEntities()) {
            if (persistentEntity.getTableName().equals(table))
                return persistentEntity;
        }

        throw new IllegalArgumentException("Unknown table name: " + table);
    }

    public Class<?> getTypeHint(Object mappedValue, Class<?> propertyType, SettableValue settableValue) {

        if (mappedValue == null || propertyType.equals(Object.class)) {
            return settableValue.getType();
        }

        if (mappedValue.getClass().equals(settableValue.getValue().getClass())) {
            return settableValue.getType();
        }

        return propertyType;
    }

    public Expression bind(@Nullable Object mappedValue, Class<?> valueType, MutableBindings bindings,
                            BindMarker bindMarker) {

        if (mappedValue != null) {
            bindings.bind(bindMarker, mappedValue);
        } else {
            bindings.bindNull(bindMarker, valueType);
        }

        return SQL.bindMarker(bindMarker.getPlaceholder());
    }

    public static class ExtendedMetadataBackedField extends MetadataBackedField {

        public ExtendedMetadataBackedField(String name, RelationalPersistentEntity<?> entity, MappingContext<? extends RelationalPersistentEntity<?>, RelationalPersistentProperty> context) {
            super(name, entity, context);
        }
    }
}
