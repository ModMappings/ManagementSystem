package org.modmappings.mmms.repository.repositories.mapping.mappable;

import java.util.UUID;

import org.modmappings.mmms.repository.model.mapping.mappable.InheritanceDataDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link InheritanceDataDMO} objects.
 */
public interface IInheritanceDataRepository extends ModMappingR2DBCRepository<InheritanceDataDMO> {

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the super type tole.
     *
     * @param superTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in super type role will be looked up.
     * @return All inheritance data which indicates that the given mappable in a game version is a super type.
     */
    @Query("Select * from inheritance_data mid where mid.superTypeVersionedMappableId = $1")
    Flux<InheritanceDataDMO> findAllForSuperType(UUID superTypeVersionedMappableId);

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the sub type tole.
     *
     * @param subTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in sub type role will be looked up.
     * @return All inheritance data which indicates that the given mappable in a game version is a sub type.
     */
    @Query("Select * from inheritance_data mid where mid.subTypeVersionedMappableId = $1")
    Flux<InheritanceDataDMO> findAllForSubType(UUID subTypeVersionedMappableId);

    @Override
    @Query("Select * from inheritance_data mid")
    Flux<InheritanceDataDMO> findAll();
}
