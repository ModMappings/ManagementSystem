package org.modmapping.mmms.repository.model.mapping.mappable;

import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

@Table("inheritance_data")
public class InheritanceDataDMO {
    @Id
    private final UUID id;
    private final UUID superTypeVersionedMappableId;
    private final UUID subTypeVersionedMappableId;

    @PersistenceConstructor
    InheritanceDataDMO(final UUID id, final UUID superTypeVersionedMappableId, final UUID subTypeVersionedMappableId) {
        this.id = id;
        this.superTypeVersionedMappableId = superTypeVersionedMappableId;
        this.subTypeVersionedMappableId = subTypeVersionedMappableId;
    }

    public InheritanceDataDMO(final UUID superTypeVersionedMappableId, final UUID subTypeVersionedMappableId) {
        this.id = UUID.randomUUID();
        this.superTypeVersionedMappableId = superTypeVersionedMappableId;
        this.subTypeVersionedMappableId = subTypeVersionedMappableId;
    }
}
