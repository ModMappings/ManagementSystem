package org.modmappings.mmms.repository.repositories.mapping.mappings.mapping;

import org.modmappings.mmms.repository.model.mapping.mappings.MappingDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the mappings.
 *
 * This repository provides both custom access to the mappings as well as the standard crud access methods.
 */
@Repository
public interface MappingRepository extends ModMappingRepository<MappingDMO>, MappingRepositoryCustom {
}