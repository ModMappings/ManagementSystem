package org.modmappings.mmms.api.converters.mapping.mappable;

import org.modmappings.mmms.api.model.mapping.mappable.VisibilityDTO;
import org.modmappings.mmms.repository.model.mapping.mappable.VisibilityDMO;
import org.springframework.stereotype.Component;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of visibility information.
 */
@Component
public class VisibilityConverter {

    public VisibilityDTO toDTO(final VisibilityDMO dmo) {
        if (dmo == null)
            return null;

        return VisibilityDTO.valueOf(VisibilityDTO.class, dmo.name());
    }

    public VisibilityDMO toDMO(final VisibilityDTO dto) {
        if (dto == null)
            return null;

        return VisibilityDMO.valueOf(VisibilityDMO.class, dto.name());
    }
}
