package org.modmappings.mmms.api.converters.mapping.mappings;

import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.model.mapping.mappings.DistributionDTO;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.DistributionDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.MappingDMO;
import org.springframework.stereotype.Component;

import java.util.UUID;
import java.util.function.Supplier;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of distributions.
 */
@Component
public class MappingConverter {

    private final DistributionConverter distributionConverter;

    public MappingConverter(final DistributionConverter distributionConverter) {
        this.distributionConverter = distributionConverter;
    }

    /**
     * Creates a new mapping DTO from the DMO.
     * Looks up the relevant components of the mapping in the db.
     *
     * @param dmo The DMO to turn into a DTO.
     * @return The mapping DMO in a Mono.
     */
    public MappingDTO toDTO(final MappingDMO dmo) {
        return new MappingDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                dmo.getVersionedMappableId(),
                dmo.getMappingTypeId(),
                dmo.getInput(),
                dmo.getOutput(),
                dmo.getDocumentation(),
                this.distributionConverter.toDTO(dmo.getDistribution())
        );
    }

    /**
     * Creates a new initial DMO from the given parameters.
     * Pulls the mapping data from the DTO passed in,
     * additionally pulls the creating user id from the supplier.
     *
     * @param versionedMappableId The id of the versioned mappable to create a mapping for.
     * @param mappingTypeId       The id of the mapping type to create a mapping for.
     * @param dto                 The DTO to pull mapping data from.
     * @param userIdSupplier      The supplier for the ID of the creating user.
     * @return The initial DMO with the data.
     */
    public MappingDMO toNewDMO(final UUID versionedMappableId,
                                final UUID mappingTypeId,
                                final MappingDTO dto,
                                final Supplier<UUID> userIdSupplier) {
        return new MappingDMO(
                userIdSupplier.get(),
                versionedMappableId,
                mappingTypeId,
                dto.getInput(),
                dto.getOutput(),
                dto.getDocumentation(),
                this.distributionConverter.toDMO(dto.getDistribution())
        );
    }


}