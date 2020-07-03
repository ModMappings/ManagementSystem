package org.modmappings.mmms.repository.repositories.core.releases.release;

import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.lang.NonNull;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository that allows access to releases via its
 * custom accessor methods.
 *
 * In particular methods in this repository take the external visibility of a release into account.
 */
public interface ReleaseRepositoryCustom {

    /**
     * Finds a given release by id.
     * Takes external visibility of the releases targeted mapping type into account.
     *
     * If {@literal false} is provided to the {@literal externallyVisibleOnly}-Parameter then this
     * method behaves exactly as the {@link org.springframework.data.repository.reactive.ReactiveCrudRepository#findById(Object)} method if
     * the same id is passed in as parameter.
     *
     * @param id The id of the release to look up.
     * @param externallyVisibleOnly Indicator used if only releases that target externally visible mapping types should be included.
     * @return The release or an errored mono.
     */
    Mono<ReleaseDMO> findById(
            UUID id,
            boolean externallyVisibleOnly
    );

    /**
     * Finds all releases which match the search criteria if they are supplied.
     * Supply null to anyone of them to ignore the search.
     *
     * @param nameRegex     The regex to filter the name on-
     * @param gameVersionId The id of the game version to filter releases on.
     * @param mappingTypeId The id of the mapping type to filter releases on.
     * @param isSnapshot    Indicate if snapshots should be included or not.
     * @param mappingId     The id of the mapping to filter releases on.
     * @param userId        The id of the creating user to filter releases on.
     * @param externallyVisibleOnly Indicates if only externally visible releases (releases for mapping types which are externally visible) should be returned.
     * @param pageable      The paging information for the query.
     * @return All releases which match the given criteria.
     */
    Mono<Page<ReleaseDMO>> findAllBy(
            String nameRegex,
            UUID gameVersionId,
            UUID mappingTypeId,
            Boolean isSnapshot,
            UUID mappingId,
            UUID userId,
            @NonNull boolean externallyVisibleOnly,
            @NonNull Pageable pageable);
}
