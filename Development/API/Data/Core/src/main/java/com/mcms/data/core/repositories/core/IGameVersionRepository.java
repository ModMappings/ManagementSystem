package com.mcms.data.core.repositories.core;

import java.util.UUID;

import com.mcms.api.datamodel.core.GameVersionDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link GameVersionDMO} objects.
 */
public interface IGameVersionRepository extends IRepository<GameVersionDMO> {

    /**
     * Finds all game versions which match the given name regex.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup game versions for.
     * @return The game versions of which the name match the regex.
     */
    Flux<GameVersionDMO> findAllForNameRegex(String nameRegex);

    /**
     * Finds all game versions which are normal full releases.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @return The game versions which are neither marked as snapshot nor pre-release.
     */
    Flux<GameVersionDMO> findAllReleases();

    /**
     * Finds all game versions which are pre-releases.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @return The game versions which are marked as being a pre-release.
     */
    Flux<GameVersionDMO> findAllPreReleases();

    /**
     * Finds all game versions which are snapshots.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @return The game versions which are marked as being a snapshot.
     */
    Flux<GameVersionDMO> findAllSnapshots();

    /**
     * Finds all game versions which where directly made by the user with the given id.
     *
     * The game versions are sorted newest to oldest.
     *
     * @param userId The id of the user to look up game versions for.
     * @return The game versions by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<GameVersionDMO> findAllForUser(final UUID userId);
}
