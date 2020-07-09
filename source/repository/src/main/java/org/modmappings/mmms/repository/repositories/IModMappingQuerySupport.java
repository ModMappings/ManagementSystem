package org.modmappings.mmms.repository.repositories;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.relational.core.sql.Column;
import org.springframework.data.relational.core.sql.Expressions;
import org.springframework.data.relational.core.sql.Functions;
import org.springframework.data.relational.core.sql.Table;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.lang.NonNull;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.UUID;
import java.util.function.UnaryOperator;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;
import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.parameter;

public interface IModMappingQuerySupport {

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

    default <T> Flux<T> createFindStarRequest(final SelectSpecWithJoin selectSpec, final Class<T> resultType, final Pageable pageable)
    {
        Assert.notNull(selectSpec, "SelectSpec must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        final List<String> columns = this.getAccessStrategy().getAllColumns(resultType);

        final SelectSpecWithJoin selectSpecWithProj = selectSpec
                .withProjectionFromColumnName(columns);

        return createFindRequest(selectSpecWithProj, resultType, pageable);
    }

    default <R> Flux<R> createFindRequest(final SelectSpecWithJoin selectSpec, final Class<R> resultType, final Pageable pageable)
    {
        Assert.notNull(selectSpec, "SelectSpec must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        final SelectSpecWithJoin selectSpecWithPagination = selectSpec
                .withPage(pageable);

        
        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper();
        if (getAccessStrategy().getConverter().getMappingContext().hasPersistentEntityFor(resultType)) {
            mapper = mapper.forType(resultType);
        }
        final PreparedOperation<?> operation = mapper.getMappedObject(selectSpecWithPagination);

        return this.getDatabaseClient().execute(operation) //
                .as(resultType) //
                .fetch()
                .all();
    }

    default Mono<Long> createCountRequest(final SelectSpecWithJoin selectSpec, final String tableName, final Class<?> resultType)
    {
        Assert.notNull(selectSpec, "SelectSpec must not be null");

        final Table table = Table.create(tableName);
        final Column column = table.column(getIdColumnName(resultType));
        final SelectSpecWithJoin selectSpecWithProj =
                selectSpec.isDistinct() ? selectSpec
                        .notDistinct()
                        .setProjection(spring(Functions.count(Expressions.just(String.format("DISTINCT %s", column))))) :
                        selectSpec
                                .setProjection(spring(Functions.count(column)));

        
        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper();
        if (getAccessStrategy().getConverter().getMappingContext().hasPersistentEntityFor(resultType)) {
            mapper = mapper.forType(resultType);
        }
        final PreparedOperation<?> operation = mapper.getMappedObject(selectSpecWithProj);

        return this.getDatabaseClient().execute(operation) //
                .map((r, md) -> r.get(0, Long.class)) //
                .first() //
                .defaultIfEmpty(0L);
    }

    default <R> Mono<Page<R>> createPagedRequest(final SelectSpecWithJoin selectSpecWithJoin, final String tableName, final Class<R> resultType, final Pageable pageable)
    {
        return createFindRequest(selectSpecWithJoin, resultType, pageable)
                .collectList()
                .flatMap(results -> createCountRequest(selectSpecWithJoin, tableName, resultType)
                        .flatMap(count -> Mono.just(new PageImpl<>(results, pageable, count))));
    }

    default <R> Mono<Page<R>> createPagedRequest(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final String tableName, final Class<R> resultType, final Pageable pageable)
    {
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

    default <T> Mono<Page<T>> createPagedStarRequest(final SelectSpecWithJoin selectSpecWithJoin, final String tableName, final Class<T> resultType, final Pageable pageable)
    {
        return createFindStarRequest(selectSpecWithJoin, resultType, pageable)
                .collectList()
                .flatMap(results -> createCountRequest(selectSpecWithJoin, tableName, resultType)
                        .flatMap(count -> Mono.just(new PageImpl<>(results, pageable, count))));
    }

    default <T> Mono<Page<T>> createPagedStarRequest(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final String tableName, final Class<T> resultType, final Pageable pageable)
    {
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

    default <T> Mono<Page<T>> createPagedStarSingleWhereRequest(final String parameterName, final Object value, final String tableName, final Class<T> resultType, final Pageable pageable)
    {
        Assert.notNull(parameterName, "ParameterName must not be null!");
        Assert.notNull(value, "Value must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        return createPagedStarRequest(
                selectSpec -> selectSpec.withCriteria(where(reference(parameterName)).is(parameter(value))),
                tableName,
                resultType,
                pageable);
    }

    default ColumnBasedCriteria nonNullAndMatchesCheckForWhere(@Nullable final ColumnBasedCriteria criteria, @Nullable final Object parameter, @NonNull final String tableName, @NonNull final String columnName) {
        if (parameter != null) {
            if (criteria == null) {
                return where(reference(tableName, columnName)).matches(parameter(parameter));
            }
            else
            {
                return criteria.and(reference(tableName, columnName)).matches(parameter(parameter));
            }
        }

        return criteria;
    }

    default ColumnBasedCriteria nonNullAndEqualsCheckForWhere(@Nullable final ColumnBasedCriteria criteria, @Nullable final Object parameter, @NonNull final String tableName, @NonNull final String columnName) {
        if (parameter != null) {
            if (criteria == null) {
                return where(reference(tableName, columnName)).is(parameter(parameter));
            }
            else
            {
                return criteria.and(reference(tableName, columnName)).is(parameter(parameter));
            }
        }

        return criteria;
    }

    default String getIdColumnName(final Class<?> entityType)
    {

        return this.getConverter() //
                .getMappingContext() //
                .getRequiredPersistentEntity(entityType) //
                .getRequiredIdProperty() //
                .getColumnName();
    }
}
