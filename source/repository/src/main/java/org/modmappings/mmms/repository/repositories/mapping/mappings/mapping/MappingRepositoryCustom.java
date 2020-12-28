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
 * <p>
 * These repositories give, in particular, access to the input and output of a mapping via
 * like expressions.
 */
public interface MappingRepositoryCustom {
    /**
     * Finds all mappings of which the input matches the given expression.
     * Will only return the latest mapping for any given versioned mappable, if this is indicated via the latestOnly flag.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * @param latestOnly            Indicator if only the latest mappings or all mappings should be returned.
     * @param versionedMappableId   The id of the versioned mappable to filter on.
     * @param releaseId             The id of the release to filter on.
     * @param mappableType          The type of the mappable to filter the mappings on.
     * @param inputExpression            The expression against which the input of the mappings is matched to be included in the result.
     * @param outputExpression           The expression against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId         The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId         The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param userId                The id of the user who created the mapping.
     * @param parentClassId         The id of the class of which the targeted mappings versioned mappable resides in.
     * @param parentMethodId        The id of the method of which the targeted mappings versioned mappable resides in.
     * @param externallyVisibleOnly Indicates if only mappings for externally visible mapping types should be included.
     * @param pageable              The paging and sorting information.
     * @return All latest mappings who' matches the given expressiones and are part of the mapping type and game version if those are specified.
     */
    Mono<Page<MappingDMO>> findAllOrLatestFor(final Boolean latestOnly,
                                              final UUID versionedMappableId,
                                              final UUID releaseId,
                                              final MappableTypeDMO mappableType,
                                              final String inputExpression,
                                              final String outputExpression,
                                              final UUID mappingTypeId,
                                              final UUID gameVersionId,
                                              final UUID userId,
                                              final UUID parentClassId,
                                              final UUID parentMethodId,
                                              final boolean externallyVisibleOnly,
                                              final Pageable pageable);

    /**
     * Finds a mapping with the given id, respecting the fact that only mappings for externally visible mapping types should be considered.
     *
     * @param id                    The id of the mapping.
     * @param externallyVisibleOnly Indicator if only externally visible mappings should be considered.
     * @return The mapping in a mono.
     */
    Mono<MappingDMO> findBy(UUID id, boolean externallyVisibleOnly);
}
