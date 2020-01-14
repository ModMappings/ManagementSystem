package org.modmappings.mmms.api.model.core;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.Optional;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;

public class GameVersionDTO {
    private final UUID      id;
    private final UUID      createdBy;
    private final Timestamp createdOn;
    private String    name;
    private Optional<Boolean>   isPreRelease;
    private Optional<Boolean>   isSnapshot;

    public GameVersionDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, final String name, final Optional<Boolean> isPreRelease, final Optional<Boolean> isSnapshot) {
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

    public Optional<Boolean> isPreRelease() {
        return isPreRelease;
    }

    public Optional<Boolean> isSnapshot() {
        return isSnapshot;
    }

    public void setName(final String name) {
        this.name = name;
    }

    public void setPreRelease(final Optional<Boolean> preRelease) {
        isPreRelease = preRelease;
    }

    public void setSnapshot(final Optional<Boolean> snapshot) {
        isSnapshot = snapshot;
    }
}
