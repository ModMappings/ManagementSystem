package org.modmappings.mmms.repository.model.mapping.mappable;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

/**
 * Represents a single mappable object in the source code of the game, across versions, mapping types and releases.
 */
@Table("mappable")
public class MappableDMO {

    @Id
    private UUID id;
    private UUID createdBy;
    private Timestamp createdOn;
    private MappableTypeDMO type;

    @PersistenceConstructor
    MappableDMO(UUID id, UUID createdBy, Timestamp createdOn, MappableTypeDMO type) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.type = type;
    }

    public MappableDMO(UUID createdBy, MappableTypeDMO type) {
        this.id = null;
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.type = type;
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

    public MappableTypeDMO getType() {
        return type;
    }
}
