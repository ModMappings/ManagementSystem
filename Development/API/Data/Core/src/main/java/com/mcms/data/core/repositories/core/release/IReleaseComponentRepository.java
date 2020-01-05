package com.mcms.data.core.repositories.core.release;

import java.util.UUID;

import com.mcms.api.datamodel.core.release.ReleaseComponentDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link ReleaseComponentDMO} objects.
 */
public interface IReleaseComponentRepository extends IRepository<ReleaseComponentDMO> {

    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @return The release components which are part of the release with the given id.
     */
    Flux<ReleaseComponentDMO> findAllForRelease(UUID releaseId);

    /**
     * Finds all release component which target a mapping with the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param mappingId The id of the mapping that the components are being looked up for.
     * @return The release components which target the mapping with the given id.
     */
    Flux<ReleaseComponentDMO> findAllForMapping(UUID mappingId);
}
