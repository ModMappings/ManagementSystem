package org.modmappings.mmms.repository.repositories;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expression;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.slf4j.Logger;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.mapping.PersistentProperty;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.relational.core.mapping.RelationalPersistentEntity;
import org.springframework.data.relational.core.mapping.RelationalPersistentProperty;
import org.springframework.lang.NonNull;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.Arrays;
import java.util.Collection;
import java.util.List;
import java.util.function.Function;
import java.util.function.UnaryOperator;
import java.util.logging.LogManager;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import java.util.stream.StreamSupport;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.where;
import static org.modmappings.mmms.er2dbc.data.statements.expression.Expressions.*;

public interface IModMappingQuerySupport {

    Logger getLogger();

    DatabaseClient getDatabaseClient();

    R2dbcConverter getConverter();

    ExtendedDataAccessStrategy getAccessStrategy();

    /**
     * Gets all entries in the repository.
     *
     * @param pageable The page and sorting information.
     * @return The page that is requested.
     */
    default <T> Mono<Page<T>> findAll(
            final String tableName,
            final Class<T> resultType,
            final Pageable pageable
    ) {
        return createPagedStarRequest(
                UnaryOperator.identity(),
                tableName,
                resultType,
                pageable
        );
    }

    default <T> Flux<T> createFindStarRequest(final SelectSpecWithJoin selectSpec, final Class<T> resultType, final Pageable pageable) {
        Assert.notNull(selectSpec, "SelectSpec must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        final List<String> columns = this.getAccessStrategy().getAllColumns(resultType);

        final SelectSpecWithJoin selectSpecWithProj = selectSpec
                .withProjectionFromColumnName(columns);

        return createFindRequest(selectSpecWithProj, resultType, pageable);
    }

    default <R> Flux<R> createFindRequest(final SelectSpecWithJoin selectSpec, final Class<R> resultType, final Pageable pageable) {
        Assert.notNull(selectSpec, "SelectSpec must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        final SelectSpecWithJoin selectSpecWithPagination = selectSpec
                .withPage(pageable);

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper();
        if (getAccessStrategy().getConverter().getMappingContext().hasPersistentEntityFor(resultType)) {
            mapper = mapper.forType(resultType);
        }
        final PreparedOperation<?> operation = mapper.getMappedObject(selectSpecWithPagination);

        getLogger().debug("Executing find operation: " + operation.toString());

        return this.getDatabaseClient().execute(operation) //
                .as(resultType) //
                .fetch()
                .all();
    }

    default Mono<Long> createCountRequest(final SelectSpecWithJoin selectSpec, final String tableName, final Class<?> resultType) {
        Assert.notNull(selectSpec, "SelectSpec must not be null");

        final Expression countingExpression = selectSpec.getProjectedFields().size() == 1 ? selectSpec.getProjectedFields().get(0).dealias() : Expressions.reference(tableName, getIdColumnName(resultType));
        final Expression countingDistinctExpression = selectSpec.isDistinct() ? Expressions.distinct(countingExpression) : countingExpression;
        final Expression projectionExpression = Expressions.invoke("count", countingDistinctExpression);

        final SelectSpecWithJoin selectSpecWithProj =
                selectSpec.notDistinct().clearSortAndPage().setProjection(projectionExpression);


        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper();
        if (getAccessStrategy().getConverter().getMappingContext().hasPersistentEntityFor(resultType)) {
            mapper = mapper.forType(resultType);
        }
        final PreparedOperation<?> operation = mapper.getMappedObject(selectSpecWithProj);

        getLogger().debug("Executing count operation: " + operation.toString());

        return this.getDatabaseClient().execute(operation) //
                .map((r, md) -> r.get(0, Long.class)) //
                .first() //
                .defaultIfEmpty(0L);
    }

    default <R> Mono<Page<R>> createPagedRequest(final SelectSpecWithJoin selectSpecWithJoin, final String tableName, final Class<R> resultType, final Pageable pageable) {
        return createFindRequest(selectSpecWithJoin, resultType, pageable)
                .collectList()
                .flatMap(results -> createCountRequest(selectSpecWithJoin, tableName, resultType)
                        .flatMap(count -> Mono.just(new PageImpl<>(results, pageable, count))));
    }

    default <R> Mono<Page<R>> createPagedRequestWithCountType(final SelectSpecWithJoin selectSpecWithJoin, final String tableName, final Class<R> resultType, Class<?> countType, final Pageable pageable) {
        return createFindRequest(selectSpecWithJoin, resultType, pageable)
                .collectList()
                .flatMap(results -> createCountRequest(selectSpecWithJoin, tableName, countType)
                        .flatMap(count -> Mono.just(new PageImpl<>(results, pageable, count))));
    }

    default <R> Mono<Page<R>> createPagedRequest(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final String tableName, final Class<R> resultType, final Pageable pageable) {
        Assert.notNull(selectSpecBuilder, "SelectSpecBuilder must not be null!");
        Assert.notNull(pageable, "Pageable most not be null!");

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper();
        if (getAccessStrategy().getConverter().getMappingContext().hasPersistentEntityFor(resultType)) {
            mapper = mapper.forType(resultType);
        }
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(tableName);

        selectSpec = selectSpecBuilder.apply(selectSpec);

        return createPagedRequest(selectSpec, tableName, resultType, pageable);
    }

    default <R> Mono<Page<R>> createPagedRequestWithCountType(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final String tableName, final Class<R> resultType, final Class<?> countType, final Pageable pageable) {
        Assert.notNull(selectSpecBuilder, "SelectSpecBuilder must not be null!");
        Assert.notNull(pageable, "Pageable most not be null!");

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper();
        if (getAccessStrategy().getConverter().getMappingContext().hasPersistentEntityFor(resultType)) {
            mapper = mapper.forType(resultType);
        }
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(tableName);

        selectSpec = selectSpecBuilder.apply(selectSpec);

        return createPagedRequestWithCountType(selectSpec, tableName, resultType, countType, pageable);
    }

    default <T> Mono<Page<T>> createPagedStarRequest(final SelectSpecWithJoin selectSpecWithJoin, final String tableName, final Class<T> resultType, final Pageable pageable) {
        return createFindStarRequest(selectSpecWithJoin, resultType, pageable)
                .collectList()
                .flatMap(results -> createCountRequest(selectSpecWithJoin, tableName, resultType)
                        .flatMap(count -> Mono.just(new PageImpl<>(results, pageable, count))));
    }

    default <T> Mono<Page<T>> createPagedStarRequest(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final String tableName, final Class<T> resultType, final Pageable pageable) {
        Assert.notNull(selectSpecBuilder, "SelectSpecBuilder must not be null!");
        Assert.notNull(pageable, "Pageable most not be null!");


        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper();
        if (getAccessStrategy().getConverter().getMappingContext().hasPersistentEntityFor(resultType)) {
            mapper = mapper.forType(resultType);
        }
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(tableName);

        selectSpec = selectSpecBuilder.apply(selectSpec);

        return createPagedStarRequest(selectSpec, tableName, resultType, pageable);
    }

    default <T> Mono<Page<T>> createPagedStarSingleWhereRequest(final String parameterName, final Object value, final String tableName, final Class<T> resultType, final Pageable pageable) {
        Assert.notNull(parameterName, "ParameterName must not be null!");
        Assert.notNull(value, "Value must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        return createPagedStarRequest(
                selectSpec -> selectSpec.withCriteria(where(Expressions.reference(parameterName)).is(parameter(value))),
                tableName,
                resultType,
                pageable);
    }

    default ColumnBasedCriteria nonNullAndMatchesCheckForWhere(@Nullable final ColumnBasedCriteria criteria, @Nullable final Object parameter, @NonNull final String tableName, @NonNull final String columnName) {
        if (parameter != null) {
            if (criteria == null) {
                return where(Expressions.reference(tableName, columnName)).matches(parameter(parameter));
            } else {
                return criteria.and(Expressions.reference(tableName, columnName)).matches(parameter(parameter));
            }
        }

        return criteria;
    }

    default ColumnBasedCriteria nonNullAndEqualsCheckForWhere(@Nullable final ColumnBasedCriteria criteria, @Nullable final Object parameter, @NonNull final String tableName, @NonNull final String columnName) {
        if (parameter != null) {
            if (criteria == null) {
                return where(Expressions.reference(tableName, columnName)).is(parameter(parameter));
            } else {
                return criteria.and(Expressions.reference(tableName, columnName)).is(parameter(parameter));
            }
        }

        return criteria;
    }

    default Collection<Expression> createSelectStatementsForCompoundEntity(final Class<?> compoundEntity) {
        return StreamSupport.stream(getAccessStrategy().getConverter().getMappingContext().getRequiredPersistentEntity(compoundEntity).spliterator(), false)
                .filter(PersistentProperty::isEntity)
                .flatMap(this::createSelectStatementsForRelationalProperty)
                .collect(Collectors.toList());
    }

    private Stream<Expression> createSelectStatementsForRelationalProperty(final RelationalPersistentProperty property) {
        return StreamSupport.stream(getAccessStrategy().getConverter().getMappingContext().getRequiredPersistentEntity(property.getActualType()).spliterator(), false)
                .map(persistentProperty -> aliased(reference(persistentProperty.getOwner().getTableName(), persistentProperty.getColumnName()),
                        String.format("%s_%s", property.getName(), persistentProperty.getColumnName())));
    }

    default String getIdColumnName(final Class<?> entityType) {

        return this.getConverter() //
                .getMappingContext() //
                .getRequiredPersistentEntity(entityType) //
                .getRequiredIdProperty() //
                .getColumnName();
    }
}
