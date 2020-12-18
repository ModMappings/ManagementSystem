package org.modmappings.mmms.api.services.mapping.mappings;

import org.modmappings.mmms.api.converters.mapping.mappable.MappableTypeConverter;
import org.modmappings.mmms.api.converters.mapping.mappings.MappingConverter;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.repositories.core.mappingtypes.MappingTypeRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappings.mapping.MappingRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;
import java.util.function.Supplier;

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
public class MappingService {

    private final Logger logger = LoggerFactory.getLogger(MappingService.class);
    private final MappingRepository repository;
    private final MappingTypeRepository mappingTypeRepository;

    private final MappingConverter mappingConverter;
    private final MappableTypeConverter mappableTypeConverter;

    private final UserLoggingService userLoggingService;

    public MappingService(final MappingRepository repository, final MappingTypeRepository mappingTypeRepository, final MappingConverter mappingConverter, final MappableTypeConverter mappableTypeConverter, final UserLoggingService userLoggingService) {
        this.repository = repository;
        this.mappingTypeRepository = mappingTypeRepository;
        this.mappingConverter = mappingConverter;
        this.mappableTypeConverter = mappableTypeConverter;
        this.userLoggingService = userLoggingService;
    }


    /**
     * Looks up a mapping with a given id.
     *
     * @param id                    The id to look the mapping up for.
     * @param externallyVisibleOnly Indicator if only externally visible mappings should be taken into account.
     * @return A {@link Mono} containing the requested mapping or a errored {@link Mono} that indicates a failure.
     */
    public Mono<MappingDTO> getBy(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        return repository.findById(id, externallyVisibleOnly)
                .doFirst(() -> logger.debug("Looking up a mapping by id: {}", id))
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Only return a mapping when it is supposed to be visible.
                .map(this.mappingConverter::toDTO)
                .doOnNext(dto -> logger.debug("Found mapping: {}-{} with id: {}", dto.getInput(), dto.getOutput(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Mapping")));
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
    public Mono<Page<MappingDTO>> getAllBy(final Boolean latestOnly,
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
        return repository.findAllOrLatestFor(latestOnly, versionedMappableId, releaseId, this.mappableTypeConverter.toDMO(mappableType), inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, externallyVisibleOnly, pageable)
                .doFirst(() -> logger.debug("Looking up mappings: {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}.", latestOnly, versionedMappableId, releaseId, mappableType, inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, externallyVisibleOnly, pageable))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this.mappingConverter::toDTO)
                        .collectList()
                        .map(mappings -> (Page<MappingDTO>) new PageImpl<>(mappings, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found mappings: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("Mapping")));
    }

    /**
     * Creates a new mapping from a DTO and saves it in the repository.
     *
     * @param versionedMappableId The id of the versioned mappable to create a mapping for.
     * @param mappingTypeId       The id of the mapping type to create a mapping for.
     * @param newMappingDto       The DTO to pull mapping data from.
     * @param userIdSupplier      The supplier for the ID of the creating user.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<MappingDTO> create(
            final UUID versionedMappableId,
            final UUID mappingTypeId,
            final MappingDTO newMappingDto,
            final Supplier<UUID> userIdSupplier
    ) {
        return mappingTypeRepository.findById(mappingTypeId)
                .filter(MappingTypeDMO::isVisible)
                .flatMap(mdto -> Mono.just(newMappingDto)
                        .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Creating new mapping: %s-%s", newMappingDto.getInput(), newMappingDto.getOutput())))
                        .map(dto -> this.mappingConverter.toNewDMO(versionedMappableId, mappingTypeId, dto, userIdSupplier))
                        .flatMap(repository::save) //Creates the mapping object in the database
                        .map(this.mappingConverter::toDTO) //Create the DTO from it.
                        .doOnNext(dto -> userLoggingService.warn(logger, userIdSupplier, String.format("Created new mapping: %s-%s with id: %s", dto.getInput(), dto.getOutput(), dto.getId()))));
    }
}