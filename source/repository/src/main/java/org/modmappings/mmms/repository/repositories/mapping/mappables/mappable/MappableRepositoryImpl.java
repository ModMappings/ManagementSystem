package org.modmappings.mmms.repository.repositories.mapping.mappables.mappable;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Represents a repository which can provide and store {@link MappableDMO} objects.
 */
@Primary
class MappableRepositoryImpl extends AbstractModMappingRepository<MappableDMO> implements MappableRepository {

    public MappableRepositoryImpl(RelationalEntityInformation<MappableDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all mappables which are of a given type.
     *
     * The order returned can not be guaranteed.
     * @param type The type of mappable to look up.
     * @param pageable The paging and sorting information.
     * @return The mappables of the given type.
     */
    @Override
    public Mono<Page<MappableDMO>> findAllBy(
            final MappableTypeDMO type,
            final Pageable pageable
    ) {
        return createPagedStarSingleWhereRequest("type", type, pageable);
    }
}
