package org.modmappings.mmms.api.services.objects;

import org.modmappings.mmms.api.services.mapping.mappable.MappableService;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.repository.repositories.objects.PackageRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Component;
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
public class PackageService {

    private final Logger logger = LoggerFactory.getLogger(PackageService.class);
    private final PackageRepository repository;

    public PackageService(final PackageRepository repository) {
        this.repository = repository;
    }

    public Mono<Page<String>> findAllBy(
            final UUID gameVersion,
            final UUID releaseId,
            final UUID mappingTypeId,
            final String packagePrefix,
            final Integer minAdditionalPackageDepth,
            final Integer maxAdditionalPackageDepth,
            final Pageable pageable
    ) {
        return repository.findAllBy(gameVersion, releaseId, mappingTypeId, packagePrefix, minAdditionalPackageDepth, maxAdditionalPackageDepth, pageable)
                .doFirst(() -> logger.debug("Looking up a packages by: {}, {}, {}, {}, {}, {}.", gameVersion, releaseId, mappingTypeId, packagePrefix, minAdditionalPackageDepth, maxAdditionalPackageDepth));
    }
}
