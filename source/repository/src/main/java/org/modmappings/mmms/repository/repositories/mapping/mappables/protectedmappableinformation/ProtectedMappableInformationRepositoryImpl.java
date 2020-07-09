package org.modmappings.mmms.repository.repositories.mapping.mappables.protectedmappableinformation;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.model.mapping.mappable.ProtectedMappableInformationDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

/**
 * Represents a repository that can provide and store {@link ProtectedMappableInformationDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class ProtectedMappableInformationRepositoryImpl extends AbstractModMappingRepository<ProtectedMappableInformationDMO> implements ProtectedMappableInformationRepository {

    public ProtectedMappableInformationRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, ProtectedMappableInformationDMO.class);
    }

    /**
     * Finds all the protected versioned mappable information which indicate that a given versioned mappable is locked
     * for mapping types.
     *
     * @param versionedMappableId The id of the versioned mappable for which protected mappable information is being looked up.
     * @param pageable            The pageable information for request.
     * @return Protected mappable information that indicates that the versioned mappable is locked for a given mapping type.
     */
    @Override
    public Mono<Page<ProtectedMappableInformationDMO>> findAllByVersionedMappable(
            final UUID versionedMappableId,
            final Pageable pageable
    ) {
        return createPagedStarSingleWhereRequest("versioned_mappable_id", versionedMappableId, pageable);
    }

    /**
     * Finds all the protected versioned mappable information which indicate that a given mapping type is locked
     * for versioned mappables.
     *
     * @param mappingTypeId The id of the mapping type for which protected mappable information is being looked up.
     * @param pageable      The paging and sorting information.
     * @return Protected mappable information that indicates that the mapping type is locked for a given versioned mappable.
     */
    @Override
    public Mono<Page<ProtectedMappableInformationDMO>> findAllByMappingType(
            final UUID mappingTypeId,
            final Pageable pageable
    ) {
        return createPagedStarSingleWhereRequest("mapping_type_id", mappingTypeId, pageable);
    }
}
