package org.modmappings.mmms.repository.repositories.mapping.mappings.mapping;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.MappingDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository for mappings.
 * Allows access to its data via custom accessor methods.
 *
 * These repositories give, in particular, access to the input and output of a mapping via
 * regular expressions.
 */
public interface MappingRepositoryCustom {
    /**
     * Finds all mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the mappings for.
     * @param pageable The paging and sorting information.
     * @return The mappings for the given versioned mappable.
     */
    Mono<Page<MappingDMO>> findAllForVersionedMappable(UUID versionedMappableId, Pageable pageable);

    /**
     * Finds the latest mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the latest mapping for.
     * @return The latest mapping for the given versioned mappable.
     */
    Mono<MappingDMO> findLatestForVersionedMappable(UUID versionedMappableId);

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param pageable The paging and sorting information.
     * @return All mappings who' matches the given regexes and are part of the mapping type and game version if those are specified.
     */
    Mono<Page<MappingDMO>> findAllForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(String inputRegex, String outputRegex, UUID mappingTypeId, UUID gameVersionId, Pageable pageable);

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given versioned mappable.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param pageable The paging and sorting information.
     * @return All latest mappings who' matches the given regexes and are part of the mapping type and game version if those are specified.
     */
    Mono<Page<MappingDMO>> findLatestForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(String inputRegex, String outputRegex, UUID mappingTypeId, UUID gameVersionId, Pageable pageable);

    /**
     * Finds all mappings which are part of a given release and who are for a given type.
     *
     * @param releaseId The id of the release where mappings are being looked up for.
     * @param type The type of the mappable where ids are being looked up for. Use a empty optional to get all.
     * @param pageable The paging and sorting information.
     * @return All mappings which are part of a given release and are for a given mappable type.
     */
    Mono<Page<MappingDMO>> findAllInReleaseAndMappableType(UUID releaseId, MappableTypeDMO type, Pageable pageable);
}
