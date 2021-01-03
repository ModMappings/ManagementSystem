package org.modmappings.mmms.api.services.mapping.mappable;

import org.modmappings.mmms.api.converters.mapping.mappable.MappableTypeConverter;
import org.modmappings.mmms.api.converters.mapping.mappable.VersionedMappableConverter;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.model.mapping.mappable.VersionedMappableDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.api.util.CacheKeyBuilder;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.*;
import org.modmappings.mmms.repository.repositories.core.mappingtypes.MappingTypeRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.inheritancedata.InheritanceDataRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.mappable.MappableRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.protectedmappableinformation.ProtectedMappableInformationRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.versionedmappables.VersionedMappableRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.data.redis.core.ReactiveValueOperations;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.time.Duration;
import java.util.Map;
import java.util.UUID;
import java.util.function.Function;
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
public class VersionedMappableService {

    @Value("${caching.versioned-mappable.lifetimes.by-id:86400}")
    private int CACHE_LIFETIME_BY_ID;

    @Value("${caching.versioned-mappable.lifetimes.all:3600}")
    private int CACHE_LIFETIME_ALL;

    private final Logger logger = LoggerFactory.getLogger(VersionedMappableService.class);

    private final VersionedMappableRepository repository;
    private final InheritanceDataRepository inheritanceDataRepository;
    private final MappableRepository mappableRepository;
    private final ProtectedMappableInformationRepository protectedMappableInformationRepository;
    private final MappingTypeRepository mappingTypeRepository;

    private final VersionedMappableConverter versionedMappableConverter;
    private final MappableTypeConverter mappableTypeConverter;
    private final ReactiveValueOperations<Map<String, String>, VersionedMappableDTO> cacheOps;
    private final ReactiveValueOperations<Map<String, String>, Page<VersionedMappableDTO>> pageCacheOps;

    private final UserLoggingService userLoggingService;

    public VersionedMappableService(final VersionedMappableRepository repository, final InheritanceDataRepository inheritanceDataRepository, final MappableRepository mappableRepository, final ProtectedMappableInformationRepository protectedMappableInformationRepository, final MappingTypeRepository mappingTypeRepository, final VersionedMappableConverter versionedMappableConverter, final MappableTypeConverter mappableTypeConverter, final ReactiveValueOperations<Map<String, String>, VersionedMappableDTO> cacheOps, final ReactiveValueOperations<Map<String, String>, Page<VersionedMappableDTO>> pageCacheOps, final UserLoggingService userLoggingService) {
        this.repository = repository;
        this.inheritanceDataRepository = inheritanceDataRepository;
        this.mappableRepository = mappableRepository;
        this.protectedMappableInformationRepository = protectedMappableInformationRepository;
        this.mappingTypeRepository = mappingTypeRepository;
        this.versionedMappableConverter = versionedMappableConverter;
        this.mappableTypeConverter = mappableTypeConverter;
        this.cacheOps = cacheOps;
        this.pageCacheOps = pageCacheOps;
        this.userLoggingService = userLoggingService;
    }

