package org.modmappings.mmms.api.model.core;

import io.swagger.v3.oas.annotations.media.Schema;

import javax.validation.constraints.NotBlank;
import java.sql.Timestamp;
import java.util.UUID;

@Schema(name="GameVersion")
public class GameVersionDTO {
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the game version.")
    private UUID      id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the game version.")
    private UUID      createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the game version was created.")
    private Timestamp createdOn;

    @Schema(description = "The name of the game version. Has to be unique", minLength = 1, required = true)
    @NotBlank
    private String    name;
    @Schema(description = "Indicates if this game version is a prerelease.", required = true)
    private Boolean preRelease;
    @Schema(description = "Indicates if this game version is a snapshot or not.", required = true)
    private Boolean snapshot;

    public GameVersionDTO(UUID id, UUID createdBy, Timestamp createdOn, String name, Boolean preRelease, Boolean snapshot) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.preRelease = preRelease;
        this.snapshot = snapshot;
    }

    public GameVersionDTO() {
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

    public Boolean getPreRelease() {
        return preRelease;
    }

    public void setPreRelease(final Boolean preRelease) {
        this.preRelease = preRelease;
    }

    public Boolean getSnapshot() {
        return snapshot;
    }

    public void setSnapshot(final Boolean snapshot) {
        this.snapshot = snapshot;
    }
}
