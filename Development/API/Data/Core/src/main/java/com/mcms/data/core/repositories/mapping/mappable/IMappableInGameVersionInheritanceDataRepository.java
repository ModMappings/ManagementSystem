package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.MappableInGameVersionInheritanceDataDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;

public interface IMappableInGameVersionInheritanceDataRepository extends IRepository<MappableInGameVersionInheritanceDataDMO> {

    Flux<MappableInGameVersionInheritanceDataDMO> findAllForSuperType(UUID superTypeMappableInGameVersionId);

    Flux<MappableInGameVersionInheritanceDataDMO> findAllForSubType(UUID subTypeMappableInGameVersionId);
}
