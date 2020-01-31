package org.modmappings.mmms.repository.repositories.mapping.mappings.votingrecord;

import org.modmappings.mmms.repository.model.mapping.mappings.voting.VotingRecordDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the voting records that exist.
 *
 * This repository provides both custom access to the voting records as well as the standard crud access methods.
 */
@Repository
public interface VotingRecordRepository extends ModMappingRepository<VotingRecordDMO>, VotingRecordRepositoryCustom {
}
