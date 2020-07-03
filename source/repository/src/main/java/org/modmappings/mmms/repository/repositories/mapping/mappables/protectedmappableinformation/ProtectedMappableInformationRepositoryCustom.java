package org.modmappings.mmms.repository.repositories.mapping.mappables.protectedmappableinformation;

import org.modmappings.mmms.repository.model.mapping.mappable.ProtectedMappableInformationDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository for protection information on mappables.
 *
 * Allows quick and custom lookups of all protection information for a given versioned mappable or mapping type.
 */
public interface ProtectedMappableInformationRepositoryCustom {
    /**
     * Finds all the protected versioned mappable information which indicate that a given versioned mappable is locked
     * for mapping types.
     *
     * @param versionedMappableId The id of the versioned mappable for which protected mappable information is being looked up.
     * @param pageable The pageable information for request.
     * @return Protected mappable information that indicates that the versioned mappable is locked for a given mapping type.
     */
    Mono<Page<ProtectedMappableInformationDMO>> findAllByVersionedMappable(
            UUID versionedMappableId,
            Pageable pageable
    );

    /**
     * Finds all the protected versioned mappable information which indicate that a given mapping type is locked
     * for versioned mappables.
     *
     * @param mappingTypeId The id of the mapping type for which protected mappable information is being looked up.
     * @param pageable The paging and sorting information.
     * @return Protected mappable information that indicates that the mapping type is locked for a given versioned mappable.
     */
    Mono<Page<ProtectedMappableInformationDMO>> findAllByMappingType(
            UUID mappingTypeId,
            Pageable pageable
    );
}
