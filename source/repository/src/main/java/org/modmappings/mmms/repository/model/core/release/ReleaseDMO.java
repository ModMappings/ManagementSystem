package org.modmappings.mmms.repository.model.core.release;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

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
    private UUID mappingType;
    private boolean isSnapshot;

    @PersistenceConstructor
    ReleaseDMO(
                    UUID id,
                    UUID createdBy,
                    Timestamp createdOn,
                    String name,
                    UUID gameVersionId,
                    UUID mappingType,
                    boolean isSnapshot) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.gameVersionId = gameVersionId;
        this.mappingType = mappingType;
        this.isSnapshot = isSnapshot;
    }

    public ReleaseDMO(UUID createdBy, String name, UUID gameVersionId, UUID mappingType, boolean isSnapshot) {
        this.id = null;
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.name = name;
        this.gameVersionId = gameVersionId;
        this.mappingType = mappingType;
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

    public UUID getGameVersionId() {
        return gameVersionId;
    }

    public UUID getMappingType() {
        return mappingType;
    }

    public boolean isSnapshot() {
        return isSnapshot;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setSnapshot(boolean snapshot) {
        isSnapshot = snapshot;
    }
}
