package org.modmappings.mmms.repository.repositories.core.gameversions;

import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the game versions which are part of the mod mappings infrastructure.
 *
 * This repository provides both custom access to game versions as well as the standard crud access methods.
 */
@Repository
public interface GameVersionRepository extends ModMappingRepository<GameVersionDMO>, GameVersionRepositoryCustom {
}
