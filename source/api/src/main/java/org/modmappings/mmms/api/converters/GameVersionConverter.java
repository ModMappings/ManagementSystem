package org.modmappings.mmms.api.converters;

import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.springframework.stereotype.Component;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of gameversions.
 */
@Component
public class GameVersionConverter {

    public GameVersionDTO toDTO(final GameVersionDMO dmo) {
        return new GameVersionDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                dmo.getName(),
                dmo.isPreRelease(),
                dmo.isSnapshot()
        );
    }
}
