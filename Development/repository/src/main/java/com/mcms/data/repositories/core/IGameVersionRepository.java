package com.mcms.data.repositories.core;

import java.util.UUID;

import com.mcms.data.model.core.GameVersionDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link GameVersionDMO} objects.
 */
public interface IGameVersionRepository extends CrudRepository<GameVersionDMO, UUID> {

    /**
     * Finds all game versions which match the given name regex.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup game versions for.
     * @param pageable The pagination information for the query.
     * @return The game versions of which the name match the regex.
     */
    Flux<GameVersionDMO> findAllForNameRegex(String nameRegex, final Pageable pageable);

    /**
     * Finds all game versions which are normal full releases.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param pageable The pagination information for the query.
     * @return The game versions which are neither marked as snapshot nor pre-release.
     */
    Flux<GameVersionDMO> findAllReleases(final Pageable pageable);

    /**
     * Finds all game versions which are pre-releases.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param pageable The pagination information for the query.
     * @return The game versions which are marked as being a pre-release.
     */
    Flux<GameVersionDMO> findAllPreReleases(final Pageable pageable);

    /**
     * Finds all game versions which are snapshots.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param pageable The pagination information for the query.
     * @return The game versions which are marked as being a snapshot.
     */
    Flux<GameVersionDMO> findAllSnapshots(final Pageable pageable);

    /**
     * Finds all game versions which where directly made by the user with the given id.
     *
     * The game versions are sorted newest to oldest.
     *
     * @param userId The id of the user to look up game versions for.
     * @param pageable The pagination information for the query.
     * @return The game versions by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<GameVersionDMO> findAllForUser(final UUID userId, final Pageable pageable);
}
