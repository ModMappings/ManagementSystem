package org.modmappings.mmms.repository.repositories.core.releases.components;

import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the release components.
 *
 * This repository provides both custom access to release components as well as the standard crud access methods.
 */
@Repository
public interface ReleaseComponentRepository extends ModMappingRepository<ReleaseComponentDMO>, ReleaseComponentRepositoryCustom {
}
