package org.modmappings.mmms.repository.repositories.mapping.mappables.versionedmappables;

import org.modmappings.mmms.repository.model.mapping.mappable.VersionedMappableDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the versioned mappables that can be remapped for a given game version.
 * <p>
 * This repository provides both custom access to the versioned mappables as well as the standard crud access methods.
 */
@Repository
public interface VersionedMappableRepository extends ModMappingRepository<VersionedMappableDMO>, VersionedMappableRepositoryCustom {
}
