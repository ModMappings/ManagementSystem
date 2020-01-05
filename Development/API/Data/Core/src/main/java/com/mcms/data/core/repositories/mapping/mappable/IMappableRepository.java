package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.MappableDMO;
import com.mcms.api.datamodel.mapping.mappable.MappableTypeDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link MappableDMO} objects.
 */
public interface IMappableRepository extends IRepository<MappableDMO> {

    /**
     * Finds all mappables which are of a given type.
     *
     * The order returned can not be guaranteed.
     * @param type The type of mappable to look up.
     * @return The mappables of the given type.
     */
    Flux<MappableDMO> findAllForType(MappableTypeDMO type);
}
