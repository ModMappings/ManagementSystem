package org.modmappings.mmms.api.model.core.release;

import io.swagger.v3.oas.annotations.media.Schema;

import javax.validation.constraints.NotBlank;
import javax.validation.constraints.NotNull;
import java.sql.Timestamp;
import java.util.Set;
import java.util.UUID;

@Schema(name = "Release")
public class ReleaseDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the release.")
    private UUID id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the release.")
    private UUID createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the release was created.")
    private Timestamp createdOn;

    @Schema(description = "The name of the release, has to be unique inside a given mapping type.", minLength = 1, required = true)
    @NotBlank
    private String name;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the game version for which this release is.", required = true, example = "9b4a9c76-3588-48b5-bedf-b0df90b00381")
    private UUID gameVersionId;
    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mapping type for which this release is.", required = true, example = "9b4a9c76-3588-48b5-bedf-b0df90b00381")
    private UUID mappingType;

    @Schema(description = "Indicates if this release is a snapshot or not. Snapshot release are potentially not stable.", required = true)
    private boolean isSnapshot;

    @NotNull
    @Schema(description = "The state of the release. Indicates how much of the release has been imported. Most releases have FIELD as final state. However PARAMETER is also possible.")
    private String state;

    public ReleaseDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, @NotBlank final String name, @NotNull final UUID gameVersionId, @NotNull final UUID mappingType, final boolean isSnapshot, final String state) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.gameVersionId = gameVersionId;
        this.mappingType = mappingType;
        this.isSnapshot = isSnapshot;
        this.state = state;
    }

    public ReleaseDTO() {
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

    public UUID getGameVersionId() {
        return gameVersionId;
    }

    public void setGameVersionId(final UUID gameVersionId) {
        this.gameVersionId = gameVersionId;
    }

    public UUID getMappingType() {
        return mappingType;
    }

    public void setMappingType(final UUID mappingType) {
        this.mappingType = mappingType;
    }

    public boolean isSnapshot() {
        return isSnapshot;
    }

    public void setSnapshot(final boolean snapshot) {
        isSnapshot = snapshot;
    }

    public String getState() {
        return state;
    }

    public void setState(final String state) {
        this.state = state;
    }
}
