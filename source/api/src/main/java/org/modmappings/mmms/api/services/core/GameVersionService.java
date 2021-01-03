package org.modmappings.mmms.api.services.core;

import org.modmappings.mmms.api.converters.core.GameVersionConverter;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.util.CacheKeyBuilder;
import org.modmappings.mmms.repository.repositories.core.gameversions.GameVersionRepository;
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
public class GameVersionService {

    @Value("${caching.gameversion.lifetimes.by-id:86400}")
    private int CACHE_LIFETIME_BY_ID;

    @Value("${caching.gameversion.lifetimes.all:3600}")
    private int CACHE_LIFETIME_ALL;

    private final Logger logger = LoggerFactory.getLogger(GameVersionService.class);
    private final GameVersionRepository repository;
    private final GameVersionConverter gameVersionConverter;
    private final ReactiveValueOperations<Map<String, String>, GameVersionDTO> cacheOps;
    private final ReactiveValueOperations<Map<String, String>, Page<GameVersionDTO>> pageCacheOps;

    public GameVersionService(final GameVersionRepository repository, final GameVersionConverter gameVersionConverter, final ReactiveValueOperations<Map<String, String>, GameVersionDTO> cacheOps, final ReactiveValueOperations<Map<String, String>, Page<GameVersionDTO>> pageCacheOps) {
        this.repository = repository;
        this.gameVersionConverter = gameVersionConverter;
        this.cacheOps = cacheOps;
        this.pageCacheOps = pageCacheOps;
    }

    /**
     * Looks up a game version with a given id.
     *
     * @param id The id to look the game version up for.
     * @return A {@link Mono} containing the requested game version or a errored {@link Mono} that indicates a failure.
     */
    public Mono<GameVersionDTO> getBy(final UUID id) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "getById")
                .put("id", id)
                .build();

        return cacheOps.get(
                cacheKey
        ).doFirst(() -> logger.debug("Looking up a game version by id from cache: {}", id))
                .doOnNext(dto -> logger.debug("Found game versions: {} with id from cache: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(repository.findById(id)
                        .doFirst(() -> logger.debug("Looking up a game version by id in database: {}", id))
                        .map(this.gameVersionConverter::toDTO)
                        .doOnNext(dto -> logger.debug("Found game version: {} with id in database: {}", dto.getName(), dto.getId()))
                        .zipWhen((dto) -> cacheOps.set(cacheKey, dto, Duration.ofSeconds(CACHE_LIFETIME_BY_ID)), (dto, aBoolean) -> dto)
                        .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "GameVersion"))));
    }

    /**
     * Looks up multiple game versions, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameRegex  The regular expression against which the name of the game version is matched.
     * @param preRelease Indicates if preReleases need to be included, null indicates do not care.
     * @param snapshot   Indicates if snapshots need to be included, null indicates do not care.
     * @param pageable   The pagination and sorting logic for the request.
     * @return A {@link Flux} with the game versions, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Page<GameVersionDTO>> getAll(final String nameRegex, final Boolean preRelease, final Boolean snapshot, final Pageable pageable) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "getAll")
                .put("nameRegex", nameRegex)
                .put("preRelease", preRelease)
                .put("snapshot", snapshot)
                .put("pageable", pageable)
                .build();

        return pageCacheOps.get(
                cacheKey
        ).doFirst(() -> logger.debug("Looking up game versions in search mode, from cache. Using parameters: {}, {}, {}", nameRegex, preRelease, snapshot))
                .doOnNext(page -> logger.debug("Found game version from cache: {}", page))
                .switchIfEmpty(repository.findAllBy(
                        nameRegex,
                        preRelease,
                        snapshot,
                        pageable)
                        .doFirst(() -> logger.debug("Looking up game versions in search mode in database. Using parameters: {}, {}, {}", nameRegex, preRelease, snapshot))
                        .flatMap(page -> Flux.fromIterable(page)
                                .map(this.gameVersionConverter::toDTO)
                                .collectList()
                                .map(gameVersions -> (Page<GameVersionDTO>) new PageImpl<>(gameVersions, page.getPageable(), page.getTotalElements())))
                        .doOnNext(page -> logger.debug("Found game versions in database: {}", page))
                        .zipWhen(page -> pageCacheOps.set(cacheKey, page, Duration.ofSeconds(CACHE_LIFETIME_ALL)), (page, aBoolean) -> page)
                        .switchIfEmpty(Mono.error(new NoEntriesFoundException("GameVersion"))));
    }
}
