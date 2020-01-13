package org.modmappings.mmms.repository.repositories.core.release;

import java.util.UUID;

import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link ReleaseComponentDMO} objects.
 */
@Repository
public interface IReleaseComponentRepository extends R2dbcRepository<ReleaseComponentDMO, UUID> {

    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @param pageable The pagination information for the query.
     * @return The release components which are part of the release with the given id.
     */
    @Query("SELECT * FROM release_component rc where rc.releaseId = $1")
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
    @Query("SELECT * FROM release_component rc where rc.mappingId = $1")
    Flux<ReleaseComponentDMO> findAllForMapping(UUID mappingId, final Pageable pageable);
}
