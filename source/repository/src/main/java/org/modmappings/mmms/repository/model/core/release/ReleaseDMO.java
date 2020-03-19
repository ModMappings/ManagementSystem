package org.modmappings.mmms.repository.model.core.release;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

/**
 * Represents a single release of data from the MCMS or from an external system from which data has been imported.
 *
 * A release always targets a given game version as well as a given mapping type.
 */
@Table("release")
public class ReleaseDMO {

    @Id
    private UUID id;
    private UUID createdBy;
    private Timestamp createdOn;
    private String name;
    private UUID gameVersionId;
    private UUID mappingTypeId;
    private boolean isSnapshot;
    private String state;

    @PersistenceConstructor
    public ReleaseDMO(final UUID id, final UUID createdBy, final Timestamp createdOn, final String name, final UUID gameVersionId, final UUID mappingTypeId, final boolean isSnapshot, final String state) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.gameVersionId = gameVersionId;
        this.mappingTypeId = mappingTypeId;
        this.isSnapshot = isSnapshot;
        this.state = state;
    }

    public ReleaseDMO(final UUID createdBy, final String name, final UUID gameVersionId, final UUID mappingTypeId, final boolean isSnapshot, final String state) {
        this.createdBy = createdBy;
        this.name = name;
        this.gameVersionId = gameVersionId;
        this.mappingTypeId = mappingTypeId;
        this.isSnapshot = isSnapshot;
        this.state = state;
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

    public UUID getGameVersionId() {
        return gameVersionId;
    }

    public UUID getMappingTypeId() {
        return mappingTypeId;
    }

    public boolean isSnapshot() {
        return isSnapshot;
    }

    public void setName(final String name) {
        this.name = name;
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
