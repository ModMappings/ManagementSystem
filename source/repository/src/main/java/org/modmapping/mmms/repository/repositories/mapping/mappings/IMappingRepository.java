package org.modmapping.mmms.repository.repositories.mapping.mappings;

import java.util.Optional;
import java.util.UUID;

import org.modmapping.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmapping.mmms.repository.model.mapping.mappings.MappingDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link MappingDMO} objects.
 */
public interface IMappingRepository extends R2dbcRepository<MappingDMO, UUID> {

    /**
     * Finds all mappings for a given versioned mappable id.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param versionedMappableId The id of the versioned mappable to get the mappings for.
     * @param pageable The pagination information for the query.
     * @return The mappings for the given versioned mappable.
     */
    @Query("SELECT * FROM mapping m WHERE m.versionedMappableId = $1 order by m.createdOn")
    Flux<MappingDMO> findAllForVersionedMappable(UUID versionedMappableId, final Pageable pageable);

    /**
     * Finds the latest mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the latest mapping for.
     * @return The latest mapping for the given versioned mappable.
     */
    @Query("SELECT * FROM mapping m WHERE m.versionedMappableId = $1 ORDER BY m.createdOn DESC TAKE 1")
    Mono<MappingDMO> findLatestForVersionedMappable(UUID versionedMappableId);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param pageable The pagination information for the query.
     * @return All mappings who' matche the given regexes and are part of the mapping type and game version if those are specified.
     */
    @Query("select m.* from mapping m join versioned_mappable vm on vm.id = m.versionedMappableId Where ($1 is null or m.input regexp $1) AND ($2 is null or m.output regexp $2) AND ($3 is null OR m.mappingTypeId = $3) AND ($4 is null OR vm.gameVersionId = $4) order by m.createdOn")
    Flux<MappingDMO> findAllForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(Optional<String> inputRegex, Optional<String> outputRegex, Optional<UUID> mappingTypeId, Optional<UUID> gameVersionId, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given versioned mappable.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param pageable The pagination information for the query.
     * @return All latest mappings who' matche the given regexes and are part of the mapping type and game version if those are specified.
     */
    @Query("select m.* from mapping m left join mapping m2 on (m.versionedMappableId = m2.versionedMappableId And m.mappingTypeId = m2.mappingTypeId And m.createdOn < m2.createdOn) join versioned_mappable vm on m.versionedMappableId = vm.id Where m2.id is null AND ($1 is null or m.input regexp $1) AND ($2 is null or m.output regexp $2) AND ($3 is null OR m.mappingTypeId = $3) AND ($4 is null OR vm.gameVersionId = $4) order by m.createdOn")
    Flux<MappingDMO> findLatestForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(Optional<String> inputRegex, Optional<String> outputRegex, Optional<UUID> mappingTypeId, Optional<UUID> gameVersionId, final Pageable pageable);

    /**
     * Finds all mappings which are part of a given release and who are for a given type.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param releaseId The id of the release where mappings are being looked up for.
     * @param type The type of the mappable where ids are being looked up for. Use a empty optional to get all.
     * @return All mappings which are part of a given release and are for a given mappable type.
     */
    @Query("select m.* from mapping m join release_component rc on rc.mappableId = m.id join versioned_mappable vm on m.versionedMappableId = vm.id join mappable mp on vm.mappableId = mp.id where rc.releaseId = $1 and ($2 is null or mp.type = $2)")
    Flux<MappingDMO> findAllInReleaseAndMappableType(UUID releaseId, Optional<MappableTypeDMO> type);
}
