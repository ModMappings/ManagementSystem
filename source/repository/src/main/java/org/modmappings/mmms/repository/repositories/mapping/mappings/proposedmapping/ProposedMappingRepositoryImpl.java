package org.modmappings.mmms.repository.repositories.mapping.mappings.proposedmapping;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.repository.model.mapping.mappings.ProposedMappingDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * Represents a repository that can store and provide {@link ProposedMappingDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class ProposedMappingRepositoryImpl extends AbstractModMappingRepository<ProposedMappingDMO> implements ProposedMappingRepository {

    public ProposedMappingRepositoryImpl(DatabaseClient databaseClient, ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, ProposedMappingDMO.class);
    }

    /**
     * Finds all proposed mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the proposed mappings for.
     * @param pageable The paging and sorting information.
     * @return The proposed mappings for the given versioned mappable.
     */
    @Override
    public Mono<Page<ProposedMappingDMO>> findAllForVersionedMappableAndStateAndMerged(UUID versionedMappableId, Boolean state, Boolean merged, Pageable pageable)
    {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .where(() -> {
                            ColumnBasedCriteria criteria = where(reference("versioned_mappable_id")).is(parameter(versionedMappableId));
                            if (state != null) {
                                if (!state) {
                                    criteria = criteria.and(reference("closed_by")).isNull().and(reference("closed_on")).isNull();
                                }
                                else
                                {
                                    criteria = criteria.and(reference("closed_by")).isNotNull().and(reference("closed_on")).isNotNull();
                                }
                            }

                            if (merged != null) {
                                if (!merged) {
                                    criteria = criteria.and(reference("mapping_id")).isNull();
                                }
                                else
                                {
                                    criteria = criteria.and(reference("mapping_id")).isNotNull();
                                }
                            }

                            return criteria;
                        }),
                pageable
        );
    }
}
