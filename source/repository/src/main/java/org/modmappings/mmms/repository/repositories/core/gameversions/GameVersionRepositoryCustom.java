package org.modmappings.mmms.repository.repositories.core.gameversions;

import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

/**
 * Defines a repository that gives access to game versions via custom
 * accessor methods.
 * <p>
 * This custom repository takes the properties of the game version into account
 * and allows for the look up via them (possibly with optional parameters).
 */
public interface GameVersionRepositoryCustom {

    /**
     * Finds all game versions which match the given search criteria.
     *
     * @param nameRegex  The regular expression used to lookup game versions for.
     * @param preRelease Indicates if prerelease are supposed to be filtered out or included, null indicates do not care.
     * @param snapshot   Indicates if snapshots are supposed to be filtered out or included, null indicates do not care.
     * @param pageable   The paging information for the request.s
     * @return The game versions which match the given search criteria.
     */
    Mono<Page<GameVersionDMO>> findAllBy(
            String nameRegex,
            Boolean preRelease,
            Boolean snapshot,
            Pageable pageable
    );
}
