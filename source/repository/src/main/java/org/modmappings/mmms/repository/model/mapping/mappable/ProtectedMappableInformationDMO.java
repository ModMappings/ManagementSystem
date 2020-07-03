package org.modmappings.mmms.repository.model.mapping.mappable;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.util.UUID;

/**
 * A record which indicates a many-to-many relationship between a component in a given game version, as well
 * as a given mapping type.
 * If such a record exists, no proposals can be made for the given component in the given game version with the given mapping type.
 */
@Table("protected_mappable")
public class ProtectedMappableInformationDMO {

    @Id
    private UUID id;
    private UUID versionedMappableId;
    private UUID mappingTypeId;

    @PersistenceConstructor
    public ProtectedMappableInformationDMO(final UUID id, final UUID versionedMappableId, final UUID mappingTypeId) {
        this.id = id;
        this.versionedMappableId = versionedMappableId;
        this.mappingTypeId = mappingTypeId;
    }

    public ProtectedMappableInformationDMO(final UUID versionedMappableId, final UUID mappingTypeId) {
        this.id = null;
        this.versionedMappableId = versionedMappableId;
        this.mappingTypeId = mappingTypeId;
    }

    public UUID getId() {
        return id;
    }

    public UUID getVersionedMappableId() {
        return versionedMappableId;
    }

    public UUID getMappingTypeId() {
        return mappingTypeId;
    }
}

