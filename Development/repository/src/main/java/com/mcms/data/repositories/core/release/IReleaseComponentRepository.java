package com.mcms.data.repositories.core.release;

import java.util.UUID;

import com.mcms.data.model.core.release.ReleaseComponentDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link ReleaseComponentDMO} objects.
 */
public interface IReleaseComponentRepository extends CrudRepository<ReleaseComponentDMO, UUID> {

    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @param pageable The pagination information for the query.
     * @return The release components which are part of the release with the given id.
     */
    Flux<ReleaseComponentDMO> findAllForRelease(UUID releaseId, final Pageable pageable);

    /**
     * Finds all release component which target a mapping with the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param mappingId The id of the mapping that the components are being looked up for.
     * @param pageable The pagination information for the query.
     * @return The release components which target the mapping with the given id.
     */
    Flux<ReleaseComponentDMO> findAllForMapping(UUID mappingId, final Pageable pageable);
}
