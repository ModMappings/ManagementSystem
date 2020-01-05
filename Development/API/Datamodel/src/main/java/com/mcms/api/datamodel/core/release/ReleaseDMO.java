package com.mcms.api.datamodel.core.release;

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
    private final UUID id;
    private final UUID createdBy;
    private final Timestamp createdOn;
    private final String name;
    private final UUID gameVersionId;
    private final UUID mappingType;
    private final boolean isSnapshot;

    @PersistenceConstructor
    ReleaseDMO(
                    final UUID id,
                    final UUID createdBy,
                    final Timestamp createdOn,
                    final String name,
                    final UUID gameVersionId,
                    final UUID mappingType,
                    final boolean isSnapshot) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.gameVersionId = gameVersionId;
        this.mappingType = mappingType;
        this.isSnapshot = isSnapshot;
    }

    public ReleaseDMO(final UUID createdBy, final String name, final UUID gameVersionId, final UUID mappingType, final boolean isSnapshot) {
        this.id = UUID.randomUUID();
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
}
