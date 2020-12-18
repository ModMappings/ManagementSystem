package org.modmappings.mmms.repository.repositories.mapping.mappings.detailed;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.DetailedMappingDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * A repository that provides access to package related information on a mapping level.
 */
@Repository
public interface DetailedMappingRepository extends org.springframework.data.repository.Repository<DetailedMappingDMO, UUID> {

    /**
     * Finds all mappings of which the input or output matches the given regex.
     * Will only return the latest mapping for any given versioned mappable, if this is indicated via the latestOnly flag.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * This method, compared to {@link org.modmappings.mmms.repository.repositories.mapping.mappings.mapping.MappingRepositoryCustom#findAllOrLatestFor(Boolean, UUID, UUID, MappableTypeDMO, String, String, UUID, UUID, UUID, boolean, Pageable)}
     * also returns all metadata information.
     *
     * @param latestOnly            Indicator if only the latest mappings or all mappings should be returned.
     * @param versionedMappableId   The id of the versioned mappable to filter on.
     * @param releaseId             The id of the release to filter on.
     * @param mappableType          The type of the mappable to filter the mappings on.
     * @param inputRegex            The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex           The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId         The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId         The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param userId                The id of the user who created the mapping.
     * @param externallyVisibleOnly Indicates if only mappings for externally visible mapping types should be included.
     * @param pageable              The paging and sorting information.
     * @return All latest mappings, and their metadata, who' matches the given regexes and are part of the mapping type and game version if those are specified.
     */
    Mono<Page<DetailedMappingDMO>> findAllBy(
            final Boolean latestOnly,
            final UUID versionedMappableId,
            final UUID releaseId,
            final MappableTypeDMO mappableType,
            final String inputRegex,
            final String outputRegex,
            final UUID mappingTypeId,
            final UUID gameVersionId,
            final UUID userId,
            final boolean externallyVisibleOnly,
            final Pageable pageable
    );
}
