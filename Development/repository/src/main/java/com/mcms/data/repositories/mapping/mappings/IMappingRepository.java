package com.mcms.data.repositories.mapping.mappings;

import java.util.UUID;

import com.mcms.data.model.mapping.mappings.MappingDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link MappingDMO} objects.
 */
public interface IMappingRepository extends CrudRepository<MappingDMO, UUID> {

    /**
     * Finds all mappings for a given versioned mappable id.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param versionedMappableId The id of the versioned mappable to get the mappings for.
     * @param pageable The pagination information for the query.
     * @return The mappings for the given versioned mappable.
     */
    Flux<MappingDMO> findAllForVersionedMappable(UUID versionedMappableId, final Pageable pageable);

    /**
     * Finds the latest mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the latest mapping for.
     * @param pageable The pagination information for the query.
     * @return The latest mapping for the given versioned mappable.
     */
    Mono<MappingDMO> findLatestForVersionedMappable(UUID versionedMappableId, final Pageable pageable);

    /**
     * Finds all mappings for a given mapping type id.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the mappings for.
     * @param pageable The pagination information for the query.
     * @return The mappings for the given mapping type.
     */
    Flux<MappingDMO> findAllForMappingType(UUID mappingTypeId, final Pageable pageable);

    /**
     * Finds all mappings for a given mapping type id and game version.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the mappings for.
     * @param gameVersionId The id of the game version that the mappings need to be for.
     * @param pageable The pagination information for the query.
     * @return The mappings for the given mapping type and game version.
     */
    Flux<MappingDMO> findAllForMappingTypeAndGameVersion(UUID mappingTypeId, UUID gameVersionId, final Pageable pageable);

    /**
     * Finds the latest mappings for a given mapping type id.
     * This will, in particular, find all the mappings for every mappable that exist, and grab their latest.
     * Which is in contrast too {@link #findAllForMappingType(UUID, Pageable)} which just finds all regardless of the fact
     * that there then might be multiple mappings for a given mappable.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the latest mapping for.
     * @return The latest mappings for the given mapping type.
     */
    Flux<MappingDMO> findAllLatestForMappingType(UUID mappingTypeId, final Pageable pageable);

    /**
     * Finds the latest mappings for a given mapping type id which are part of a given game version.
     *
     * This will, in particular, find all the mappings for every mappable that exist, inside of the given game version, and grab their latest.
     * Which is in contrast too {@link #findAllLatestForMappingType(UUID, Pageable)} which just finds all regardless of the fact
     * that there then might be multiple mappings for a given mappable.
     *
     * The mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the latest mapping for.
     * @param gameVersionId The id of the game version to get the latest mappings for.
     * @param pageable The pagination information for the query.
     * @return The latest mappings for the given mapping type.
     */
    Flux<MappingDMO> findAllLatestForMappingTypeAndGameVersion(UUID mappingTypeId, UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegexAndOutputRegex(String inputRegex, String outputRegex, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given versioned mappable.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegexAndOutputRegex(String inputRegex, String outputRegex, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param gameVersionId The id of the game version that the mappings mappable in a game version has to target.
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegexAndOutputRegexAndGameVersion(String inputRegex, String outputRegex, UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given versioned mappable.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param gameVersionId The id of the game version that the mappings mappable in a game version has to target.
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegexAndOutputRegexAndGameVersion(String inputRegex, String outputRegex, UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegexAndOutputRegexAndMappingType(String inputRegex, String outputRegex, UUID mappingTypeId, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given versioned mappable.
     * The mapping also has to be for a mapping type with the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegexAndOutputRegexAndMappingType(String inputRegex, String outputRegex, UUID mappingTypeId, final Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * The mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findAllForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(String inputRegex, String outputRegex, UUID mappingTypeId, UUID gameVersionId, final Pageable pageable);

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
     * @param pageable The pagination information for the query.
     * @return All mappings who's input matches the given regex.
     */
    Flux<MappingDMO> findLatestForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(String inputRegex, String outputRegex, UUID mappingTypeId, UUID gameVersionId, final Pageable pageable);
}
