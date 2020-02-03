package org.modmappings.mmms.repository.repositories.core.releases.components;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * Represents a repository which can provide and store {@link ReleaseComponentDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class ReleaseComponentRepositoryImpl extends AbstractModMappingRepository<ReleaseComponentDMO> implements ReleaseComponentRepository {

    public ReleaseComponentRepositoryImpl(DatabaseClient databaseClient, ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, ReleaseComponentDMO.class);
    }

    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @param pageable The paging information for the query.
     * @return The release components which are part of the release with the given id.
     */
    @Override
    public Mono<Page<ReleaseComponentDMO>> findAllByReleaseId(final UUID releaseId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("release_id", releaseId, pageable);
    }

    /**
     * Finds all release component which target a mapping with the given id.
     *
     * @param mappingId The id of the mapping that the components are being looked up for.
     * @param pageable The paging information for the query.
     * @return The release components which target the mapping with the given id.
     */
    @Override
    public Mono<Page<ReleaseComponentDMO>> findAllByMappingId(final UUID mappingId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("mapping_id", mappingId, pageable);
    }

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a package.
     *
     * @param releaseId The id of the release to get the package mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a package which are part of the given release.
     */
    @Override
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
    @Override
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
    @Override
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
    @Override
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
    @Override
    public Mono<Page<UUID>> findAllMappingIdsByReleaseIdForParameter(final UUID releaseId, final Pageable pageable)
    {
        return createPagedMappingIdsByReleaseIdForTypeRequest(releaseId, MappableTypeDMO.PARAMETER, pageable);
    }

    private Mono<Page<UUID>> createPagedMappingIdsByReleaseIdForTypeRequest(final UUID releaseId, final MappableTypeDMO mappableType, final Pageable pageable) {
        return createPagedRequest(
                selectSpec -> selectSpec
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
                                        .and(reference("mappable", "type")).is(parameter(mappableType.ordinal()))),
                UUID.class,
                pageable
        );
    }
}
