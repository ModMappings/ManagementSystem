package org.modmappings.mmms.repository.repositories.mapping.mappable;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * Represents a repository which can provide and store {@link MappableDMO} objects.
 */
public class MappableRepository extends ModMappingR2DBCRepository<MappableDMO> {

    /**
     * Finds all mappables which are of a given type.
     *
     * The order returned can not be guaranteed.
     * @param type The type of mappable to look up.
     * @return The mappables of the given type.
     */
    @Query("SELECT * FROM mappable m WHERE m.type = $1")
    Mono<Page<MappableDMO>> findAllByType(
            final MappableTypeDMO type,
            final Pageable pageable
    ) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin.withCriteria(where(reference("type")).is(parameter(type))),
                pageable
        );
    }

    @Override
    @Query("Select * from mappable m")
    Flux<MappableDMO> findAll();
}
