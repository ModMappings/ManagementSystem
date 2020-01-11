package org.modmapping.mmms.repository.repositories.core;

import java.util.UUID;

import org.modmapping.mmms.repository.model.core.GameVersionDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
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
    @Query("SELECT * FROM game_version gv WHERE gv.name regexp $1")
    Flux<GameVersionDMO> findAllForNameRegex(String nameRegex, final Pageable pageable);

    /**
     * Finds all game versions which are normal full releases.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param pageable The pagination information for the query.
     * @return The game versions which are neither marked as snapshot nor pre-release.
     */
    @Query("SELECT * FROM game_version gv WHERE gv.isPreRelease = false AND gv.isSnapshot = false")
    Flux<GameVersionDMO> findAllReleases(final Pageable pageable);

    /**
     * Finds all game versions which are pre-releases.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param pageable The pagination information for the query.
     * @return The game versions which are marked as being a pre-release.
     */
    @Query("SELECT * FROM game_version gv WHERE gv.isPreRelease = true")
    Flux<GameVersionDMO> findAllPreReleases(final Pageable pageable);

    /**
     * Finds all game versions which are snapshots.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param pageable The pagination information for the query.
     * @return The game versions which are marked as being a snapshot.
     */
    @Query("SELECT * FROM game_version gv WHERE gv.isSnapshot = true")
    Flux<GameVersionDMO> findAllSnapshots(final Pageable pageable);
}
