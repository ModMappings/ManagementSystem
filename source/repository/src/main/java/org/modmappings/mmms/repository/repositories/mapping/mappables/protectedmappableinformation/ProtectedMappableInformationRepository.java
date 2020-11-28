package org.modmappings.mmms.repository.repositories.mapping.mappables.protectedmappableinformation;

import org.modmappings.mmms.repository.model.mapping.mappable.ProtectedMappableInformationDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the protection information of mappables.
 * <p>
 * This repository provides both custom access to the protection information as well as the standard crud access methods.
 */
@Repository
public interface ProtectedMappableInformationRepository extends ModMappingRepository<ProtectedMappableInformationDMO>, ProtectedMappableInformationRepositoryCustom {
}
