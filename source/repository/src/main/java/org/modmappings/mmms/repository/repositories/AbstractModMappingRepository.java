package org.modmappings.mmms.repository.repositories;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.r2dbc.repository.support.SimpleR2dbcRepository;
import org.springframework.data.relational.core.mapping.RelationalPersistentEntity;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.data.relational.repository.support.MappingRelationalEntityInformation;
import org.springframework.data.repository.NoRepositoryBean;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;
import java.util.function.UnaryOperator;

/**
 * An extended variant of the {@link R2dbcRepository} that adds support
 * to get the standard methods of the {@link R2dbcRepository} in a pageable
 * variant.
 * <p>
 * Also sets the type that the id field needs to be to {@link UUID}.
 *
 * @param <T> The type stored in the repository.
 */
@NoRepositoryBean
public abstract class AbstractModMappingRepository<T> extends SimpleR2dbcRepository<T, UUID> implements ModMappingRepository<T>, IModMappingQuerySupport {

    private final RelationalEntityInformation<T, UUID> entity;
    private final DatabaseClient databaseClient;
    private final R2dbcConverter converter;
    private final ExtendedDataAccessStrategy accessStrategy;

    public AbstractModMappingRepository(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy, final Class<T> entityClass) {
        super(new MappingRelationalEntityInformation<>((RelationalPersistentEntity<T>) accessStrategy.getConverter().getMappingContext().getRequiredPersistentEntity(entityClass)), databaseClient, accessStrategy.getConverter(), accessStrategy);
        this.entity = new MappingRelationalEntityInformation<>((RelationalPersistentEntity<T>) accessStrategy.getConverter().getMappingContext().getRequiredPersistentEntity(entityClass));
        this.databaseClient = databaseClient;
        this.converter = accessStrategy.getConverter();
        this.accessStrategy = accessStrategy;
    }

    protected RelationalEntityInformation<T, UUID> getEntity() {
        return entity;
    }

    public DatabaseClient getDatabaseClient() {
        return databaseClient;
    }

    public R2dbcConverter getConverter() {
        return converter;
    }

    public ExtendedDataAccessStrategy getAccessStrategy() {
        return accessStrategy;
    }

    public String getTableName() {
        return this.getEntity().getTableName();
    }

    public Class<T> getEntityType() {
        return this.getEntity().getJavaType();
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
        return this.findAll(
                getTableName(),
                getEntityType(),
                pageable
        );
    }

    public Flux<T> createFindStarRequest(final SelectSpecWithJoin selectSpec, final Pageable pageable) {
        return this.createFindStarRequest(
                selectSpec,
                getEntityType(),
                pageable
        );
    }

    public Mono<Long> createCountRequest(final SelectSpecWithJoin selectSpec) {
        return this.createCountRequest(
                selectSpec,
                getTableName(),
                getEntityType()
        );
    }

    protected Mono<Page<T>> createPagedStarRequest(final SelectSpecWithJoin selectSpecWithJoin, final Pageable pageable) {
        return this.createPagedStarRequest(
                selectSpecWithJoin,
                getTableName(),
                getEntityType(),
                pageable
        );
    }

    public Mono<Page<T>> createPagedStarRequest(final UnaryOperator<SelectSpecWithJoin> selectSpecBuilder, final Pageable pageable) {
        return this.createPagedStarRequest(
                selectSpecBuilder,
                getTableName(),
                getEntityType(),
                pageable
        );
    }

    public Mono<Page<T>> createPagedStarSingleWhereRequest(final String parameterName, final Object value, final Pageable pageable) {
        return this.createPagedStarSingleWhereRequest(
                parameterName,
                value,
                getTableName(),
                getEntityType(),
                pageable
        );
    }

    protected String getIdColumnName() {
        return this.getIdColumnName(this.getEntityType());
    }
}
