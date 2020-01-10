package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.MappableInGameVersionInheritanceDataDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;

/**
 * This is the R2DBC based implementation of the {@link IMappableInGameVersionInheritanceDataRepository}.
 *
 * This interface, although not directly implementing the methods from the {@link IMappableInGameVersionInheritanceDataRepository},
 * provides all the functionality via the {@link Query}-Annotation that is put on its methods with the relevant SQL Query implemented.
 */
public interface ISpringR2DBCMappableInGameVersionInheritanceDataRepository extends IMappableInGameVersionInheritanceDataRepository {

    @Query("Select * from mappable_inheritance_data mid where mid.superTypeMappableInGameVersionId = $1")
    @Override
    Flux<MappableInGameVersionInheritanceDataDMO> findAllForSuperType(
                    UUID superTypeMappableInGameVersionId, final Pageable pageable);

    @Query("Select * from mappable_inheritance_data mid where mid.subTypeMappableInGameVersionId = $1")
    @Override
    Flux<MappableInGameVersionInheritanceDataDMO> findAllForSubType(
                    UUID subTypeMappableInGameVersionId, final Pageable pageable);
}
