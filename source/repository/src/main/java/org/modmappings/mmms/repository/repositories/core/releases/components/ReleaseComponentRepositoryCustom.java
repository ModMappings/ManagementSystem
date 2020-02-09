package org.modmappings.mmms.repository.repositories.core.releases.components;

import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.NoRepositoryBean;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * This interface describes a release component repository that gives access to its data
 * via custom accessor methods.
 *
 * It provides quick lookup of components via a release, a mapping, or the mappable type that they target.
 */
@NoRepositoryBean
public interface ReleaseComponentRepositoryCustom {
    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @param pageable The paging information for the query.
     * @return The release components which are part of the release with the given id.
     */
    Mono<Page<ReleaseComponentDMO>> findAllByReleaseId(UUID releaseId, Pageable pageable);

    /**
     * Finds all release component which target a mapping with the given id.
     *
     * @param mappingId The id of the mapping that the components are being looked up for.
     * @param pageable The paging information for the query.
     * @return The release components which target the mapping with the given id.
     */
    Mono<Page<ReleaseComponentDMO>> findAllByMappingId(UUID mappingId, Pageable pageable);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a class.
     *
     * @param releaseId The id of the release to get the class mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a class which are part of the given release.
     */
    Mono<Page<UUID>> findAllMappingIdsByReleaseIdForClass(UUID releaseId, Pageable pageable);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a method.
     *
     * @param releaseId The id of the release to get the method mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a method which are part of the given release.
     */
    Mono<Page<UUID>> findAllMappingIdsByReleaseIdForMethod(UUID releaseId, Pageable pageable);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a field.
     *
     * @param releaseId The id of the release to get the field mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a field which are part of the given release.
     */
    Mono<Page<UUID>> findAllMappingIdsByReleaseIdForField(UUID releaseId, Pageable pageable);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a parameter.
     *
     * @param releaseId The id of the release to get the parameter mappings for.
     * @param pageable The paging information for the query.
     * @return The mappings for a parameter which are part of the given release.
     */
    Mono<Page<UUID>> findAllMappingIdsByReleaseIdForParameter(UUID releaseId, Pageable pageable);
}
