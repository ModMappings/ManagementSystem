package org.modmapping.mmms.repository.repositories.mapping.mappable;

import java.util.UUID;

import org.modmapping.mmms.repository.model.mapping.mappable.VersionedMappableDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link VersionedMappableDMO} objects.
 */
public interface IVersionedMappableRepository extends R2dbcRepository<VersionedMappableDMO, UUID> {

    /**
     * Finds all versioned mappables for a given game version.
     *
     * The order of the returned versioned mappables is not guaranteed.
     * @param gameVersionId The id of the game version to look up all versioned mappables for.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the given game version.
     */
    @Query("Select * from versioned_mappable vm where vm.gameVersionId = $1")
    Flux<VersionedMappableDMO> findAllForGameVersion(UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all versioned mappables, which represent classes or other packages that are part of the package with the given id.
     *
     * @param packageVersionedMappableId The id of the package of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the package of which the versioned mappable has the given id.
     */
    @Query("Select * from versioned_mappable vm where vm.parentPackageId = $1")
    Flux<VersionedMappableDMO> findAllWhichArePartOfPackage(UUID packageVersionedMappableId, final Pageable pageable);

    /**
     * Finds all versioned mappables, which represent methods, fields and inner classes that are part of the class with the given id.
     *
     * @param classVersionedMappableId The id of the class of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the class of which the versioned mappable has the given id.
     */
    @Query("Select * from versioned_mappable vm where vm.partOfClassId = $1")
    Flux<VersionedMappableDMO> findAllWhichArePartOfClass(UUID classVersionedMappableId, final Pageable pageable);

    /**
     * Finds all versioned mappables, which represent parameters that are part of the method with the given id.
     *
     * @param methodVersionedMappableId The id of the method of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which are part of the method of which the versioned mappable has the given id.
     */
    @Query("Select * from versioned_mappable vm where vm.partOfMethodId = $1")
    Flux<VersionedMappableDMO> findAllWhichArePartOfMethod(UUID methodVersionedMappableId, final Pageable pageable);

    /**
     * Finds the versioned mappable who has the given mapping.
     *
     * @param mappingId The id of the mapping to look up.
     * @return The versioned mappable who has the given mapping.
     */
    @Query("SELECT vm.* FROM versioned_mappable vm JOIN mapping m ON vm.id = m.versionedMappableId WHERE m.id = $1 TAKE 1")
    Mono<VersionedMappableDMO> findAllForMapping(UUID mappingId);

    /**
     * Finds all super types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    @Query("SELECT vm.* FROM versioned_mappable vm JOIN inheritance_data mid ON vm.id = m.superTypeVersionedMappableId WHERE mid.subTypeVersionedMappableId = $1")
    Flux<VersionedMappableDMO> findAllSuperTypesOf(UUID classVersionedMappableId, final Pageable pageable);

    /**
     * Finds all sub types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable The pagination information for the query.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    @Query("SELECT *vm. FROM versioned_mappable vm JOIN inheritance_data mid ON vm.id = m.subTypeVersionedMappableId WHERE mid.superTypeVersionedMappableId = $1")
    Flux<VersionedMappableDMO> findAllSubTypesOf(UUID classVersionedMappableId, final Pageable pageable);
}
