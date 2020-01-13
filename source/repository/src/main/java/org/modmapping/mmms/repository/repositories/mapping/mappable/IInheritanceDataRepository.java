package org.modmapping.mmms.repository.repositories.mapping.mappable;

import java.util.UUID;

import org.modmapping.mmms.repository.model.mapping.mappable.InheritanceDataDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link InheritanceDataDMO} objects.
 */
public interface IInheritanceDataRepository extends R2dbcRepository<InheritanceDataDMO, UUID> {

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the super type tole.
     *
     * @param superTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in super type role will be looked up.
     * @param pageable The pagination information for the query.
     * @return All inheritance data which indicates that the given mappable in a game version is a super type.
     */
    @Query("Select * from inheritance_data mid where mid.superTypeVersionedMappableId = $1")
    Flux<InheritanceDataDMO> findAllForSuperType(UUID superTypeVersionedMappableId, final Pageable pageable);

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the sub type tole.
     *
     * @param subTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in sub type role will be looked up.
     * @param pageable The pagination information for the query.
     * @return All inheritance data which indicates that the given mappable in a game version is a sub type.
     */
    @Query("Select * from inheritance_data mid where mid.subTypeVersionedMappableId = $1")
    Flux<InheritanceDataDMO> findAllForSubType(UUID subTypeVersionedMappableId, final Pageable pageable);
}
