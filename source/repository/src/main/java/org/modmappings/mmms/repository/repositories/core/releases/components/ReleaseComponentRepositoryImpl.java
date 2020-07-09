package org.modmappings.mmms.repository.repositories.core.releases.components;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
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

    public ReleaseComponentRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
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
                        .distinct()
                        .withProjection(Expressions.reference("mp", "id")) //
                        .withJoin(
                                JoinSpec.join("mapping", "mp")
                                        .withOn(on(Expressions.reference("mapping_id")).is(Expressions.reference("mp", "id"))),
                                JoinSpec.join("versioned_mappable", "vm")
                                        .withOn(on(Expressions.reference("mp","versioned_mappable_id")).is(Expressions.reference("vm", "id"))),
                                JoinSpec.join("mappable", "m")
                                        .withOn(on(Expressions.reference("vm", "mappable_id")).is(Expressions.reference("m", "id")))
                        )
                        .withCriteria(
                                where(Expressions.reference("release_id")).is(Expressions.parameter(releaseId))
                                        .and(Expressions.reference("m", "type")).is(Expressions.parameter(mappableType.ordinal()))),
                getTableName(),
                UUID.class,
                pageable
        );
    }
}
