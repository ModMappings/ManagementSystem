package org.modmappings.mmms.repository.repositories.mapping.mappings.votingrecord;

import org.modmappings.mmms.repository.model.mapping.mappings.voting.VotingRecordDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository that gives access to voting records
 * via its custom accessor methods.
 *
 * Allows, in particular, the look up of voting records by their properties.
 */
public interface VotingRecordRepositoryCustom {
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
    Mono<Page<VotingRecordDMO>> findAllForProposedMappingAndIndicationAndRescinded(
            UUID proposedMappingId,
            Boolean indication,
            Boolean isRescinded,
            Pageable pageable);
}
