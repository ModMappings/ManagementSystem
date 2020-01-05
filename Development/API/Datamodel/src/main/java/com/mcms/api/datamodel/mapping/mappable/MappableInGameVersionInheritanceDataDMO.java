package com.mcms.api.datamodel.mapping.mappable;

import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

@Table("mappable_inheritance_data")
public class MappableInGameVersionInheritanceDataDMO {
    @Id
    private final UUID id;
    private final UUID superTypeMappableInGameVersionId;
    private final UUID subTypeMappableInGameVersionId;

    @PersistenceConstructor
    MappableInGameVersionInheritanceDataDMO(final UUID id, final UUID superTypeMappableInGameVersionId, final UUID subTypeMappableInGameVersionId) {
        this.id = id;
        this.superTypeMappableInGameVersionId = superTypeMappableInGameVersionId;
        this.subTypeMappableInGameVersionId = subTypeMappableInGameVersionId;
    }

    public MappableInGameVersionInheritanceDataDMO(final UUID superTypeMappableInGameVersionId, final UUID subTypeMappableInGameVersionId) {
        this.id = UUID.randomUUID();
        this.superTypeMappableInGameVersionId = superTypeMappableInGameVersionId;
        this.subTypeMappableInGameVersionId = subTypeMappableInGameVersionId;
    }
}
