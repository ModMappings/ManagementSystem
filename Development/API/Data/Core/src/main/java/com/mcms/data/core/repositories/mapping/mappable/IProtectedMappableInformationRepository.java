package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.ProtectedMappableInformationDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository that can provide and store {@link ProtectedMappableInformationDMO} objects.
 */
public interface IProtectedMappableInformationRepository extends CrudRepository<ProtectedMappableInformationDMO, UUID> {

    /**
     * Finds all the protected mappable in game version information which indicate that a given mappable in game version is locked
     * for mapping types.
     *
     * The returned order cannot be guaranteed.
     *
     * @param mappableInGameVersionId The id of the mappable in game version for which protected mappable information is being looked up.
     * @param pageable The pagination information for the query.
     * @return Protected mappable information that indicates that the mappable in game version is locked for a given mapping type.
     */
    Flux<ProtectedMappableInformationDMO> findAllForMappableInGameVersion(UUID mappableInGameVersionId, final Pageable pageable);

    /**
     * Finds all the protected mappable in game version information which indicate that a given mapping type is locked
     * for mappables in game version.
     *
     * The returned order cannot be guaranteed.
     *
     * @param mappingTypeId The id of the mapping type for which protected mappable information is being looked up.
     * @param pageable The pagination information for the query.
     * @return Protected mappable information that indicates that the mapping type is locked for a given mappable in game version.
     */
    Flux<ProtectedMappableInformationDMO> findAllForMappingType(UUID mappingTypeId, final Pageable pageable);
}
