package org.modmappings.mmms.api.services.mapping.mappings;

import org.modmappings.mmms.api.converters.mapping.mappable.MappableTypeConverter;
import org.modmappings.mmms.api.converters.mapping.mappings.DetailedMappingConverter;
import org.modmappings.mmms.api.model.mapping.mappable.DetailedMappingDTO;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.util.CacheKeyBuilder;
import org.modmappings.mmms.repository.repositories.mapping.mappings.detailed.DetailedMappingRepository;
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

    @Value("${caching.detailed-mapping.lifetimes.by-id:86400}")
    private int CACHE_LIFETIME_BY_ID;
    @Value("${caching.detailed-mapping.lifetimes.all:3600}")
    private int CACHE_LIFETIME_ALL;

    private final Logger logger = LoggerFactory.getLogger(MappingService.class);
    private final DetailedMappingRepository repository;

    private final DetailedMappingConverter instancedMappingConverter;
    private final MappableTypeConverter mappableTypeConverter;

    private final ReactiveValueOperations<Map<String, String>, DetailedMappingDTO> cacheOps;
    private final ReactiveValueOperations<Map<String, String>, Page<DetailedMappingDTO>> pageCacheOps;

    public DetailedMappingService(final DetailedMappingRepository repository, final DetailedMappingConverter instancedMappingConverter, final MappableTypeConverter mappableTypeConverter, final ReactiveValueOperations<Map<String, String>, DetailedMappingDTO> cacheOps, final ReactiveValueOperations<Map<String, String>, Page<DetailedMappingDTO>> pageCacheOps) {
        this.repository = repository;
        this.instancedMappingConverter = instancedMappingConverter;
        this.mappableTypeConverter = mappableTypeConverter;
        this.cacheOps = cacheOps;
        this.pageCacheOps = pageCacheOps;
    }

    /**
     * Looks up a detailed mapping with a given id.
     *
     * @param id                    The id to look the mapping up for.
     * @param externallyVisibleOnly Indicator if only externally visible mappings should be taken into account.
     * @return A {@link Mono} containing the requested mapping or a errored {@link Mono} that indicates a failure.
     */
    public Mono<DetailedMappingDTO> getBy(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "getById")
                .put("id", id)
                .put("externallyVisibleOnly", externallyVisibleOnly)
                .build();

        return cacheOps.get(
                cacheKey
        ).doFirst(() -> logger.debug("Looking up a detailed mapping by id in cache: {}", id))
                .doOnNext(dto -> logger.debug("Found detailed mapping: {}-{} with id in cache: {}", dto.getMappingDTO().getInput(), dto.getMappingDTO().getOutput(), dto.getMappingDTO().getId()))
                .switchIfEmpty(repository.findById(id, externallyVisibleOnly)
                        .doFirst(() -> logger.debug("Looking up a detailed mapping by id in database: {}", id))
                        .flatMap(this.instancedMappingConverter::toDTO)
                        .doOnNext(dto -> logger.debug("Found detailed mapping in database: {}-{} with id: {}", dto.getMappingDTO().getInput(), dto.getMappingDTO().getOutput(), dto.getMappingDTO().getId()))
                        .zipWhen(dto -> cacheOps.set(cacheKey, dto, Duration.ofSeconds(CACHE_LIFETIME_BY_ID)), (dto, a) -> dto)
                        .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Mapping"))));
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
     * @param gameVersionId         The id of the game version that the mapping needs to be for. Use an empty optional for any game version.* @param parentClassId         The id of the class of which the targeted mappings versioned mappable resides in.
     * @param parentClassId         The id of the class of which the targeted mappings versioned mappable resides in.
     * @param parentMethodId        The id of the method of which the targeted mappings versioned mappable resides in.
     * @param externallyVisibleOnly Indicates if only mappings for externally visible mapping types should be included.
     * @param pageable              The paging and sorting information.
     * @return A {@link Mono} with the mappings, or an errored {@link Mono} that indicates a failure.
     */
    public Mono<Page<DetailedMappingDTO>> getAllBy(final Boolean latestOnly,
                                                   final UUID versionedMappableId,
                                                   final UUID releaseId,
                                                   final MappableTypeDTO mappableType,
                                                   final String inputRegex,
                                                   final String outputRegex,
                                                   final UUID mappingTypeId,
                                                   final UUID gameVersionId,
                                                   final UUID userId,
                                                   final UUID parentClassId,
                                                   final UUID parentMethodId,
                                                   final boolean externallyVisibleOnly,
                                                   final Pageable pageable) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "getAll")
                .put("latestOnly", latestOnly)
                .put("versionedMappableId", versionedMappableId)
                .put("releaseId", releaseId)
                .put("mappableType", mappableType)
                .put("inputRegex", inputRegex)
                .put("outputRegex", outputRegex)
                .put("mappingTypeId", mappingTypeId)
                .put("gameVersionId", gameVersionId)
                .put("userId", userId)
                .put("parentClassId", parentClassId)
                .put("parentMethodId", parentMethodId)
                .put("externallyVisibleOnly", externallyVisibleOnly)
                .put("pageable", pageable)
                .build();

        return pageCacheOps.get(
                cacheKey
        ).doFirst(() -> logger.debug("Looking up detailed mappings in cache: {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}.", latestOnly, versionedMappableId, releaseId, mappableType, inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, parentClassId, parentMethodId, externallyVisibleOnly, pageable))
                .doOnNext(page -> logger.debug("Found detailed mappings in cache: {}", page))
                .switchIfEmpty(repository.findAllBy(latestOnly, versionedMappableId, releaseId, this.mappableTypeConverter.toDMO(mappableType), inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, parentClassId, parentMethodId, externallyVisibleOnly, pageable)
                        .doFirst(() -> logger.debug("Looking up detailed mappings in database: {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}, {}.", latestOnly, versionedMappableId, releaseId, mappableType, inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, parentClassId, parentMethodId, externallyVisibleOnly, pageable))
                        .flatMap(page -> Flux.fromIterable(page)
                                .flatMap(this.instancedMappingConverter::toDTO)
                                .collectList()
                                .map(mappings -> (Page<DetailedMappingDTO>) new PageImpl<>(mappings, page.getPageable(), page.getTotalElements())))
                        .doOnNext(page -> logger.debug("Found detailed mappings in database: {}", page))
                        .zipWhen(page -> pageCacheOps.set(cacheKey, page, Duration.ofSeconds(CACHE_LIFETIME_ALL)), (page, a) -> page)
                        .switchIfEmpty(Mono.error(new NoEntriesFoundException("DetailedMapping"))));
    }
}
