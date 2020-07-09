package org.modmappings.mmms.repository.repositories.mapping.mappables.versionedmappables;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.VersionedMappableDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository that gives access to versioned mappables via
 * its custom accessor methods.
 * <p>
 * This repository in particular allows the lookup of versioned mappables via
 * the code structure and the game version.
 */
public interface VersionedMappableRepositoryCustom {


    /**
     * Finds all versioned mappables for a given mappable.
     * <p>
     * The order of the returned versioned mappables is not guaranteed.
     *
     * @param mappableId The id of the mappable to look up all versioned mappables for.
     * @param pageable   The paging and sorting information.
     * @return The versioned mappables which are part of the given mappable.
     */
    Mono<Page<VersionedMappableDMO>> findAllForMappable(
            UUID mappableId,
            Pageable pageable
    );

    /**
     * Finds all versioned mappables for a given game version.
     * <p>
     * The order of the returned versioned mappables is not guaranteed.
     *
     * @param gameVersionId The id of the game version to look up all versioned mappables for.
     * @param pageable      The paging and sorting information.
     * @return The versioned mappables which are part of the given game version.
     */
    Mono<Page<VersionedMappableDMO>> findAllForGameVersion(
            UUID gameVersionId,
            Pageable pageable
    );

    /**
     * Finds all versioned mappables, which represent methods, fields and inner classes that are part of the class with the given id.
     *
     * @param classVersionedMappableId The id of the class of which the contents are being looked up.
     * @param pageable                 The paging and sorting information.
     * @return The versioned mappables which are part of the class of which the versioned mappable has the given id.
     */
    Mono<Page<VersionedMappableDMO>> findAllWhichArePartOfClass(
            UUID classVersionedMappableId,
            Pageable pageable
    );

    /**
     * Finds all versioned mappables, which represent parameters that are part of the method with the given id.
     *
     * @param methodVersionedMappableId The id of the method of which the contents are being looked up.
     * @param pageable                  The paging and sorting information.
     * @return The versioned mappables which are part of the method of which the versioned mappable has the given id.
     */
    Mono<Page<VersionedMappableDMO>> findAllWhichArePartOfMethod(
            UUID methodVersionedMappableId,
            Pageable pageable
    );

    /**
     * Finds the versioned mappable who has the given mapping.
     *
     * @param mappingId The id of the mapping to look up.
     * @return The versioned mappable who has the given mapping.
     */
    Mono<VersionedMappableDMO> findAllForMapping(
            UUID mappingId
    );

    /**
     * Finds all super types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable                 The paging and sorting information.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    Mono<Page<VersionedMappableDMO>> findAllSuperTypesOf(
            UUID classVersionedMappableId,
            Pageable pageable);

    /**
     * Finds all sub types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable                 The paging and sorting information.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    Mono<Page<VersionedMappableDMO>> findAllSubTypesOf(
            UUID classVersionedMappableId,
            Pageable pageable);

    /**
     * Finds all versioned mappables who match the given search criteria.
     *
     * @param gameVersionId         The id of the game version. Null to ignore.
     * @param mappableTypeDMO       The type of the mappable to look up. Null to ignore.
     * @param classId               The id of the class to find versioned mappables in. Null to ignore.
     * @param methodId              The id of the method to find versioned mappables in. Null to ignore.
     * @param mappingId             The id of the mapping to find the versioned mappables for. Null to ignore. If parameter is passed, either a single result is returned or none. Since each mapping can only target a single versioned mappable.
     * @param mappingTypeId         The id of the mapping type to find the versioned mappables for. Null to ignore. Use full in combination with a input and output regex.
     * @param mappingInputRegex     A regex that is mapped against the input of the mapping. Null to ignore
     * @param mappingOutputRegex    A regex that is mapped against the output of the mapping. Null to ignore
     * @param superTypeTargetId     The id of the class to find the super types for. Null to ignore.
     * @param subTypeTargetId       The id of the class to find the sub types for. Null to ignore.
     * @param externallyVisibleOnly Indicate if externally visible classes only
     * @param pageable              The pagination and sorting information for the request.
     * @return The page that returns the requested versioned mappables.
     */
    Mono<Page<VersionedMappableDMO>> findAllFor(
            final UUID gameVersionId,
            final MappableTypeDMO mappableTypeDMO,
            final UUID classId,
            final UUID methodId,
            final UUID mappingId,
            final UUID mappingTypeId,
            final String mappingInputRegex,
            final String mappingOutputRegex,
            final UUID superTypeTargetId,
            final UUID subTypeTargetId,
            final boolean externallyVisibleOnly,
            final Pageable pageable
    );
}
