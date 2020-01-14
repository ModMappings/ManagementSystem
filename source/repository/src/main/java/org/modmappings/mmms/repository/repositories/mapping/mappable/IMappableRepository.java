package org.modmappings.mmms.repository.repositories.mapping.mappable;

import java.util.UUID;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.IPageableR2DBCRepository;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link MappableDMO} objects.
 */
public interface IMappableRepository extends IPageableR2DBCRepository<MappableDMO> {

    /**
     * Finds all mappables which are of a given type.
     *
     * The order returned can not be guaranteed.
     * @param type The type of mappable to look up.
     * @param pageable The pagination information for the query.
     * @return The mappables of the given type.
     */
    @Query("SELECT * FROM mappable m WHERE m.type = $1")
    Flux<MappableDMO> findAllForType(MappableTypeDMO type, final Pageable pageable);

    @Override
    @Query("Select * from mappable m")
    Flux<MappableDMO> findAll(Pageable pageable);
}
