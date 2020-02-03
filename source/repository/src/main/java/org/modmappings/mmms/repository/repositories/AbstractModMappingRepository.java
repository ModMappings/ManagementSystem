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
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.r2dbc.repository.support.SimpleR2dbcRepository;
import org.springframework.data.relational.core.mapping.RelationalPersistentEntity;
import org.springframework.data.relational.core.sql.Functions;
import org.springframework.data.relational.core.sql.Table;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.data.relational.repository.support.MappingRelationalEntityInformation;
import org.springframework.data.repository.NoRepositoryBean;
import org.springframework.lang.NonNull;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.UUID;
import java.util.function.Consumer;
import java.util.function.Function;
import java.util.function.UnaryOperator;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * An extended variant of the {@link R2dbcRepository} that adds support
 * to get the standard methods of the {@link R2dbcRepository} in a pageable
 * variant.
 *
 * Also sets the type that the id field needs to be to {@link UUID}.
 *
 * @param <T> The type stored in the repository.
 */
@NoRepositoryBean
public abstract class AbstractModMappingRepository<T> extends SimpleR2dbcRepository<T, UUID> implements ModMappingRepository<T> {

    private final RelationalEntityInformation<T, UUID> entity;
    private final DatabaseClient databaseClient;
    private final R2dbcConverter converter;
    private final ExtendedDataAccessStrategy accessStrategy;

    public AbstractModMappingRepository(DatabaseClient databaseClient, ExtendedDataAccessStrategy accessStrategy, Class<T> entityClass) {
        super(new MappingRelationalEntityInformation<>((RelationalPersistentEntity<T>) accessStrategy.getConverter().getMappingContext().getRequiredPersistentEntity(entityClass)), databaseClient, accessStrategy.getConverter(), accessStrategy);
        this.entity = new MappingRelationalEntityInformation<>((RelationalPersistentEntity<T>) accessStrategy.getConverter().getMappingContext().getRequiredPersistentEntity(entityClass));
        this.databaseClient = databaseClient;
        this.converter = accessStrategy.getConverter();
        this.accessStrategy = accessStrategy;
    }

    protected RelationalEntityInformation<T, UUID> getEntity() {
        return entity;
    }

    protected DatabaseClient getDatabaseClient() {
        return databaseClient;
    }

    protected R2dbcConverter getConverter() {
        return converter;
    }

    protected ExtendedDataAccessStrategy getAccessStrategy() {
        return accessStrategy;
    }

    /**
     * Gets all entries in the repository.
     * Is identical to {@link #findAll()} but accepts page and sorting information.
     *
     * @param pageable The page and sorting information.
     * @return The page that is requested.
     */
    @Override
    public Mono<Page<T>> findAll(
            final Pageable pageable
    ) {
        return createPagedStarRequest(
                UnaryOperator.identity(),
                pageable
        );
    }

    protected Flux<T> createFindStarRequest(final SelectSpecWithJoin selectSpec, final Pageable pageable)
    {
        Assert.notNull(selectSpec, "SelectSpec must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        List<String> columns = this.getAccessStrategy().getAllColumns(this.getEntity().getJavaType());

        final SelectSpecWithJoin selectSpecWithProj = selectSpec
                .withProjectionFromColumnName(columns);

        return createFindRequest(selectSpecWithProj, this.getEntity().getJavaType(), pageable);
    }

    protected <R> Flux<R> createFindRequest(final SelectSpecWithJoin selectSpec, final Class<R> resultType, final Pageable pageable)
    {
        Assert.notNull(selectSpec, "SelectSpec must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        final SelectSpecWithJoin selectSpecWithPagination = selectSpec
                .withPage(pageable);

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        PreparedOperation<?> operation = mapper.getMappedObject(selectSpecWithPagination);

        return this.getDatabaseClient().execute(operation) //
                .as(resultType) //
                .fetch()
                .all();
    }

    protected Mono<Long> createCountRequest(final SelectSpecWithJoin selectSpec)
    {
        Assert.notNull(selectSpec, "SelectSpec must not be null");

        Table table = Table.create(this.entity.getTableName());
        final SelectSpecWithJoin selectSpecWithProj = selectSpec
                .setProjection(spring(Functions.count(table.column(getIdColumnName()))));

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        PreparedOperation<?> operation = mapper.getMappedObject(selectSpecWithProj);

        return this.getDatabaseClient().execute(operation) //
                .map((r, md) -> r.get(0, Long.class)) //
                .first() //
                .defaultIfEmpty(0L);
    }

    protected <R> Mono<Page<R>> createPagedRequest(final SelectSpecWithJoin selectSpecWithJoin, final Class<R> resultType, final Pageable pageable)
    {
        return createFindRequest(selectSpecWithJoin, resultType, pageable)
                .collectList()
                .flatMap(results -> createCountRequest(selectSpecWithJoin)
                        .flatMap(count -> Mono.just(new PageImpl<>(results, pageable, count))));
    }

    protected <R> Mono<Page<R>> createPagedRequest(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final Class<R> resultType, final Pageable pageable)
    {
        Assert.notNull(selectSpecBuilder, "SelectSpecBuilder must not be null!");
        Assert.notNull(pageable, "Pageable most not be null!");

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(this.getEntity().getTableName());

        selectSpec = selectSpecBuilder.apply(selectSpec);

        return createPagedRequest(selectSpec, resultType, pageable);
    }

    protected Mono<Page<T>> createPagedStarRequest(final SelectSpecWithJoin selectSpecWithJoin, final Pageable pageable)
    {
        return createFindStarRequest(selectSpecWithJoin, pageable)
                .collectList()
                .flatMap(results -> createCountRequest(selectSpecWithJoin)
                        .flatMap(count -> Mono.just(new PageImpl<>(results, pageable, count))));
    }

    protected Mono<Page<T>> createPagedStarRequest(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final Pageable pageable)
    {
        Assert.notNull(selectSpecBuilder, "SelectSpecBuilder must not be null!");
        Assert.notNull(pageable, "Pageable most not be null!");

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(this.getEntity().getTableName());

        selectSpec = selectSpecBuilder.apply(selectSpec);

        return createPagedStarRequest(selectSpec, pageable);
    }

    protected Mono<Page<T>> createPagedStarSingleWhereRequest(final String parameterName, final Object value, final Pageable pageable)
    {
        Assert.notNull(parameterName, "ParameterName must not be null!");
        Assert.notNull(value, "Value must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        return createPagedStarRequest(
                selectSpec -> selectSpec.withCriteria(where(reference(parameterName)).is(parameter(value))),
                pageable);
    }

    protected ColumnBasedCriteria nonNullAndMatchesCheckForWhere(@Nullable final ColumnBasedCriteria criteria, @Nullable Object parameter, @NonNull String tableName, @NonNull String columnName) {
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

    protected ColumnBasedCriteria nonNullAndEqualsCheckForWhere(@Nullable final ColumnBasedCriteria criteria, @Nullable Object parameter, @NonNull String tableName, @NonNull String columnName) {
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

    protected String getIdColumnName()
    {

        return this.converter //
                .getMappingContext() //
                .getRequiredPersistentEntity(this.entity.getJavaType()) //
                .getRequiredIdProperty() //
                .getColumnName();
    }
}
