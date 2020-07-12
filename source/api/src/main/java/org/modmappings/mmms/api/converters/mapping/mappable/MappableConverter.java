package org.modmappings.mmms.api.converters.mapping.mappable;

import org.modmappings.mmms.api.model.mapping.mappable.MappableDTO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.springframework.stereotype.Component;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of mappables.
 */
@Component
public class MappableConverter {

    private final MappableTypeConverter mappableTypeConverter;

    public MappableConverter(final MappableTypeConverter mappableTypeConverter) {
        this.mappableTypeConverter = mappableTypeConverter;
    }

    public MappableDTO toDTO(final MappableDMO dmo) {
        return new MappableDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                this.mappableTypeConverter.toDTO(dmo.getType())
        );
    }

    public MappableDMO toDMO(final MappableDTO dto) {
        return new MappableDMO(
                dto.getId(),
                dto.getCreatedBy(),
                dto.getCreatedOn(),
                this.mappableTypeConverter.toDMO(dto.getType())
        );
    }

    public MappableDMO toNewDMO(final MappableDTO dto)
    {
        return new MappableDMO(
                dto.getCreatedBy(),
                this.mappableTypeConverter.toDMO(dto.getType())
        );
    }
}
