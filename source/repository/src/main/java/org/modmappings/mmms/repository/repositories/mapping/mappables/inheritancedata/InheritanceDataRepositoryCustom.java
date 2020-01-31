package org.modmappings.mmms.repository.repositories.mapping.mappables.inheritancedata;

import org.modmappings.mmms.repository.model.mapping.mappable.InheritanceDataDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository that gives access to inheritance data of mappables which represent classes
 * and interfaces.
 *
 * This repository allows, in particular, the lookup of super and sub types of a given class.
 */
public interface InheritanceDataRepositoryCustom {
    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the super type tole.
     *
     * @param superTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in super type role will be looked up.
     * @param pageable The pageable information in the request.
     * @return All inheritance data which indicates that the given mappable in a game version is a super type.
     */
    Mono<Page<InheritanceDataDMO>> findAllForSuperType(
            UUID superTypeVersionedMappableId,
            Pageable pageable);

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the sub type tole.
     *
     * @param subTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in sub type role will be looked up.
     * @param pageable The pageable information in the request.
     * @return All inheritance data which indicates that the given mappable in a game version is a sub type.
     */
    Mono<Page<InheritanceDataDMO>> findAllForSubType(
            UUID subTypeVersionedMappableId,
            Pageable pageable
    );
}
