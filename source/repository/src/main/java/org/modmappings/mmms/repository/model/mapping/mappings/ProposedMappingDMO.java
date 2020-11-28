package org.modmappings.mmms.repository.model.mapping.mappings;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

@Table("proposed_mapping")
public class ProposedMappingDMO {

    @Id
    private UUID id;
    private UUID createdBy;
    private Timestamp createdOn;
    private UUID versionedMappableId;
    private UUID mappingTypeId;
    private String input;
    private String output;
    private String documentation;
    private DistributionDMO distribution;
    private boolean isPublic;
    private UUID closedBy;
    private Timestamp closedOn;
    private UUID mappingId;

    @PersistenceConstructor
    ProposedMappingDMO(
            final UUID id,
            final UUID createdBy,
            final Timestamp createdOn,
            final UUID versionedMappableId,
            final UUID mappingTypeId,
            final String input,
            final String output,
            final String documentation,
            final DistributionDMO distribution,
            final boolean isPublic,
            final UUID closedBy,
            final Timestamp closedOn,
            final UUID mappingId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.versionedMappableId = versionedMappableId;
        this.mappingTypeId = mappingTypeId;
        this.input = input;
        this.output = output;
        this.documentation = documentation;
        this.distribution = distribution;
        this.isPublic = isPublic;
        this.closedBy = closedBy;
        this.closedOn = closedOn;
        this.mappingId = mappingId;
    }

    public ProposedMappingDMO(
            final UUID createdBy,
            final UUID versionedMappableId,
            final UUID mappingTypeId,
            final String input,
            final String output,
            final String documentation,
            final DistributionDMO distribution, final boolean isPublic) {
        this.id = null;
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.versionedMappableId = versionedMappableId;
        this.mappingTypeId = mappingTypeId;
        this.input = input;
        this.output = output;
        this.documentation = documentation;
        this.distribution = distribution;
        this.isPublic = isPublic;
        this.closedBy = null;
        this.closedOn = null;
        this.mappingId = null;
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

    public boolean isPublic() {
        return isPublic;
    }

    public UUID getClosedBy() {
        return closedBy;
    }

    public Timestamp getClosedOn() {
        return closedOn;
    }

    public UUID getMappingId() {
        return mappingId;
    }

    public boolean isOpen() {
        return closedBy == null && closedOn == null && mappingId == null;
    }

    public boolean isMerged() {
        return closedBy != null && closedOn != null && mappingId != null;
    }

    /**
     * Creates a proposed mapping that is identical to this one in the closed none merged state.
     *
     * @param closedBy The user who closed the proposal.
     * @return This proposed mapping in a closed none merged state.
     */
    public ProposedMappingDMO close(final UUID closedBy) {
        return new ProposedMappingDMO(
                id,
                createdBy,
                createdOn,
                versionedMappableId,
                mappingTypeId,
                input,
                output,
                documentation,
                distribution,
                isPublic,
                closedBy,
                Timestamp.from(Instant.now()),
                null
        );
    }


    /**
     * Creates a proposed mapping that is identical to this one in the closed merged state.
     *
     * @param closedBy The user who closed the proposal.
     * @return This proposed mapping in a closed merged state.
     */
    public ProposedMappingDMO merge(final UUID closedBy, final UUID mappingId) {
        return new ProposedMappingDMO(
                id,
                createdBy,
                createdOn,
                versionedMappableId,
                mappingTypeId,
                input,
                output,
                documentation,
                distribution,
                isPublic,
                closedBy,
                Timestamp.from(Instant.now()),
                mappingId
        );
    }
}
