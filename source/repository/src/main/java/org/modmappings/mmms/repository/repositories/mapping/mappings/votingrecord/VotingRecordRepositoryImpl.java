package org.modmappings.mmms.repository.repositories.mapping.mappings.votingrecord;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.repository.model.mapping.mappings.voting.VotingRecordDMO;
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
 * Represents a repository that can store and provide {@link VotingRecordDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class VotingRecordRepositoryImpl extends AbstractModMappingRepository<VotingRecordDMO> implements VotingRecordRepository {

    public VotingRecordRepositoryImpl(DatabaseClient databaseClient, ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, VotingRecordDMO.class);
    }

    /**
     * Finds all voting records for a given proposed mapping.
     *
     * @param proposedMappingId The id of the proposed mapping to look up the voting records for.
     * @param indication An optional possibly containing the indication that voting records are being looked up for. If the optional contains true then all for-votes are returned
     *                   if the optional contains false then all against-votes are returned and if the optional is empty then all votes are returned.
     * @param isRescinded An optional possibly containing the rescinded state that voting records are being looked up for. If the optional contains true then all the rescinded votes are returned
     *                    if the optional contains false then all the active none rescinded votes are returned and if the optional is empty then all votes are returned.
     * @param pageable The paging and sorting information.
     * @return All the voting records that match the filter criteria.
     */
    @Override
    public Mono<Page<VotingRecordDMO>> findAllForProposedMappingAndIndicationAndRescinded(
            final UUID proposedMappingId,
            final Boolean indication,
            final Boolean isRescinded,
            final Pageable pageable) {
        return createPagedStarRequest(
                selectSpecWithJoin ->
                    selectSpecWithJoin
                        .where(() -> {
                            ColumnBasedCriteria criteria = where(reference("proposed_mapping_id")).is(parameter(proposedMappingId));
                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    indication,
                                    "",
                                    "is_for_vote"
                            );

                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    isRescinded,
                                    "",
                                    "has_been_rescinded"
                            );
                            return criteria;
                        })
                ,
                pageable
        );
    }
}
