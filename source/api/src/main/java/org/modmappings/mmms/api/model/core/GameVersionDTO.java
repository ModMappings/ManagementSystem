package org.modmappings.mmms.api.model.core;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;

public class GameVersionDTO {
    private final UUID      id;
    private final UUID      createdBy;
    private final Timestamp createdOn;
    private String    name;
    private boolean   isPreRelease;
    private boolean   isSnapshot;

    public GameVersionDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, final String name, final boolean isPreRelease, final boolean isSnapshot) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.isPreRelease = isPreRelease;
        this.isSnapshot = isSnapshot;
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

    public String getName() {
        return name;
    }

    public boolean isPreRelease() {
        return isPreRelease;
    }

    public boolean isSnapshot() {
        return isSnapshot;
    }

    public void setName(final String name) {
        this.name = name;
    }

    public void setPreRelease(final boolean preRelease) {
        isPreRelease = preRelease;
    }

    public void setSnapshot(final boolean snapshot) {
        isSnapshot = snapshot;
    }
}
