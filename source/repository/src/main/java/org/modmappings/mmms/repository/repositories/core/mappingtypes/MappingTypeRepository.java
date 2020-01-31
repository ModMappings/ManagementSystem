package org.modmappings.mmms.repository.repositories.core.mappingtypes;

import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;

/**
 * Represents a repository that gives access to the mapping types which are part of the mod mappings infrastructure.
 *
 * This repository provides both custom access to mapping types as well as the standard crud access methods.
 */
public interface MappingTypeRepository extends ModMappingRepository<MappingTypeDMO>, MappingTypeRepositoryCustom {
}
