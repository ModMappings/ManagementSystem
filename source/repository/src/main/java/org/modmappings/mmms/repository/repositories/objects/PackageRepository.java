package org.modmappings.mmms.repository.repositories.objects;

import org.modmappings.mmms.repository.model.mapping.mappable.VersionedMappableDMO;
import org.modmappings.mmms.repository.model.objects.PackageDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * A repository that provides access to package related information on a mapping level.
 */
@Repository
public interface PackageRepository extends org.springframework.data.repository.Repository<VersionedMappableDMO, UUID> {

    Mono<Page<PackageDMO>> findAllBy(
            final Boolean latestOnly,
            final UUID gameVersion,
            final UUID releaseId,
            final UUID mappingTypeId,
            final String matchingRegex,
            final String parentPackagePath,
            final boolean externallyVisibleOnly,
            final Pageable pageable
    );
}
