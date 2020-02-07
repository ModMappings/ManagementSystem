package org.modmappings.mmms.repository.model.core;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

/**
 * Represents a single mapping type.
 * So for example from OBF to SRG or from OBF to Intermediary or from SRG to MCP.
 */
@Table("mapping_type")
public class MappingTypeDMO {

    @Id
    private UUID id;
    private UUID      createdBy;
    private Timestamp createdOn;
    private String    name;
    private boolean visible;
    private boolean editable;
    private String stateIn;
    private String stateOut;

    @PersistenceConstructor
    MappingTypeDMO(final UUID id, final UUID createdBy, final Timestamp createdOn, final String name, final boolean visible, final boolean editable, final String stateIn, final String stateOut) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.visible = visible;
        this.editable = editable;
        this.stateIn = stateIn;
        this.stateOut = stateOut;
    }

    public MappingTypeDMO(final UUID createdBy, final String name, final boolean visible, final boolean editable, final String stateIn, final String stateOut) {
        this.visible = visible;
        this.editable = editable;
        this.id = null;
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.name = name;
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

    public String getStateIn() {
        return stateIn;
    }

    public String getStateOut() {
        return stateOut;
    }

    public void setStateIn(final String stateIn) {
        this.stateIn = stateIn;
    }

    public void setStateOut(final String stateOut) {
        this.stateOut = stateOut;
    }

    public void setName(final String name) {
        this.name = name;
    }

    public boolean isVisible() {
        return visible;
    }

    public boolean isEditable() {
        return editable;
    }

    public void setEditable(final boolean editable) {
        this.editable = editable;
    }




}
