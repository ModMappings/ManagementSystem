package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.VersionedMappableDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link VersionedMappableDMO} objects.
 */
public interface IVersionedMappableRepository extends CrudRepository<VersionedMappableDMO, UUID> {

    /**
     * Finds all versioned mappables for a given game version.
     *
     * The order of the returned versioned mappables is not guaranteed.
     * @param gameVersionId The id of the game version to look up all versioned mappables for.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the given game version.
     */
    Flux<VersionedMappableDMO> findAllForGameVersion(UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all versioned mappables, which represent classes or other packages that are part of the package with the given id.
     *
     * @param packageVersionedMappableId The id of the package of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the package of which the versioned mappable has the given id.
     */
    Flux<VersionedMappableDMO> findAllWhichArePartOfPackage(UUID packageVersionedMappableId, final Pageable pageable);

    /**
     * Finds all versioned mappables, which represent methods, fields and inner classes that are part of the class with the given id.
     *
     * @param classVersionedMappableId The id of the class of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the class of which the versioned mappable has the given id.
     */
    Flux<VersionedMappableDMO> findAllWhichArePartOfClass(UUID classVersionedMappableId, final Pageable pageable);

    /**
     * Finds all versioned mappables, which represent parameters that are part of the method with the given id.
     *
     * @param methodVersionedMappableId The id of the method of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the method of which the versioned mappable has the given id.
     */
    Flux<VersionedMappableDMO> findAllWhichArePartOfMethod(UUID methodVersionedMappableId, final Pageable pageable);

    /**
     * Finds the versioned mappable who has the given mapping.
     *
     * @param mappingId The id of the mapping to look up.
     * @return The versioned mappable who has the given mapping.
     */
    Mono<VersionedMappableDMO> findAllForMapping(UUID mappingId);

    /**
     * Finds all super types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    Flux<VersionedMappableDMO> findAllSuperTypesOf(UUID classVersionedMappableId, final Pageable pageable);

    /**
     * Finds all sub types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    Flux<VersionedMappableDMO> findAllSubTypesOf(UUID classVersionedMappableId, final Pageable pageable);
}
