package org.modmappings.mmms.repository.repositories.core.releases.release;

import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the releases which are part of the mod mappings infrastructure.
 * Each release targets a given mapping type and provides access to the mappings that are part of it.
 * <p>
 * This repository provides both custom access to releases as well as the standard crud access methods.
 */
@Repository
public interface ReleaseRepository extends ModMappingRepository<ReleaseDMO>, ReleaseRepositoryCustom {
}
