package org.modmappings.mmms.api.services.objects;

import org.modmappings.mmms.api.converters.mapping.mappings.MappingConverter;
import org.modmappings.mmms.api.converters.objects.PackageConverter;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;
import org.modmappings.mmms.api.model.objects.PackageDTO;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.util.CacheKeyBuilder;
import org.modmappings.mmms.repository.model.objects.PackageDMO;
import org.modmappings.mmms.repository.repositories.objects.PackageRepository;
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
public class PackageService {

    @Value("${caching.package.lifetimes.all:3600}")
    private int CACHE_LIFETIME_ALL;

    private final Logger logger = LoggerFactory.getLogger(PackageService.class);
    private final PackageRepository repository;
    private final PackageConverter packageConverter;
    private final ReactiveValueOperations<Map<String, String>, Page<PackageDTO>> pageCacheOps;

    public PackageService(final PackageRepository repository, final PackageConverter packageConverter, final ReactiveValueOperations<Map<String, String>, Page<PackageDTO>> pageCacheOps) {
        this.repository = repository;
        this.packageConverter = packageConverter;
        this.pageCacheOps = pageCacheOps;
    }

    public Mono<Page<PackageDTO>> findAllBy(
            final Boolean latestOnly,
            final UUID gameVersion,
            final UUID releaseId,
            final UUID mappingTypeId,
            final String matchingRegex,
            final String parentPackagePath,
            final boolean externallyVisibleOnly,
            final Pageable pageable
    ) {
        final Map<String, String> cacheKey = CacheKeyBuilder.create()
                .put("ops", "findAllBy")
                .put("latestOnly", latestOnly)
                .put("gameVersion", gameVersion)
                .put("releaseId", releaseId)
                .put("mappingTypeId", mappingTypeId)
                .put("matchingRegex", matchingRegex)
                .put("externallyVisibleOnly", externallyVisibleOnly)
                .put("parentPackagePath", parentPackagePath)
                .put("pageable", pageable)
                .build();

        return pageCacheOps.get(
                cacheKey
        ).doFirst(() -> logger.debug("Looking up a packages in cache by: {}, {}, {}, {}, {}, {}, {}.",latestOnly, gameVersion, releaseId, mappingTypeId, matchingRegex, parentPackagePath, externallyVisibleOnly))
                .doOnNext((page) -> logger.debug("Found packages in cache: {}", page))
                .switchIfEmpty(repository.findAllBy(latestOnly, gameVersion, releaseId, mappingTypeId, matchingRegex, parentPackagePath, externallyVisibleOnly, pageable)
                        .doFirst(() -> logger.debug("Looking up a packages by in database: {}, {}, {}, {}, {}, {}, {}.",latestOnly, gameVersion, releaseId, mappingTypeId, matchingRegex, parentPackagePath, externallyVisibleOnly))
                        .doOnNext((page) -> logger.debug("Found packages in database: {}", page))
                        .flatMap(page -> Flux.fromIterable(page)
                                .map(this.packageConverter::toDTO)
                                .collectList()
                                .map(mappings -> (Page<PackageDTO>) new PageImpl<>(mappings, page.getPageable(), page.getTotalElements())))
                        .zipWhen(page -> pageCacheOps.set(cacheKey, page, Duration.ofSeconds(CACHE_LIFETIME_ALL)), (page, a) -> page)
                        .switchIfEmpty(Mono.error(new NoEntriesFoundException("Packages"))));
    }
}
