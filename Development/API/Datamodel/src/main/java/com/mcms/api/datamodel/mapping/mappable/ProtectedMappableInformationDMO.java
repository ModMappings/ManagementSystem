package com.mcms.api.datamodel.mapping.mappable;

import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

/**
 * An record that indicates a many to many relationship between a component in a given game version, as well
 * as a given mapping type.
 * If such a record exists, no proposals can be made for the given component in the given game version with the given mapping type.
 */
@Table("protected_mappable")
public class ProtectedMappableInformationDMO {

    @Id
    private final UUID id;
    private final UUID mappableInGameVersionId;
    private final UUID mappingType;

    @PersistenceConstructor
    ProtectedMappableInformationDMO(final UUID id, final UUID mappableInGameVersionId, final UUID mappingType) {
        this.id = id;
        this.mappableInGameVersionId = mappableInGameVersionId;
        this.mappingType = mappingType;
    }

    public ProtectedMappableInformationDMO(final UUID mappableInGameVersionId, final UUID mappingType) {
        this.id = UUID.randomUUID();
        this.mappableInGameVersionId = mappableInGameVersionId;
        this.mappingType = mappingType;
    }

    public UUID getId() {
        return id;
    }

    public UUID getMappableInGameVersionId() {
        return mappableInGameVersionId;
    }

    public UUID getMappingType() {
        return mappingType;
    }
}

