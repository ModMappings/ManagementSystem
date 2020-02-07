package org.modmappings.mmms.repository.model.mapping.mappable;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.util.UUID;

@Table("inheritance_data")
public class InheritanceDataDMO {
    @Id
    private UUID id;
    private UUID superTypeVersionedMappableId;
    private UUID subTypeVersionedMappableId;

    @PersistenceConstructor
    InheritanceDataDMO(final UUID id, final UUID superTypeVersionedMappableId, final UUID subTypeVersionedMappableId) {
        this.id = id;
        this.superTypeVersionedMappableId = superTypeVersionedMappableId;
        this.subTypeVersionedMappableId = subTypeVersionedMappableId;
    }

    public InheritanceDataDMO(final UUID superTypeVersionedMappableId, final UUID subTypeVersionedMappableId) {
        this.id = null;
        this.superTypeVersionedMappableId = superTypeVersionedMappableId;
        this.subTypeVersionedMappableId = subTypeVersionedMappableId;
    }

    public UUID getId() {
        return id;
    }

    public UUID getSuperTypeVersionedMappableId() {
        return superTypeVersionedMappableId;
    }

    public UUID getSubTypeVersionedMappableId() {
        return subTypeVersionedMappableId;
    }
}
