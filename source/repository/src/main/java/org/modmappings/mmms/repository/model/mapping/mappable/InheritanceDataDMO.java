package org.modmappings.mmms.repository.model.mapping.mappable;

import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

@Table("inheritance_data")
public class InheritanceDataDMO {
    @Id
    private UUID id;
    private UUID superTypeVersionedMappableId;
    private UUID subTypeVersionedMappableId;

    @PersistenceConstructor
    InheritanceDataDMO(UUID id, UUID superTypeVersionedMappableId, UUID subTypeVersionedMappableId) {
        this.id = id;
        this.superTypeVersionedMappableId = superTypeVersionedMappableId;
        this.subTypeVersionedMappableId = subTypeVersionedMappableId;
    }

    public InheritanceDataDMO(UUID superTypeVersionedMappableId, UUID subTypeVersionedMappableId) {
        this.id = null;
        this.superTypeVersionedMappableId = superTypeVersionedMappableId;
        this.subTypeVersionedMappableId = subTypeVersionedMappableId;
    }
}
