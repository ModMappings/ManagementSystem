package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.MappableDMO;
import com.mcms.api.datamodel.mapping.mappable.MappableInGameVersionDMO;
import com.mcms.api.datamodel.mapping.mappable.MappableTypeDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link MappableInGameVersionDMO} objects.
 */
public interface IMappableInGameVersionRepository extends IRepository<MappableInGameVersionDMO> {

    /**
     * Finds all mappables in a game version for a given game version.
     *
     * The order of the returned mappables in a game version is not guaranteed.
     * @param gameVersionId The id of the game version to look up all mappables in a game version for.
     * @return The mappables in a game version which are part of the given game version.
     */
    Flux<MappableDMO> findAllForGameVersion(UUID gameVersionId);

    /**
     * Finds all mappables in a game version, which represent classes or other packages that are part of the package with the given id.
     *
     * @param packageMappableInGameVersionId The id of the package of which the contents are being looked up.
     * @return The mappables in a game version which are part of the package of which the mappable in a game version has the given id.
     */
    Flux<MappableDMO> findAllWhichArePartOfPackage(UUID packageMappableInGameVersionId);

    /**
     * Finds all mappables in a game version, which represent methods, fields and inner classes that are part of the class with the given id.
     *
     * @param classMappableInGameVersionId The id of the class of which the contents are being looked up.
     * @return The mappables in a game version which are part of the class of which the mappable in a game version has the given id.
     */
    Flux<MappableDMO> findAllWhichArePartOfClass(UUID classMappableInGameVersionId);

    /**
     * Finds all mappables in a game version, which represent parameters that are part of the method with the given id.
     *
     * @param methodMappableInGameVersionId The id of the method of which the contents are being looked up.
     * @return The mappables in a game version which are part of the method of which the mappable in a game version has the given id.
     */
    Flux<MappableDMO> findAllWhichArePartOfMethod(UUID methodMappableInGameVersionId);

    /**
     * Finds the mappable in a game version who has the given mapping.
     *
     * @param mappingId The id of the mapping to look up.
     * @return The mappable in a game version who has the given mapping.
     */
    Mono<MappableDMO> findAllForMapping(UUID mappingId);

    /**
     * Finds all super types (in the form of classes) of the class with the given id and returns their mappables in a game version.
     *
     * @param classMappableInGameVersionId The id of the class of which the super types are being looked up.
     * @return The mappables in a game version which represent the super types of the class of which the id of its mappable in a game version was provided.
     */
    Flux<MappableDMO> findAllSuperTypesOf(UUID classMappableInGameVersionId);

    /**
     * Finds all sub types (in the form of classes) of the class with the given id and returns their mappables in a game version.
     *
     * @param classMappableInGameVersionId The id of the class of which the super types are being looked up.
     * @return The mappables in a game version which represent the super types of the class of which the id of its mappable in a game version was provided.
     */
    Flux<MappableDMO> findAllSubTypesOf(UUID classMappableInGameVersionId);
}
