package org.modmappings.mmms.repository.model.mapping.mappings;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

/**
 * Represents a single mapping which is "live".
 * It is committed to be used in the public, and will be included in the next release.
 */
@Table("mapping")
public class MappingDMO {
    @Id
    private UUID id;
    private UUID      createdBy;
    private Timestamp createdOn;
    private UUID      versionedMappableId;
    private UUID mappingTypeId;
    private String input;
    private String output;
    private String documentation;
    private DistributionDMO distribution;

    @PersistenceConstructor
    MappingDMO(
            final UUID id,
            final UUID createdBy,
            final Timestamp createdOn,
            final UUID versionedMappableId,
            final UUID mappingTypeId,
            final String input,
            final String output,
            final String documentation,
            final DistributionDMO distribution) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.versionedMappableId = versionedMappableId;
        this.mappingTypeId = mappingTypeId;
        this.input = input;
        this.output = output;
        this.documentation = documentation;
        this.distribution = distribution;
    }

    public MappingDMO(
            final UUID createdBy,
            final UUID versionedMappableId,
            final UUID mappingTypeId,
            final String input,
            final String output,
            final String documentation,
            final DistributionDMO distribution) {
        this.id = null;
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.versionedMappableId = versionedMappableId;
        this.mappingTypeId = mappingTypeId;
        this.input = input;
        this.output = output;
        this.documentation = documentation;
        this.distribution = distribution;
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

    public UUID getVersionedMappableId() {
        return versionedMappableId;
    }

    public UUID getMappingTypeId() {
        return mappingTypeId;
    }

    public String getInput() {
        return input;
    }

    public String getOutput() {
        return output;
    }

    public String getDocumentation() {
        return documentation;
    }

    public DistributionDMO getDistribution() {
        return distribution;
    }
}
