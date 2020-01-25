package org.modmappings.mmms.repository.repositories.core.release;

import java.util.List;
import java.util.UUID;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.r2dbc.query.Criteria;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.relational.core.sql.Functions;
import org.springframework.data.relational.core.sql.Table;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * Represents a repository which can provide and store {@link ReleaseComponentDMO} objects.
 */
@Repository
public class ReleaseComponentRepository extends ModMappingR2DBCRepository<ReleaseComponentDMO> {

    public ReleaseComponentRepository(RelationalEntityInformation<ReleaseComponentDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @param pageable The paging information for the query.
     * @return The release components which are part of the release with the given id.
     */
    public Mono<Page<ReleaseComponentDMO>> findAllByReleaseId(final UUID releaseId, final Pageable pageable)
    {
        return createPagedSingleWhereRequest("release_id", releaseId, pageable);
    }

    /**
     * Finds all release component which target a mapping with the given id.
     *
     * @param mappingId The id of the mapping that the components are being looked up for.
     * @param pageable The paging information for the query.
     * @return The release components which target the mapping with the given id.
     */
    public Mono<Page<ReleaseComponentDMO>> findAllByMappingId(final UUID mappingId, final Pageable pageable)
    {
        return createPagedSingleWhereRequest("mapping_id", mappingId, pageable);
    }

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a package.
     *
     * @param releaseId The id of the release to get the package mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a package which are part of the given release.
     */
    public Mono<Page<UUID>> findAllMappingIdsByReleaseIdForPackage(final UUID releaseId, final Pageable pageable)
    {
        return createPagedMappingIdsByReleaseIdForTypeRequest(releaseId, MappableTypeDMO.PACKAGE, pageable);
    }

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a class.
     *
     * @param releaseId The id of the release to get the class mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a class which are part of the given release.
     */
    public Mono<Page<UUID>> findAllMappingIdsByReleaseIdForClass(final UUID releaseId, final Pageable pageable)
    {
        return createPagedMappingIdsByReleaseIdForTypeRequest(releaseId, MappableTypeDMO.CLASS, pageable);
    }

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a method.
     *
     * @param releaseId The id of the release to get the method mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a method which are part of the given release.
     */
    public Mono<Page<UUID>> findAllMappingIdsByReleaseIdForMethod(final UUID releaseId, final Pageable pageable)
    {
        return createPagedMappingIdsByReleaseIdForTypeRequest(releaseId, MappableTypeDMO.METHOD, pageable);
    }

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a field.
     *
     * @param releaseId The id of the release to get the field mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a field which are part of the given release.
     */
    public Mono<Page<UUID>> findAllMappingIdsByReleaseIdForField(final UUID releaseId, final Pageable pageable)
    {
        return createPagedMappingIdsByReleaseIdForTypeRequest(releaseId, MappableTypeDMO.FIELD, pageable);
    }

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a parameter.
     *
     * @param releaseId The id of the release to get the parameter mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a parameter which are part of the given release.
     */
    public Mono<Page<UUID>> findAllMappingIdsByReleaseIdForParameter(final UUID releaseId, final Pageable pageable)
    {
        return createPagedMappingIdsByReleaseIdForTypeRequest(releaseId, MappableTypeDMO.PARAMETER, pageable);
    }

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping for a given mappable type.
     *
     * @param releaseId The id of the release to get the package mappings for.
     * @param mappableType The type of mappables to find the mapping ids for.
     * @param pageable The paging information for the query.
     * @return The mappings for a package which are part of the given release.
     */
    private Flux<UUID> createFindAllMappingIdsByReleaseIdForTypeRequest(final UUID releaseId, final MappableTypeDMO mappableType, final Pageable pageable) {
        Assert.notNull(releaseId, "ReleaseId must not be null!");
        Assert.notNull(pageable, "Pageable most not be null!");

        List<String> columns = this.getAccessStrategy().getAllColumns(this.getEntity().getJavaType());

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(UUID.class);
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(this.getEntity().getTableName()) //
                .withProjection(reference("mp", "mapping_id")) //
                .withJoin(
                        JoinSpec.join("mapping", "mp")
                                .withOn(on(reference("mapping_id")).is(reference("mp", "id"))),
                        JoinSpec.join("versioned_mappable", "vm")
                                .withOn(on(reference("versioned_mappable_id")).is(reference("vm", "id"))),
                        JoinSpec.join("mappable", "m")
                                .withOn(on(reference("vm", "mappable_id")).is(reference("m", "id")))
                )
                .withCriteria(
                        where(reference("release_id")).is(parameter(releaseId))
                                .and(reference("mappable", "type")).is(parameter(mappableType.ordinal())))
                .withPage(pageable);

        PreparedOperation<?> operation = mapper.getMappedObject(selectSpec);

        return this.getDatabaseClient().execute(operation) //
                .as(UUID.class) //
                .fetch()
                .all();
    }

    /**
     * Counts all mapping ids which are part of the given release and represent a mapping for a given mappable type.
     *
     * @param releaseId The id of the release to get the package mappings for.
     * @param mappableType The type of mappables to find the mapping ids for.
     * @param pageable The paging information for the query.
     * @return The mappings for a package which are part of the given release.
     */
    private Mono<Long> countAllMappingIdsByReleaseIdForTypeRequest(final UUID releaseId, final MappableTypeDMO mappableType, final Pageable pageable) {
        Assert.notNull(releaseId, "ReleaseId must not be null!");
        Assert.notNull(pageable, "Pageable most not be null!");

        Table table = Table.create(this.getEntity().getTableName());
        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(UUID.class);
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(this.getEntity().getTableName()) //
                .withProjection(spring(Functions.count(table.column(getIdColumnName())))) //
                .withJoin(
                        JoinSpec.join("mapping", "mp")
                                .withOn(on(reference("mapping_id")).is(reference("mp", "id"))),
                        JoinSpec.join("versioned_mappable", "vm")
                                .withOn(on(reference("versioned_mappable_id")).is(reference("vm", "id"))),
                        JoinSpec.join("mappable", "m")
                                .withOn(on(reference("vm", "mappable_id")).is(reference("m", "id")))
                )
                .withCriteria(
                        where(reference("release_id")).is(parameter(releaseId))
                                .and(reference("mappable", "type")).is(parameter(mappableType.ordinal())))
                .withPage(pageable);

        PreparedOperation<?> operation = mapper.getMappedObject(selectSpec);

        return this.getDatabaseClient().execute(operation) //
                .map((r, md) -> r.get(0, Long.class)) //
                .first() //
                .defaultIfEmpty(0L);
    }

    private Mono<Page<UUID>> createPagedMappingIdsByReleaseIdForTypeRequest(final UUID releaseId, final MappableTypeDMO mappableType, final Pageable pageable) {
        return createFindAllMappingIdsByReleaseIdForTypeRequest(releaseId, mappableType, pageable)
                .collectList()
                .flatMap(ids -> countAllMappingIdsByReleaseIdForTypeRequest(releaseId, mappableType, pageable)
                        .flatMap(count -> Mono.just(new PageImpl<>(ids, pageable, count))));
    }
}
