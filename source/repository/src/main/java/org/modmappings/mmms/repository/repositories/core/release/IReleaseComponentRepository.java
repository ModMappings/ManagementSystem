package org.modmappings.mmms.repository.repositories.core.release;

import java.util.UUID;

import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.repositories.IPageableR2DBCRepository;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link ReleaseComponentDMO} objects.
 */
@Repository
public interface IReleaseComponentRepository extends IPageableR2DBCRepository<ReleaseComponentDMO> {

    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @return The release components which are part of the release with the given id.
     */
    @Query("SELECT * FROM release_component rc where rc.releaseId = $1")
    Flux<ReleaseComponentDMO> findAllForRelease(UUID releaseId);

    /**
     * Finds all release component which target a mapping with the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param mappingId The id of the mapping that the components are being looked up for.
     * @return The release components which target the mapping with the given id.
     */
    @Query("SELECT * FROM release_component rc where rc.mappingId = $1")
    Flux<ReleaseComponentDMO> findAllForMapping(UUID mappingId);

    @Override
    @Query("Select * from release_component rc")
    Flux<ReleaseComponentDMO> findAll();
}
