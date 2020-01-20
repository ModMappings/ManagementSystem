package org.modmappings.mmms.repository.repositories.core.release;

import java.util.UUID;

import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.repositories.IPageableR2DBCRepository;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link ReleaseComponentDMO} objects.
 */
@Repository
public interface IReleaseComponentRepository extends IPageableR2DBCRepository<ReleaseComponentDMO> {

    /**
     * Finds all release component which are part of a given release that has the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param releaseId The id of the release that the components are being looked up for.
     * @return The release components which are part of the release with the given id.
     */
    @Query("SELECT * FROM release_component rc where rc.releaseId = :releaseId")
    Flux<ReleaseComponentDMO> findAllForRelease(@Param("releaseId") UUID releaseId);

    /**
     * Finds all release component which target a mapping with the given id.
     *
     * The order of the release components is not guaranteed.
     *
     * @param mappingId The id of the mapping that the components are being looked up for.
     * @return The release components which target the mapping with the given id.
     */
    @Query("SELECT * FROM release_component rc where rc.mappingId = :mappingId")
    Flux<ReleaseComponentDMO> findAllForMapping(@Param("mappingId") UUID mappingId);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a package.
     * The mapping ids are returned in newest mapping to oldest mapping order.
     *
     * @param releaseId The id of the release to get the package mappings for.
     * @return The mappings for a package which are part of the given release.
     */
    @Query("Select rc.mapping_id from rc join mapping m on mp.id = rc.id join versioned_mappable vm on mp.versioned_mappable_id = vm.id join mappable m on vm.mappable_id = m.id where rc.release_id = :releaseId and m.type = 0 order by mp.created_on")
    Flux<UUID> findAllPackageMappingIdsForRelease(@Param("releaseId") UUID releaseId);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a class.
     * The mapping ids are returned in newest mapping to oldest mapping order.
     *
     * @param releaseId The id of the release to get the class mappings for.
     * @return The mappings for a class which are part of the given release.
     */
    @Query("Select rc.mapping_id from rc join mapping m on mp.id = rc.id join versioned_mappable vm on mp.versioned_mappable_id = vm.id join mappable m on vm.mappable_id = m.id where rc.release_id = :releaseId and m.type = 1 order by mp.created_on")
    Flux<UUID> findAllClassMappingIdsForRelease(@Param("releaseId") UUID releaseId);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a method.
     * The mapping ids are returned in newest mapping to oldest mapping order.
     *
     * @param releaseId The id of the release to get the method mappings for.
     * @return The mappings for a method which are part of the given release.
     */
    @Query("Select rc.mapping_id from rc join mapping m on mp.id = rc.id join versioned_mappable vm on mp.versioned_mappable_id = vm.id join mappable m on vm.mappable_id = m.id where rc.release_id = :releaseId and m.type = 2 order by mp.created_on")
    Flux<UUID> findAllMethodMappingIdsForRelease(@Param("releaseId") UUID releaseId);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a field.
     * The mapping ids are returned in newest mapping to oldest mapping order.
     *
     * @param releaseId The id of the release to get the field mappings for.
     * @return The mappings for a field which are part of the given release.
     */
    @Query("Select rc.mapping_id from rc join mapping m on mp.id = rc.id join versioned_mappable vm on mp.versioned_mappable_id = vm.id join mappable m on vm.mappable_id = m.id where rc.release_id = :releaseId and m.type = 3 order by mp.created_on")
    Flux<UUID> findAllFieldMappingIdsForRelease(@Param("releaseId") UUID releaseId);

    /**
     * Finds all mapping ids which are part of the given release and represent a mapping of a parameter.
     * The mapping ids are returned in newest mapping to oldest mapping order.
     *
     * @param releaseId The id of the release to get the parameter mappings for.
     * @return The mappings for a parameter which are part of the given release.
     */
    @Query("Select rc.mapping_id from rc join mapping m on mp.id = rc.id join versioned_mappable vm on mp.versioned_mappable_id = vm.id join mappable m on vm.mappable_id = m.id where rc.release_id = :releaseId and m.type = 4 order by mp.created_on")
    Flux<UUID> findAllParameterMappingIdsForRelease(@Param("releaseId") UUID releaseId);

    @Override
    @Query("Select * from release_component rc")
    Flux<ReleaseComponentDMO> findAll();
}
