package org.modmappings.mmms.api.converters.mapping.mappings;

import org.modmappings.mmms.api.model.mapping.mappings.DistributionDTO;
import org.modmappings.mmms.repository.model.mapping.mappings.DistributionDMO;
import org.springframework.stereotype.Component;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of distributions.
 */
@Component
public class DistributionConverter {

    /**
     * Converts a given distributions dmo value into a valid dto.
     *
     * @param dmo The dmo to convert.
     * @return Returns the dto value of the given dmo. If null is passed in then {@link DistributionDTO#UNKNOWN} is returned.
     */
    public DistributionDTO toDTO(final DistributionDMO dmo) {
        if (dmo == null)
            return DistributionDTO.UNKNOWN;

        return DistributionDTO.valueOf(dmo.name());
    }

    /**
     * Converts a given distributions dto value into a valid dmo.
     *
     * @param dto The dto to convert.
     * @return Returns the dmo value of the given dto. If null is passed in then {@link DistributionDMO#UNKNOWN} is returned.
     */
    public DistributionDMO toDMO(final DistributionDTO dto) {
        if (dto == null)
            return DistributionDMO.UNKNOWN;

        return DistributionDMO.valueOf(dto.name());
    }
}
