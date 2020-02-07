package org.modmappings.mmms.repository.model.mapping.mappable;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

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
    MappableDMO(final UUID id, final UUID createdBy, final Timestamp createdOn, final MappableTypeDMO type) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.type = type;
    }

    public MappableDMO(final UUID createdBy, final MappableTypeDMO type) {
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
