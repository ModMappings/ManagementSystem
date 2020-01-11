package com.mcms.data.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.data.model.mapping.mappable.InheritanceDataDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link InheritanceDataDMO} objects.
 */
public interface IInheritanceDataRepository extends CrudRepository<InheritanceDataDMO, UUID> {

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the super type tole.
     *
     * @param superTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in super type role will be looked up.
     * @param pageable The pagination information for the query.
     * @return All inheritance data which indicates that the given mappable in a game version is a super type.
     */
    Flux<InheritanceDataDMO> findAllForSuperType(UUID superTypeVersionedMappableId, final Pageable pageable);

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the sub type tole.
     *
     * @param subTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in sub type role will be looked up.
     * @param pageable The pagination information for the query.
     * @return All inheritance data which indicates that the given mappable in a game version is a sub type.
     */
    Flux<InheritanceDataDMO> findAllForSubType(UUID subTypeVersionedMappableId, final Pageable pageable);
}
