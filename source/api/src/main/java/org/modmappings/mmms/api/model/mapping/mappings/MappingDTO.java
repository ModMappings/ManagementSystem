package org.modmappings.mmms.api.model.mapping.mappings;

import io.swagger.v3.oas.annotations.media.Schema;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;

import java.sql.Timestamp;
import java.util.UUID;

@Schema(name = "Mapping", description = "Represents a single mapping.")
public class MappingDTO {
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mapping.")
    private UUID id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user or system who created the mapping.")
    private UUID createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment this mapping was created.")
    private Timestamp createdOn;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the versioned mappable this mapping exists for.")
    private UUID versionedMappableId;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mapping type this mapping exists for.")
    private UUID mappingTypeId;
    @Schema(description = "The input of the mapping.")
    private String input;
    @Schema(description = "The output of the mapping.")
    private String output;
    @Schema(description = "The documentation that accompanies the mapping. (EG: JavaDoc for a class, method or parameter.)")
    private String documentation;
    @Schema(description = "The distribution that the versioned mappable that is targeted by this mapping is in.")
    private DistributionDTO distribution;
    @Schema(description = "The package the mapping for the class is in. Might be null when this is not a mapping for a class")
    private String packagePath;
    @Schema(description = "The parent package of the package the mapping for the class is in. Might be null when this is not a mapping for a class")
    private String parentPackagePath;
    @Schema(description = "The id of the game version this mapping is for.")
    private UUID gameVersionId;
    @Schema(description = "The type of mappable this is a mapping for.")
    private MappableTypeDTO mappableType;
    @Schema(description = "The id of the mappable this is a mapping for.")
    private UUID mappableId;

    public MappingDTO(final UUID id,
                      final UUID createdBy,
                      final Timestamp createdOn,
                      final UUID versionedMappableId,
                      final UUID mappingTypeId,
                      final String input,
                      final String output,
                      final String documentation,
                      final DistributionDTO distribution,
                      final String packagePath,
                      final String parentPackagePath,
                      final UUID gameVersionId,
                      final MappableTypeDTO mappableType,
                      final UUID mappableId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.versionedMappableId = versionedMappableId;
        this.mappingTypeId = mappingTypeId;
        this.input = input;
        this.output = output;
        this.documentation = documentation;
        this.distribution = distribution;
        this.packagePath = packagePath;
        this.parentPackagePath = parentPackagePath;
        this.gameVersionId = gameVersionId;
        this.mappableType = mappableType;
        this.mappableId = mappableId;
    }

    public MappingDTO() {
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

    public DistributionDTO getDistribution() {
        return distribution;
    }

    public String getPackagePath() {
        return packagePath;
    }

    public String getParentPackagePath() {
        return parentPackagePath;
    }

    public UUID getGameVersionId() {
        return gameVersionId;
    }

    public MappableTypeDTO getMappableType() {
        return mappableType;
    }

    public UUID getMappableId() {
        return mappableId;
    }
}
