package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.MappableInGameVersionDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link MappableInGameVersionDMO} objects.
 */
public interface IMappableInGameVersionRepository extends CrudRepository<MappableInGameVersionDMO, UUID> {

    /**
     * Finds all mappables in a game version for a given game version.
     *
     * The order of the returned mappables in a game version is not guaranteed.
     * @param gameVersionId The id of the game version to look up all mappables in a game version for.
     * @param pageable The pagination information for the query.
     * @return The mappables in a game version which are part of the given game version.
     */
    Flux<MappableInGameVersionDMO> findAllForGameVersion(UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all mappables in a game version, which represent classes or other packages that are part of the package with the given id.
     *
     * @param packageMappableInGameVersionId The id of the package of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The mappables in a game version which are part of the package of which the mappable in a game version has the given id.
     */
    Flux<MappableInGameVersionDMO> findAllWhichArePartOfPackage(UUID packageMappableInGameVersionId, final Pageable pageable);

    /**
     * Finds all mappables in a game version, which represent methods, fields and inner classes that are part of the class with the given id.
     *
     * @param classMappableInGameVersionId The id of the class of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The mappables in a game version which are part of the class of which the mappable in a game version has the given id.
     */
    Flux<MappableInGameVersionDMO> findAllWhichArePartOfClass(UUID classMappableInGameVersionId, final Pageable pageable);

    /**
     * Finds all mappables in a game version, which represent parameters that are part of the method with the given id.
     *
     * @param methodMappableInGameVersionId The id of the method of which the contents are being looked up.
     * @param pageable The pagination information for the query.
     * @return The mappables in a game version which are part of the method of which the mappable in a game version has the given id.
     */
    Flux<MappableInGameVersionDMO> findAllWhichArePartOfMethod(UUID methodMappableInGameVersionId, final Pageable pageable);

    /**
     * Finds the mappable in a game version who has the given mapping.
     *
     * @param mappingId The id of the mapping to look up.
     * @param pageable The pagination information for the query.
     * @return The mappable in a game version who has the given mapping.
     */
    Mono<MappableInGameVersionDMO> findAllForMapping(UUID mappingId, final Pageable pageable);

    /**
     * Finds all super types (in the form of classes) of the class with the given id and returns their mappables in a game version.
     *
     * @param classMappableInGameVersionId The id of the class of which the super types are being looked up.
     * @param pageable The pagination information for the query.
     * @return The mappables in a game version which represent the super types of the class of which the id of its mappable in a game version was provided.
     */
    Flux<MappableInGameVersionDMO> findAllSuperTypesOf(UUID classMappableInGameVersionId, final Pageable pageable);

    /**
     * Finds all sub types (in the form of classes) of the class with the given id and returns their mappables in a game version.
     *
     * @param classMappableInGameVersionId The id of the class of which the super types are being looked up.
     * @param pageable The pagination information for the query.
     * @return The mappables in a game version which represent the super types of the class of which the id of its mappable in a game version was provided.
     */
    Flux<MappableInGameVersionDMO> findAllSubTypesOf(UUID classMappableInGameVersionId, final Pageable pageable);
}
