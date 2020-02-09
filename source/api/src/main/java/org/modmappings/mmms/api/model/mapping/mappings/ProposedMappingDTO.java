package org.modmappings.mmms.api.model.mapping.mappings;

import io.swagger.v3.oas.annotations.media.Schema;
import org.modmappings.mmms.repository.model.mapping.mappings.DistributionDMO;
import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

@Schema(name = "ProposedMapping", description = "Represents a proposal to change a mapping.")
public class ProposedMappingDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the proposed mapping.")
    private UUID            id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the mapping.")
    private UUID            createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment a proposal was created.")
    private Timestamp       createdOn;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the versioned mappable this proposal was created for.")
    private UUID            versionedMappableId;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mapping type this proposal was created for.")
    private UUID mappingTypeId;
    @Schema(description = "The input of the proposal. Might be equal to the input of the current mapping if no change is required.")
    private String input;
    @Schema(description = "The output of the proposal. Might be equal to the output of the current mapping if no change is required.")
    private String output;
    @Schema(description = "The documentation of the proposal. Might be equal to the documentation of the current mapping if no change is required.")
    private String          documentation;
    @Schema(description = "The distribution of the proposal. Might be equal to the distribution of the current mapping if no change is required.")
    private DistributionDMO distribution;
    @Schema(description = "Indicates if this proposal is public. None public proposals can only be seen by users or systems who have the role PROPOSALS_NONE_PUBLIC.")
    private boolean isPublic;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The user who closed this proposal. This is null if the proposal is still open.")
    private UUID closedBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the proposal was closed. This is null if the proposal is still open.")
    private Timestamp closedOn;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mapping, which was created of this proposal when it was closed. This is null if the proposal is still open or has been rejected.")
    private UUID mappingId;

    public ProposedMappingDTO(final UUID id,
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

    public ProposedMappingDTO() {
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

    public boolean getIsPublic() {
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
}
