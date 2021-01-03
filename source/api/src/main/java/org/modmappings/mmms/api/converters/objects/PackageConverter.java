package org.modmappings.mmms.api.converters.objects;

import org.modmappings.mmms.api.model.objects.PackageDTO;
import org.modmappings.mmms.repository.model.objects.PackageDMO;
import org.springframework.stereotype.Component;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of packages.
 */
@Component
public class PackageConverter {

    public PackageDTO toDTO(final PackageDMO dmo) {
        return new PackageDTO(
                dmo.getPath(),
                dmo.getName(),
                dmo.getParentPath(),
                dmo.getParentParentPath(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn());
    }
}
