package com.mcms.data.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.data.model.mapping.mappable.MappableDMO;
import com.mcms.data.model.mapping.mappable.MappableTypeDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link MappableDMO} objects.
 */
public interface IMappableRepository extends CrudRepository<MappableDMO, UUID> {

    /**
     * Finds all mappables which are of a given type.
     *
     * The order returned can not be guaranteed.
     * @param type The type of mappable to look up.
     * @param pageable The pagination information for the query.
     * @return The mappables of the given type.
     */
    Flux<MappableDMO> findAllForType(MappableTypeDMO type, final Pageable pageable);
}
