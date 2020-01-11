package org.modmapping.mmms.repository.repositories.mapping.mappings;

import java.util.UUID;

import org.modmapping.mmms.repository.model.mapping.mappings.ProposedMappingDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository that can store and provide {@link ProposedMappingDMO} objects.
 */
public interface IProposedMappingRepository extends CrudRepository<ProposedMappingDMO, UUID> {

    /**
     * Finds all proposed mappings for a given versioned mappable id.
     *
     * The proposed mappings will be returned from newest to oldest.
     *
     * @param versionedMappableId The id of the versioned mappable to get the proposed mappings for.
     * @param pageable The pagination information for the query.
     * @return The proposed mappings for the given versioned mappable.
     */
    Flux<ProposedMappingDMO> findAllForVersionedMappable(UUID versionedMappableId, final Pageable pageable);

    /**
     * Finds all proposed mappings for a given mapping type id.
     *
     * The proposed mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the proposed mappings for.
     * @param pageable The pagination information for the query.
     * @return The proposed mappings for the given mapping type.
     */
    Flux<ProposedMappingDMO> findAllForMappingType(UUID mappingTypeId, final Pageable pageable);

    /**
     * Finds all proposed mappings for a given mapping type id and game version.
     *
     * The proposed mappings will be returned from newest to oldest.
     *
     * @param mappingTypeId The id of the mapping type to get the proposed mappings for.
     * @param gameVersionId The id of the game version that the proposed mappings need to be for.
     * @param pageable The pagination information for the query.
     * @return The proposed mappings for the given mapping type and game version.
     */
    Flux<ProposedMappingDMO> findAllForMappingTypeAndGameVersion(UUID mappingTypeId, UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all proposed mappings of which the input matches the given regex.
     *
     * The proposed mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the proposed mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All proposed mappings who's input matches the given regex.
     */
    Flux<ProposedMappingDMO> findAllForInputRegexAndOutputRegex(String inputRegex, String outputRegex, final Pageable pageable);

    /**
     * Finds all proposed mappings of which the input matches the given regex.
     * The proposed mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * The proposed mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the proposed mappings is matched to be included in the result.
     * @param gameVersionId The id of the game version that the proposed mappings mappable in a game version has to target.
     * @param pageable The pagination information for the query.
     * @return All proposed mappings who's input matches the given regex.
     */
    Flux<ProposedMappingDMO> findAllForInputRegexAndOutputRegexAndGameVersion(String inputRegex, String outputRegex, UUID gameVersionId, final Pageable pageable);

    /**
     * Finds all proposed mappings of which the input matches the given regex.
     * The proposed mapping also has to be for a mapping type with the given id.
     *
     * The proposed mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the proposed mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All proposed mappings who's input matches the given regex.
     */
    Flux<ProposedMappingDMO> findAllForInputRegexAndOutputRegexAndMappingType(String inputRegex, String outputRegex, UUID mappingTypeId, final Pageable pageable);

    /**
     * Finds all proposed mappings of which the input matches the given regex.
     * The proposed mapping also has to be for a mapping type with the given id.
     * The proposed mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * The proposed mappings will be returned in newest to oldest order.
     *
     * @param inputRegex The regex against which the input of the proposed mappings is matched to be included in the result.
     * @param pageable The pagination information for the query.
     * @return All proposed mappings who's input matches the given regex.
     */
    Flux<ProposedMappingDMO> findAllForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(String inputRegex, String outputRegex, UUID mappingTypeId, UUID gameVersionId, final Pageable pageable);

}
