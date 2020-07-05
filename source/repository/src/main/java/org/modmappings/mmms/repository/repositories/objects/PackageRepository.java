package org.modmappings.mmms.repository.repositories.objects;

import org.modmappings.mmms.repository.model.objects.PackageDMO;
import org.springframework.data.domain.Page;
import org.springframework.stereotype.Repository;

import java.util.UUID;

/**
 * A repository that provides access to package related information on a mapping level.
 */
@Repository
public interface PackageRepository {

    Page<PackageDMO> findAllBy(
            final UUID gameVersion,
            final UUID releaseId,
            final UUID mappingTypeId,
            final String packagePrefix,
            final Integer minAdditionalPackageDepth,
            final Integer maxAdditionalPackageDepth
    );
}
