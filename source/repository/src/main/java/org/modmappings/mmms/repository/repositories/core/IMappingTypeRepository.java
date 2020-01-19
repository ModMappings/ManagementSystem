package org.modmappings.mmms.repository.repositories.core;

import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.repositories.IPageableR2DBCRepository;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.repository.query.Param;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which provides and stores {@link MappingTypeDMO} objects.
 */
public interface IMappingTypeRepository extends IPageableR2DBCRepository<MappingTypeDMO> {

    /**
     * Finds all mapping types which match the given name regex.
     * and which are editable if that parameter is supplied.
     *
     * The mapping types are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup mapping types for.
     * @return The mapping types of which the name match the regex.
     */
    @Query("SELECT mt.* from mapping_type mt WHERE mt.name ~ :nameRegex and (:editable is null or mt.editable = :editable)")
    Flux<MappingTypeDMO> findAllFor(@Param("nameRegex") String nameRegex, @Param("editable") Boolean editable);

    /**
     * Finds all mappings types.
     *
     * @return The mappings types in the database.
     */
    @Override
    @Query("Select mt.* from mapping_type mt")
    Flux<MappingTypeDMO> findAll();
}
