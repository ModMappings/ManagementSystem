package org.modmappings.mmms.api.services.mapping.mappings;

import org.modmappings.mmms.api.converters.mapping.mappable.MappableTypeConverter;
import org.modmappings.mmms.api.converters.mapping.mappings.DetailedMappingConverter;
import org.modmappings.mmms.api.model.mapping.mappable.DetailedMappingDTO;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.repository.repositories.mapping.mappings.detailed.DetailedMappingRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Business layer service which handles the interactions of the API with the DataLayer.
 * <p>
 * This services validates data as well as converts between the API models as well as the data models.
 * <p>
 * This services however does not validate if a given user is authorized to execute a given action.
 * It only validates the interaction from a data perspective.
 * <p>
 * The caller is to make sure that any interaction with this service is authorized, for example by checking
 * against a role that a user needs to have.
 */
@Component
public class DetailedMappingService {

    private final Logger logger = LoggerFactory.getLogger(MappingService.class);
    private final DetailedMappingRepository repository;

    private final DetailedMappingConverter instancedMappingConverter;
    private final MappableTypeConverter mappableTypeConverter;

    public DetailedMappingService(final DetailedMappingRepository repository, final DetailedMappingConverter instancedMappingConverter, final MappableTypeConverter mappableTypeConverter) {
        this.repository = repository;
        this.instancedMappingConverter = instancedMappingConverter;
        this.mappableTypeConverter = mappableTypeConverter;
    }

    /**
     * Looks up multiple mappings, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param latestOnly            Indicator if only the latest mappings or all mappings should be returned.
     * @param versionedMappableId   The id of the versioned mappable to filter on.
     * @param releaseId             The id of the release to filter on.
     * @param mappableType          The type of the mappable to filter the mappings on.
     * @param inputRegex            The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex           The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId         The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId         The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param externallyVisibleOnly Indicates if only mappings for externally visible mapping types should be included.
     * @param pageable              The paging and sorting information.
     * @return A {@link Mono} with the mappings, or an errored {@link Mono} that indicates a failure.
     */
    public Mono<Page<DetailedMappingDTO>> getAllBy(final boolean latestOnly,
                                                   final UUID versionedMappableId,
                                                   final UUID releaseId,
                                                   final MappableTypeDTO mappableType,
                                                   final String inputRegex,
                                                   final String outputRegex,
                                                   final UUID mappingTypeId,
                                                   final UUID gameVersionId,
                                                   final UUID userId,
                                                   final boolean externallyVisibleOnly,
                                                   final Pageable pageable) {
        return repository.findAllBy(latestOnly, versionedMappableId, releaseId, this.mappableTypeConverter.toDMO(mappableType), inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, externallyVisibleOnly, pageable)
                .doFirst(() -> logger.debug("Looking up mappings: {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}.", latestOnly, versionedMappableId, releaseId, mappableType, inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, externallyVisibleOnly, pageable))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this.instancedMappingConverter::toDTO)
                        .collectList()
                        .map(mappings -> (Page<DetailedMappingDTO>) new PageImpl<>(mappings, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found mappings: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("DetailedMapping")));
    }
}
