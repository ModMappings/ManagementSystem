package org.modmappings.mmms.api.model.core;

import io.swagger.v3.oas.annotations.media.Schema;

import javax.validation.constraints.NotBlank;
import java.sql.Timestamp;
import java.util.UUID;

@Schema(name="MappingType")
public class MappingTypeDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mapping type.")
    private UUID id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the mapping type.")
    private UUID      createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the mapping type was created.")
    private Timestamp createdOn;
    @Schema(description = "The name of the mapping type. Has to be unique.", minLength = 1, required = true)
    @NotBlank
    private String    name;
    @Schema(description = "Indicates if this mapping type is editable or not. For none editable mapping types the api prevents users from making and proposing new mappings. Making this mapping type basically only available for lookup purposes!")
    private boolean editable;
    @Schema(description = "The state of the source code that this mapping type maps away from.", required = true, minLength = 1)
    private String stateIn;
    @Schema(description = "The state of the source code that this mapping type maps into.", required = true, minLength = 1)
    private String stateOut;

    public MappingTypeDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, @NotBlank final String name, final boolean editable) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.editable = editable;
    }

    public MappingTypeDTO() {
    }

    public UUID getId() {
        return id;
    }

    public void setId(final UUID id) {
        this.id = id;
    }

    public UUID getCreatedBy() {
        return createdBy;
    }

    public void setCreatedBy(final UUID createdBy) {
        this.createdBy = createdBy;
    }

    public Timestamp getCreatedOn() {
        return createdOn;
    }

    public void setCreatedOn(final Timestamp createdOn) {
        this.createdOn = createdOn;
    }

    public String getName() {
        return name;
    }

    public void setName(final String name) {
        this.name = name;
    }

    public boolean isEditable() {
        return editable;
    }

    public void setEditable(final boolean editable) {
        this.editable = editable;
    }

    public String getStateIn() {
        return stateIn;
    }

    public void setStateIn(final String stateIn) {
        this.stateIn = stateIn;
    }

    public String getStateOut() {
        return stateOut;
    }

    public void setStateOut(final String stateOut) {
        this.stateOut = stateOut;
    }
}
