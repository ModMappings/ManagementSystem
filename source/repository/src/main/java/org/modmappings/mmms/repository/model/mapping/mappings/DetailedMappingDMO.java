package org.modmappings.mmms.repository.model.mapping.mappings;

import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.VersionedMappableDMO;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

/**
 * Represents a single mapping which is "live".
 * It is committed to be used in the public, and will be included in the next release.
 */
@Table("mapping_snapshot")
public class DetailedMappingDMO {

    private MappingDMO mapping;
    private VersionedMappableDMO versionedMappable;
    private MappableDMO mappable;

    @PersistenceConstructor
    public DetailedMappingDMO() {
    }

    public DetailedMappingDMO(final MappingDMO mapping, final VersionedMappableDMO versionedMappable, final MappableDMO mappable) {
        this.mapping = mapping;
        this.versionedMappable = versionedMappable;
        this.mappable = mappable;
    }

    public MappingDMO getMapping() {
        return mapping;
    }

    public void setMapping(final MappingDMO mapping) {
        this.mapping = mapping;
    }

    public VersionedMappableDMO getVersionedMappable() {
        return versionedMappable;
    }

    public void setVersionedMappable(final VersionedMappableDMO versionedMappable) {
        this.versionedMappable = versionedMappable;
    }

    public MappableDMO getMappable() {
        return mappable;
    }

    public void setMappable(final MappableDMO mappable) {
        this.mappable = mappable;
    }
}
