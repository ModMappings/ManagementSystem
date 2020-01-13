package org.modmapping.mmms.repository.repositories.core.release;

import java.util.UUID;

import org.modmapping.mmms.repository.model.core.release.ReleaseDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link ReleaseDMO} objects.
 */
@Repository
public interface IReleaseRepository extends R2dbcRepository<ReleaseDMO, UUID> {
    /**
     * Finds all releases which match the given name regex.
     *
     * The releases are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup releases for.
     * @param pageable The pagination information for the query.
     * @return The releases of which the name match the regex.
     */
    @Query("SELECT * FROM release r WHERE r.name regexp $1")
    Flux<ReleaseDMO> findAllForNameRegex(String nameRegex, final Pageable pageable);

    /**
     * Finds all releases which where directly made by the game version with the given id.
     *
     * The releases are sorted newest to oldest.
     *
     * @param gameVersionId The id of the game version to look up releases for.
     * @param pageable The pagination information for the query.
     * @return The releases by the given game version.
     * @throws IllegalArgumentException in case the given {@literal gameVersionId} is {@literal null}.
     */
    @Query("SELECT * FROM release r WHERE r.gameVersionId = $1")
    Flux<ReleaseDMO> findAllForGameVersion(final UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all releases which where directly made by the mapping type with the given id.
     *
     * The releases are sorted newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to look up releases for.
     * @param pageable The pagination information for the query.
     * @return The releases by the given mapping type.
     * @throws IllegalArgumentException in case the given {@literal mappingTypeId} is {@literal null}.
     */
    @Query("SELECT * FROM release r WHERE r.mappingTypeId = $1")
    Flux<ReleaseDMO> findAllForMappingType(final UUID mappingTypeId, final Pageable pageable);

    /**
     * Finds all releases which are a snapshot.
     *
     * The releases are sorted newest to oldest.
     *
     * @param pageable The pagination information for the query.
     * @return The releases which are marked as being a snapshot.
     */
    @Query("SELECT * FROM release r where r.isSnapshot is true")
    Flux<ReleaseDMO> findAllSnapshots(final Pageable pageable);

    /**
     * Finds all release which the mapping with the given id is part of.
     *
     * The releases are sorted newest to oldest.
     *
     * @param mappingId The id of the mapping for which releases are being looked up for.
     * @param pageable The pagination information for the query.
     * @return The releases which have the mapping with the given id as component.
     */
    @Query("SELECT r.* FROM release r JOIN release_component rc ON r.id = rc.releaseId WHERE rc.mappingId = $1")
    Flux<ReleaseDMO> findAllForMapping(UUID mappingId, final Pageable pageable);

    /**
     * Finds all releases which where directly made by the user with the given id.
     *
     * The releases are sorted newest to oldest.
     *
     * @param userId The id of the user to look up releases for.
     * @param pageable The pagination information for the query.
     * @return The releases by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    @Query("SELECT * FROM release r WHERE r.createdBy = userId")
    Flux<ReleaseDMO> findAllForUser(final UUID userId, final Pageable pageable);
}
