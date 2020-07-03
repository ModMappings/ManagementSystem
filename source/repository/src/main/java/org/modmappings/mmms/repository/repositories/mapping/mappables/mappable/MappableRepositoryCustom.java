package org.modmappings.mmms.repository.repositories.mapping.mappables.mappable;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

/**
 * Defines a custom repository for mappables.
 * Allows access to its data via custom accessor methods.
 *
 * Takes pagination and the type of a mappable into account.
 */
public interface MappableRepositoryCustom {
    /**
     * Finds all mappables which are of a given type.
     *
     * The order returned can not be guaranteed.
     * @param type The type of mappable to look up.
     * @param pageable The paging and sorting information.
     * @return The mappables of the given type.
     */
    Mono<Page<MappableDMO>> findAllBy(
            MappableTypeDMO type,
            Pageable pageable
    );
}
