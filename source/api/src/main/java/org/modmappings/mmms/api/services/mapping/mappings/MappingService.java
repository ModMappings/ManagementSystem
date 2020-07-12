package org.modmappings.mmms.api.services.mapping.mappings;

import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.model.mapping.mappings.DistributionDTO;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.DistributionDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.MappingDMO;
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

    private final UserLoggingService userLoggingService;

    public MappingService(final MappingRepository repository, final MappingTypeRepository mappingTypeRepository, final UserLoggingService userLoggingService) {
        this.repository = repository;
        this.mappingTypeRepository = mappingTypeRepository;
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
                .map(this::toDTO)
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
    public Mono<Page<MappingDTO>> getAllBy(final boolean latestOnly,
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
        return repository.findAllOrLatestFor(latestOnly, versionedMappableId, releaseId, toMappableTypeDMO(mappableType), inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, externallyVisibleOnly, pageable)
                .doFirst(() -> logger.debug("Looking up mappings: {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}.", latestOnly, versionedMappableId, releaseId, mappableType, inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, externallyVisibleOnly, pageable))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this::toDTO)
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
                        .map(dto -> this.toNewDMO(versionedMappableId, mappingTypeId, dto, userIdSupplier))
                        .flatMap(repository::save) //Creates the mapping object in the database
                        .map(this::toDTO) //Create the DTO from it.
                        .doOnNext(dto -> userLoggingService.warn(logger, userIdSupplier, String.format("Created new mapping: %s-%s with id: %s", dto.getInput(), dto.getOutput(), dto.getId()))));
    }

    /**
     * Creates a new mapping DTO from the DMO.
     * Looks up the relevant components of the mapping in the db.
     *
     * @param dmo The DMO to turn into a DTO.
     * @return The mapping DMO in a Mono.
     */
    private MappingDTO toDTO(final MappingDMO dmo) {
        return new MappingDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                dmo.getVersionedMappableId(),
                dmo.getMappingTypeId(),
                dmo.getParentClassId(),
                dmo.getParentMethodId(),
                dmo.getInput(),
                dmo.getOutput(),
                dmo.getDocumentation(),
                toDistributionDTO(dmo.getDistribution())
        );
    }

    /**
     * Converts a given distributions dmo value into a valid dto.
     *
     * @param dmo The dmo to convert.
     * @return Returns the dto value of the given dmo. If null is passed in then {@link DistributionDTO#UNKNOWN} is returned.
     */
    private DistributionDTO toDistributionDTO(final DistributionDMO dmo) {
        if (dmo == null)
            return DistributionDTO.UNKNOWN;

        return DistributionDTO.valueOf(dmo.name());
    }

    /**
     * Creates a new initial DMO from the given parameters.
     * Pulls the mapping data from the DTO passed in,
     * additionally pulls the creating user id from the supplier.
     *
     * @param versionedMappableId The id of the versioned mappable to create a mapping for.
     * @param mappingTypeId       The id of the mapping type to create a mapping for.
     * @param dto                 The DTO to pull mapping data from.
     * @param userIdSupplier      The supplier for the ID of the creating user.
     * @return The initial DMO with the data.
     */
    private MappingDMO toNewDMO(final UUID versionedMappableId,
                                final UUID mappingTypeId,
                                final MappingDTO dto,
                                final Supplier<UUID> userIdSupplier) {
        return new MappingDMO(
                userIdSupplier.get(),
                versionedMappableId,
                mappingTypeId,
                dto.getParentClassId(),
                dto.getParentMethodId(),
                dto.getInput(),
                dto.getOutput(),
                dto.getDocumentation(),
                toDistributionDMO(dto.getDistribution())
        );
    }

    /**
     * Converts a given mappable type dto value into a valid dmo.
     *
     * @param dto The dto to convert.
     * @return Returns the dmo value of the given dto. If null is passed in then null is returned.
     */
    private MappableTypeDMO toMappableTypeDMO(final MappableTypeDTO dto) {
        if (dto == null)
            return null;

        return MappableTypeDMO.valueOf(dto.name());
    }

    /**
     * Converts a given distributions dto value into a valid dmo.
     *
     * @param dto The dto to convert.
     * @return Returns the dmo value of the given dto. If null is passed in then {@link DistributionDMO#UNKNOWN} is returned.
     */
    private DistributionDMO toDistributionDMO(final DistributionDTO dto) {
        if (dto == null)
            return DistributionDMO.UNKNOWN;

        return DistributionDMO.valueOf(dto.name());
    }
}