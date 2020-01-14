package org.modmappings.mmms.repository.repositories.core;

import java.util.UUID;

import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.repositories.IPageableR2DBCRepository;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which provides and stores {@link MappingTypeDMO} objects.
 */
public interface IMappingTypeRepository extends IPageableR2DBCRepository<MappingTypeDMO> {

    /**
     * Finds all mapping types which match the given name regex.
     *
     * The mapping types are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup mapping types for.
     * @return The mapping types of which the name match the regex.
     */
    @Query("SELECT * from mapping_type mt WHERE mt.name regexp $1")
    Flux<MappingTypeDMO> findAllForNameRegex(String nameRegex);

    @Override
    @Query("Select * from mapping_type mt")
    Flux<MappingTypeDMO> findAll();
}
