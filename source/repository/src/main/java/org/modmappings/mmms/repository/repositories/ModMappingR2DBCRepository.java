package org.modmappings.mmms.repository.repositories;

import java.util.List;
import java.util.UUID;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.r2dbc.core.ReactiveDataAccessStrategy;
import org.springframework.data.r2dbc.core.StatementMapper;
import org.springframework.data.r2dbc.query.Criteria;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.r2dbc.repository.support.SimpleR2dbcRepository;
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.data.repository.NoRepositoryBean;
import org.springframework.util.Assert;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

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
public class ModMappingR2DBCRepository<T> extends SimpleR2dbcRepository<T, UUID> implements R2dbcRepository<T, UUID> {

    private final RelationalEntityInformation<T, UUID> entity;
    private final DatabaseClient databaseClient;
    private final R2dbcConverter converter;
    private final ExtendedDataAccessStrategy accessStrategy;

    public ModMappingR2DBCRepository(RelationalEntityInformation<T, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
        this.entity = entity;
        this.databaseClient = databaseClient;
        this.converter = converter;
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

    protected Flux<T> createSelectWithSingleWhereRequest(final String parameterName, final Object value, final Pageable pageable)
    {
        Assert.notNull(parameterName, "ParameterName must not be null!");
        Assert.notNull(value, "Value must not be null");
        Assert.notNull(pageable, "Pageable most not be null!");

        List<String> columns = this.getAccessStrategy().getAllColumns(this.getEntity().getJavaType());

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        StatementMapper.SelectSpec selectSpec = mapper.createSelect(this.getEntity().getTableName()) //
                .withProjection(columns) //
                .withCriteria(Criteria.where(parameterName).is(value))
                .withPage(pageable);

        PreparedOperation<?> operation = mapper.getMappedObject(selectSpec);

        return this.getDatabaseClient().execute(operation) //
                .as(this.getEntity().getJavaType()) //
                .fetch()
                .all();
    }

    protected Mono<Long> createTotalCountWithSingleWhereRequest(final String parameterName, final Object value)
    {
        Assert.notNull(parameterName, "ParameterName must not be null!");
        Assert.notNull(value, "Value must not be null");

        Table table = Table.create(this.entity.getTableName());
        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(this.getEntity().getTableName())
                .withProjection(spring(Functions.count(table.column(getIdColumnName()))))
                .withCriteria(where(reference(parameterName)).is(parameter(value)));

        PreparedOperation<?> operation = mapper.getMappedObject(selectSpec);

        return this.getDatabaseClient().execute(operation) //
                .map((r, md) -> r.get(0, Long.class)) //
                .first() //
                .defaultIfEmpty(0L);
    }

    protected Mono<Page<T>> createPagedSingleWhereRequest(final String parameterName, final Object value, final Pageable pageable)
    {
        return createSelectWithSingleWhereRequest(parameterName, value, pageable)
                .collectList()
                .flatMap(pageResults -> createTotalCountWithSingleWhereRequest(parameterName, value)
                        .flatMap(count -> Mono.just(new PageImpl<>(pageResults, pageable, count))));
    }

    protected String getIdColumnName() {

        return this.converter //
                .getMappingContext() //
                .getRequiredPersistentEntity(this.entity.getJavaType()) //
                .getRequiredIdProperty() //
                .getColumnName();
    }
}
