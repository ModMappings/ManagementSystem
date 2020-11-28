package org.modmappings.mmms.api.converters.release;

import org.modmappings.mmms.api.model.core.release.ReleaseDTO;
import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.springframework.stereotype.Component;

import java.util.UUID;
import java.util.function.Supplier;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of releases.
 */
@Component
public class ReleaseConverter {

    /**
     * Creates a new release DTO from the DMO.
     * Looks up the relevant components of the release in the db.
     *
     * @param dmo The DMO to turn into a DTO.
     * @return The release DMO in a Mono.
     */
    public ReleaseDTO toDTO(final ReleaseDMO dmo) {
        return new ReleaseDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                dmo.getName(),
                dmo.getGameVersionId(),
                dmo.getMappingTypeId(),
                dmo.isSnapshot(),
                dmo.getState());
    }

    /**
     * Creates a new initial DMO from the given parameters.
     * Pulls the name and snapshot state from the DTO passed in,
     * additionally pulls the creating user id from the supplier.
     *
     * @param gameVersionId  The id of the game version to create a release for.
     * @param mappingTypeId  The id of the mapping type to create a release for.
     * @param dto            The DTO to pull name and snapshot state from.
     * @param userIdSupplier The supplier for the ID of the creating user.
     * @return The initial DMO with the data.
     */
    public ReleaseDMO toNewDMO(
            final UUID gameVersionId,
            final UUID mappingTypeId,
            final ReleaseDTO dto,
            final Supplier<UUID> userIdSupplier) {
        return new ReleaseDMO(
                userIdSupplier.get(),
                dto.getName(),
                gameVersionId,
                mappingTypeId,
                dto.isSnapshot(),
                dto.getState()
        );
    }

    /**
     * Updates an existing DMO with information from the DTO.
     *
     * @param dto The DTO to pull information from.
     * @param dmo The DMO to write information into.
     */
    public void updateDMO(final ReleaseDTO dto,
                           final ReleaseDMO dmo) {
        dmo.setName(dto.getName());
        dmo.setSnapshot(dto.isSnapshot());
        dmo.setState(dto.getState());
    }
}