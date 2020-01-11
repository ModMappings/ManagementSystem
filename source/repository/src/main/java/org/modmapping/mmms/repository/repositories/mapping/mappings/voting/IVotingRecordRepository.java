package org.modmapping.mmms.repository.repositories.mapping.mappings.voting;

import java.util.Optional;
import java.util.UUID;

import org.modmapping.mmms.repository.model.mapping.mappings.voting.VotingRecordDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository that can store and provide {@link VotingRecordDMO} objects.
 */
public interface IVotingRecordRepository extends CrudRepository<VotingRecordDMO, UUID> {

    /**
     * Finds all voting records for a given proposed mapping.
     *
     * @param proposedMappingId The id of the proposed mapping to look up the voting records for.
     * @param indication An optional possibly containing the indication that voting records are being looked up for. If the optional contains true then all for-votes are returned
     *                   if the optional contains false then all against-votes are returned and if the optional is empty then all votes are returned.
     * @param isRescinded An optional possibly containing the rescinded state that voting records are being looked up for. If the optional contains true then all the rescinded votes are returned
     *                    if the optional contains false then all the active none rescinded votes are returned and if the optional is empty then all votes are returned.
     * @param pageable The paging information for the request.
     * @return All the voting records that match the filter criteria.
     */
    @Query("SELECT * FROM voting_record vr WHERE vr.proposedMappingId = $1 AND ($2 IS NULL OR vr.isForVote = $2) AND ($3 IS NULL OR vr.hasBeenRescinded = $3)")
    Flux<VotingRecordDMO> findAllForProposedMappingAndIndicationAndRescinded(UUID proposedMappingId, Optional<Boolean> indication, Optional<Boolean> isRescinded, Pageable pageable);
}
