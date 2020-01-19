package org.modmappings.mmms.api.model.core;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.Optional;
import java.util.UUID;

import io.swagger.v3.oas.annotations.media.Schema;
import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;

@Schema(name="GameVersion")
public class GameVersionDTO {
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private UUID      id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private UUID      createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Timestamp createdOn;

    private String    name;

    @Schema(nullable = true)
    private Boolean   isPreRelease;
    @Schema(nullable = true)
    private Boolean   isSnapshot;

    public GameVersionDTO(UUID id, UUID createdBy, Timestamp createdOn, String name, Boolean isPreRelease, Boolean isSnapshot) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.isPreRelease = isPreRelease;
        this.isSnapshot = isSnapshot;
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
        return isPreRelease;
    }

    public void setPreRelease(final Boolean preRelease) {
        isPreRelease = preRelease;
    }

    public Boolean getSnapshot() {
        return isSnapshot;
    }

    public void setSnapshot(final Boolean snapshot) {
        isSnapshot = snapshot;
    }
}
