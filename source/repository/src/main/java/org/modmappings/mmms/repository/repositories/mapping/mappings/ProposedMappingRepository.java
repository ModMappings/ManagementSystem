package org.modmappings.mmms.repository.repositories.mapping.mappings;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.repository.model.mapping.mappings.ProposedMappingDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * Represents a repository that can store and provide {@link ProposedMappingDMO} objects.
 */
@Repository
public class ProposedMappingRepository extends ModMappingR2DBCRepository<ProposedMappingDMO> {

    public ProposedMappingRepository(RelationalEntityInformation<ProposedMappingDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all proposed mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the proposed mappings for.
     * @param pageable The paging and sorting information.
     * @return The proposed mappings for the given versioned mappable.
     */
    public Mono<Page<ProposedMappingDMO>> findAllForVersionedMappableAndStateAndMerged(UUID versionedMappableId, Boolean state, Boolean merged, Pageable pageable)
    {
        return createPagedStarRequest(
                selectSpecWithJoin -> {
                    selectSpecWithJoin
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
                            });
                },
                pageable
        );
    }
}
