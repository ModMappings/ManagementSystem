package org.modmappings.mmms.repository.repositories.mapping.mappings.proposedmapping;

import org.modmappings.mmms.repository.model.mapping.mappings.ProposedMappingDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the proposed mappings that exist.
 *
 * This repository provides both custom access to the proposed mappings as well as the standard crud access methods.
 */
@Repository
public interface ProposedMappingRepository extends ModMappingRepository<ProposedMappingDMO>, ProposedMappingRepositoryCustom {
}
