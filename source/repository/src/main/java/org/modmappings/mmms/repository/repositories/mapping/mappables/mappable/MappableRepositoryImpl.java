package org.modmappings.mmms.repository.repositories.mapping.mappables.mappable;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;

/**
 * Represents a repository which can provide and store {@link MappableDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class MappableRepositoryImpl extends AbstractModMappingRepository<MappableDMO> implements MappableRepository {

    public MappableRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, MappableDMO.class);
    }

    /**
     * Finds all mappables which are of a given type.
     * <p>
     * The order returned can not be guaranteed.
     *
     * @param type     The type of mappable to look up.
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