    /**
     * Looks up a mappable with a given id.
     *
     * @param id The id to look the mappable up for.
     * @return A {@link Mono} containing the requested mappable or a errored {@link Mono} that indicates a failure.
     */
    public Mono<VersionedMappableDTO> getBy(final UUID id) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "getById")
                .put("id", id)
                .build();

        return cacheOps.get(
                cacheKey
        )
                .doFirst(() -> logger.debug("Looking up a mappable by id in cache: {}", id))
                .doOnNext(dto -> logger.debug("Found mappable: {} with id in cache: {}", dto.getType(), dto.getId()))
                .switchIfEmpty(repository.findById(id)
                        .doFirst(() -> logger.debug("Looking up a mappable by id in database: {}", id))
                        .flatMap(this::toDTO)
                        .doOnNext(dto -> logger.debug("Found mappable: {} with id in database: {}", dto.getType(), dto.getId()))
                        .zipWhen(dto -> cacheOps.set(cacheKey, dto, Duration.ofSeconds(CACHE_LIFETIME_BY_ID)), (dto, a) -> dto)
                        .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Mappable"))));
    }

    /**
     * Look up all versioned mappables who match the given search criteria.
     *
     * @param gameVersionId      The id of the game version. Null to ignore.
     * @param mappableTypeDTO    The type of the mappable to look up. Null to ignore.
     * @param classId            The id of the class to find versioned mappables in. Null to ignore.
     * @param methodId           The id of the method to find versioned mappables in. Null to ignore.
     * @param mappingId          The id of the mapping to find the versioned mappables for. Null to ignore. If parameter is passed, either a single result is returned or none. Since each mapping can only target a single versioned mappable.
     * @param mappingTypeId      The id of the mapping type to find the versioned mappables for. Null to ignore. Use full in combination with a input and output regex.
     * @param mappingInputRegex  A regex that is mapped against the input of the mapping. Null to ignore
     * @param mappingOutputRegex A regex that is mapped against the output of the mapping. Null to ignore
     * @param superTypeTargetId  The id of the class to find the super types for. Null to ignore.
     * @param subTypeTargetId    The id of the class to find the sub types for. Null to ignore.
     * @param pageable           The pagination and sorting information for the request.
     * @return The page that returns the requested versioned mappables.
     */
    public Mono<Page<VersionedMappableDTO>> getAll(
            final UUID gameVersionId,
            final MappableTypeDTO mappableTypeDTO,
            final UUID classId,
            final UUID methodId,
            final UUID mappingId,
            final UUID mappingTypeId,
            final String mappingInputRegex,
            final String mappingOutputRegex,
            final UUID superTypeTargetId,
            final UUID subTypeTargetId,
            final Pageable pageable
    ) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "getAll")
                .put("gameVersionId", gameVersionId)
                .put("mappableTypeDTO", mappableTypeDTO)
                .put("classId", classId)
                .put("methodId", methodId)
                .put("mappingId", mappingId)
                .put("mappingTypeId", mappingTypeId)
                .put("mappingInputRegex", mappingInputRegex)
                .put("mappingOutputRegex", mappingOutputRegex)
                .put("superTypeTargetId", superTypeTargetId)
                .put("subTypeTargetId", subTypeTargetId)
                .put("pageable", pageable)
                .build();

        return pageCacheOps.get(
                cacheKey
        ).doFirst(() -> logger.debug("Looking up versioned mappables in cache: {}, {}, {}, {}, {}, {}, {}, {}, {}, {}.", gameVersionId, mappableTypeDTO, classId, methodId, mappingId, mappingTypeId, mappingInputRegex, mappingOutputRegex, superTypeTargetId, subTypeTargetId))
                .doOnNext(page -> logger.debug("Found versioned mappables in cache: {}", page))
                .switchIfEmpty(repository.findAllFor(
                        gameVersionId, this.mappableTypeConverter.toDMO(mappableTypeDTO), classId, methodId, mappingId, mappingTypeId, mappingInputRegex, mappingOutputRegex, superTypeTargetId, subTypeTargetId, true, pageable
                )
                        .doFirst(() -> logger.debug("Looking up versioned mappables in database: {}, {}, {}, {}, {}, {}, {}, {}, {}, {}.", gameVersionId, mappableTypeDTO, classId, methodId, mappingId, mappingTypeId, mappingInputRegex, mappingOutputRegex, superTypeTargetId, subTypeTargetId))
                        .flatMap(page -> Flux.fromIterable(page)
                                .flatMap(this::toDTO)
                                .collectList()
                                .map(mappables -> (Page<VersionedMappableDTO>) new PageImpl<>(mappables, page.getPageable(), page.getTotalElements())))
                        .doOnNext(page -> logger.debug("Found versioned mappables in database: {}", page))
                        .zipWhen(page -> pageCacheOps.set(cacheKey, page, Duration.ofSeconds(CACHE_LIFETIME_ALL)), (page, a) -> page)
                        .switchIfEmpty(Mono.error(new NoEntriesFoundException("Mappable"))));
    }

    /**
     * Updates the versioned mappable DMO that has a given id, with the data passed in the DTO.
     *
     * @param id                        The id of the dmo to update.
     * @param versionedMappableToUpdate The data to update the dmo with.
     * @param userIdSupplier            The user id supplier that can be used to determine which user is updating the dmo.
     * @return The updated versioned mappable in a Mono.
     */
    public Mono<VersionedMappableDTO> update(
            final UUID id,
            final VersionedMappableDTO versionedMappableToUpdate,
            final Supplier<UUID> userIdSupplier) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "getById")
                .put("id", id)
                .build();

        return repository.findById(id)
                .flatMap(dmo -> protectedMappableInformationRepository.findAllByVersionedMappable(id, Pageable.unpaged())
                        .doFirst(() -> userLoggingService.info(logger, userIdSupplier, String.format("Retrieving protection information for: %s.", id)))
                        .flatMapIterable(Function.identity())
                        .collectList()
                        .flatMap(currentlyProtectedTypes -> Flux.fromIterable(currentlyProtectedTypes)
                                .filter(currentlyProtectedType -> !versionedMappableToUpdate.getLockedIn().contains(currentlyProtectedType.getMappingTypeId()))
                                .flatMap(currentlyProtectedTypeToDelete -> protectedMappableInformationRepository.deleteById(currentlyProtectedTypeToDelete.getId())
                                        .doFirst(() -> userLoggingService.info(logger, userIdSupplier, String.format("Deleting protection information for: %s with mapping type: %s", currentlyProtectedTypeToDelete.getVersionedMappableId(), currentlyProtectedTypeToDelete.getMappingTypeId()))))
                                .then(Flux.fromIterable(versionedMappableToUpdate.getLockedIn())
                                        .filterWhen(newLockInId -> mappingTypeRepository.findById(newLockInId).filter(MappingTypeDMO::isVisible).filter(MappingTypeDMO::isEditable).hasElement())
                                        .filter(validNewLockInId -> currentlyProtectedTypes.stream().noneMatch(pmi -> pmi.getId() == validNewLockInId))
                                        .map(validNewLockInId -> new ProtectedMappableInformationDMO(id, validNewLockInId))
                                        .flatMap(newProtectionInformation -> protectedMappableInformationRepository.save(newProtectionInformation)
                                                .doFirst(() -> userLoggingService.info(logger, userIdSupplier, String.format("Creating new protection information for: %s with mapping type: %s", newProtectionInformation.getVersionedMappableId(), newProtectionInformation.getMappingTypeId()))))
                                        .collectList()
                                )
                        )
                        .then()
                )
                .zipWhen(v -> cacheOps.delete(cacheKey))
                //TODO: Somehow clear the page cache as well.
                .flatMap(v -> this.getBy(id));
    }

    private Mono<VersionedMappableDTO> toDTO(final VersionedMappableDMO dmo) {
        return this.versionedMappableConverter.toDTO(
                dmo,
                mappableRepository.findById(dmo.getMappableId()),
                inheritanceDataRepository.findAllForSuperType(dmo.getId(), Pageable.unpaged())
                        .flatMapIterable(Function.identity())
                        .map(InheritanceDataDMO::getSubTypeVersionedMappableId),
                inheritanceDataRepository.findAllForSubType(dmo.getId(), Pageable.unpaged())
                        .flatMapIterable(Function.identity())
                        .map(InheritanceDataDMO::getSuperTypeVersionedMappableId),
                protectedMappableInformationRepository.findAllByVersionedMappable(dmo.getId(), Pageable.unpaged())
                        .flatMapIterable(Function.identity())
                        .map(ProtectedMappableInformationDMO::getMappingTypeId)
                        .filterWhen(mappingTypeId -> mappingTypeRepository.findById(mappingTypeId)
                                .filter(MappingTypeDMO::isVisible)
                                .hasElement()
                        )
        );
    }
}
