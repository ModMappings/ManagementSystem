package org.modmappings.mmms.api.converters.core;

import org.modmappings.mmms.api.model.core.MappingTypeDTO;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.springframework.stereotype.Component;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of mapping types.
 */
@Component
public class MappingTypeConverter {

    public MappingTypeDTO toDTO(final MappingTypeDMO dmo) {
        return new MappingTypeDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                dmo.getName(),
                dmo.isEditable()
        );
    }
}
