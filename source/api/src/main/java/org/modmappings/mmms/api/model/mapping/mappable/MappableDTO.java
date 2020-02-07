package org.modmappings.mmms.api.model.mapping.mappable;

import io.swagger.v3.oas.annotations.media.Schema;

import java.sql.Timestamp;
import java.util.UUID;

@Schema(name="Mappable", description = "Represents a single piece of the sourcecode whose name can be remapped.")
public class MappableDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mappable.")
    private UUID id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the mappable.")
    private UUID createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the mappable was created.")
    private Timestamp createdOn;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The type (package, class, method, field or parameter) that this mappable represents.")
    private MappableTypeDTO type;

    public MappableDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, final MappableTypeDTO type) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.type = type;
    }

    public MappableDTO() {
    }

    public UUID getId() {
        return id;
    }

    public UUID getCreatedBy() {
        return createdBy;
    }

    public Timestamp getCreatedOn() {
        return createdOn;
    }

    public MappableTypeDTO getType() {
        return type;
    }
}
