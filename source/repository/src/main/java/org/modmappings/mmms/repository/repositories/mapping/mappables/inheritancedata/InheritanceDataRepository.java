package org.modmappings.mmms.repository.repositories.mapping.mappables.inheritancedata;

import org.modmappings.mmms.repository.model.mapping.mappable.InheritanceDataDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the inheritance data of classes.
 *
 * This repository provides both custom access to inheritance data as well as the standard crud access methods.
 */
@Repository
public interface InheritanceDataRepository extends ModMappingRepository<InheritanceDataDMO>, InheritanceDataRepositoryCustom {
}
