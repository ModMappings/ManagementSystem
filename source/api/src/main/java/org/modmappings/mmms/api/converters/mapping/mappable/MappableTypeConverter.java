package org.modmappings.mmms.api.converters.mapping.mappable;

import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.springframework.stereotype.Component;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of mappable types.
 */
@Component
public class MappableTypeConverter {
    public MappableTypeDTO toDTO(final MappableTypeDMO dmo) {
        if (dmo == null)
            return null;

        return MappableTypeDTO.valueOf(MappableTypeDTO.class, dmo.name());
    }

    public MappableTypeDMO toDMO(final MappableTypeDTO dto) {
        if (dto == null)
            return null;

        return MappableTypeDMO.valueOf(MappableTypeDMO.class, dto.name());
    }
}
