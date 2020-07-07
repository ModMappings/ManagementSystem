package org.modmappings.mmms.repository.repositories.objects;

import org.modmappings.mmms.repository.model.mapping.mappable.VersionedMappableDMO;
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

    Mono<Page<String>> findAllBy(
            final UUID gameVersion,
            final UUID releaseId,
            final UUID mappingTypeId,
            String packagePrefix,
            Integer minAdditionalPackageDepth,
            Integer maxAdditionalPackageDepth,
            final Pageable pageable
            );
}
