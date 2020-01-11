package com.mcms.data.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.data.model.mapping.mappable.InheritanceDataDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;

/**
 * This is the R2DBC based implementation of the {@link IInheritanceDataRepository}.
 *
 * This interface, although not directly implementing the methods from the {@link IInheritanceDataRepository},
 * provides all the functionality via the {@link Query}-Annotation that is put on its methods with the relevant SQL Query implemented.
 */
public interface InheritanceDataRepositoryImpl extends IInheritanceDataRepository {

    @Query("Select * from mappable_inheritance_data mid where mid.superTypeVersionedMappableId = $1")
    @Override
    Flux<InheritanceDataDMO> findAllForSuperType(
                    UUID superTypeVersionedMappableId, final Pageable pageable);

    @Query("Select * from mappable_inheritance_data mid where mid.subTypeVersionedMappableId = $1")
    @Override
    Flux<InheritanceDataDMO> findAllForSubType(
                    UUID subTypeVersionedMappableId, final Pageable pageable);
}
