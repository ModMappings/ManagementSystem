package org.modmappings.mmms.repository.repositories.mapping.mappings.proposedmapping;

import org.modmappings.mmms.repository.model.mapping.mappings.ProposedMappingDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository that gives access to proposed mappings via
 * its custom accessor methods.
 *
 * Defines lookup of proposed mappings via their properties.
 */
public interface ProposedMappingRepositoryCustom {
    /**
     * Finds all proposed mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the proposed mappings for.
     * @param pageable The paging and sorting information.
     * @return The proposed mappings for the given versioned mappable.
     */
    Mono<Page<ProposedMappingDMO>> findAllForVersionedMappableAndStateAndMerged(UUID versionedMappableId, Boolean state, Boolean merged, Pageable pageable);
}
