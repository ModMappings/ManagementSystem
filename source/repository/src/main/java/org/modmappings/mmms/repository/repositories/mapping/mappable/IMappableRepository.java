package org.modmappings.mmms.repository.repositories.mapping.mappable;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link MappableDMO} objects.
 */
public interface IMappableRepository extends ModMappingR2DBCRepository<MappableDMO> {

    /**
     * Finds all mappables which are of a given type.
     *
     * The order returned can not be guaranteed.
     * @param type The type of mappable to look up.
     * @return The mappables of the given type.
     */
    @Query("SELECT * FROM mappable m WHERE m.type = $1")
    Flux<MappableDMO> findAllForType(MappableTypeDMO type);

    @Override
    @Query("Select * from mappable m")
    Flux<MappableDMO> findAll();
}
