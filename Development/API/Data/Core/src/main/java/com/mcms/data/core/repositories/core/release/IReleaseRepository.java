package com.mcms.data.core.repositories.core.release;

import java.util.UUID;

import com.mcms.api.datamodel.core.release.ReleaseDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link ReleaseDMO} objects.
 */
public interface IReleaseRepository extends IRepository<ReleaseDMO> {
    /**
     * Finds all releases which match the given name regex.
     *
     * The releases are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup releases for.
     * @return The releases of which the name match the regex.
     */
    Flux<ReleaseDMO> findAllForNameRegex(String nameRegex);

    /**
     * Finds all releases which where directly made by the game version with the given id.
     *
     * The releases are sorted newest to oldest.
     *
     * @param gameVersionId The id of the game version to look up releases for.
     * @return The releases by the given game version.
     * @throws IllegalArgumentException in case the given {@literal gameVersionId} is {@literal null}.
     */
    Flux<ReleaseDMO> findAllForGameVersion(final UUID gameVersionId);

    /**
     * Finds all releases which where directly made by the mapping type with the given id.
     *
     * The releases are sorted newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to look up releases for.
     * @return The releases by the given mapping type.
     * @throws IllegalArgumentException in case the given {@literal mappingTypeId} is {@literal null}.
     */
    Flux<ReleaseDMO> findAllForMappingType(final UUID mappingTypeId);

    /**
     * Finds all releases which are a snapshot.
     *
     * @return The releases which are marked as being a snapshot.
     */
    Flux<ReleaseDMO> findAllSnapshots();

    /**
     * Finds all releases which where directly made by the user with the given id.
     *
     * The releases are sorted newest to oldest.
     *
     * @param userId The id of the user to look up releases for.
     * @return The releases by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<ReleaseDMO> findAllForUser(final UUID userId);
}
