package org.modmappings.mmms.repository.repositories.mapping.mappables.mappable;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the mappables that can be remapped.
 *
 * This repository provides both custom access to the mappables as well as the standard crud access methods.
 */
@Repository
public interface MappableRepository extends ModMappingRepository<MappableDMO>, MappableRepositoryCustom {
}
