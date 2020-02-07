package org.modmappings.mmms.repository.model.core.release;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.util.UUID;

/**
 * Represents a single mapping that is part of a given release.
 * Models the many to many relationship between releases and mappings.
 */
@Table("release_component")
public class ReleaseComponentDMO {

    @Id
    private UUID id;
    private UUID releaseId;
    private UUID mappingId;

    @PersistenceConstructor
    ReleaseComponentDMO(final UUID id, final UUID releaseId, final UUID mappingId) {
        this.id = id;
        this.releaseId = releaseId;
        this.mappingId = mappingId;
    }

    public ReleaseComponentDMO(final UUID releaseId, final UUID mappingId) {
        this.id = null;
        this.releaseId = releaseId;
        this.mappingId = mappingId;
    }

    public UUID getId() {
        return id;
    }

    public UUID getReleaseId() {
        return releaseId;
    }

    public UUID getMappingId() {
        return mappingId;
    }
}
