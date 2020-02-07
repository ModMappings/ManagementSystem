package org.modmappings.mmms.api.model.core.release;

import io.swagger.v3.oas.annotations.media.Schema;

import javax.validation.constraints.NotBlank;
import javax.validation.constraints.NotNull;
import java.sql.Timestamp;
import java.util.Set;
import java.util.UUID;

@Schema(name = "Release")
public class ReleaseDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the release.")
    private UUID id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the release.")
    private UUID createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the release was created.")
    private Timestamp createdOn;

    @Schema(description = "The name of the release, has to be unique inside a given mapping type.", minLength = 1, required = true)
    @NotBlank
    private String name;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the game version for which this release is.", required = true, example = "9b4a9c76-3588-48b5-bedf-b0df90b00381")
    private UUID gameVersionId;
    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the mapping type for which this release is.", required = true, example = "9b4a9c76-3588-48b5-bedf-b0df90b00381")
    private UUID mappingType;

    @Schema(description = "Indicates if this release is a snapshot or not. Snapshot release are potentially not stable.", required = true)
    private boolean isSnapshot;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The ids of the mappings which remap packages that are part of this release")
    private Set<UUID> packageMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The ids of the mappings which remap classes that are part of this release")
    private Set<UUID> classMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The ids of the mappings which remap methods that are part of this release")
    private Set<UUID> methodMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The ids of the mappings which remap fields that are part of this release")
    private Set<UUID> fieldMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The ids of the mappings which remap parameters that are part of this release")
    private Set<UUID> parameterMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The ids of the comments which were made on this release.")
    private Set<UUID> commentsMappings;

    public ReleaseDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, @NotBlank final String name, @NotNull final UUID gameVersionId, @NotNull final UUID mappingType, final boolean isSnapshot, @NotNull final Set<UUID> packageMappings, @NotNull final Set<UUID> classMappings, @NotNull final Set<UUID> methodMappings, @NotNull final Set<UUID> fieldMappings, @NotNull final Set<UUID> parameterMappings, @NotNull final Set<UUID> commentsMappings) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.name = name;
        this.gameVersionId = gameVersionId;
        this.mappingType = mappingType;
        this.isSnapshot = isSnapshot;
        this.packageMappings = packageMappings;
        this.classMappings = classMappings;
        this.methodMappings = methodMappings;
        this.fieldMappings = fieldMappings;
        this.parameterMappings = parameterMappings;
        this.commentsMappings = commentsMappings;
    }

    public ReleaseDTO() {
    }

    public UUID getId() {
        return id;
    }

    public void setId(final UUID id) {
        this.id = id;
    }

    public UUID getCreatedBy() {
        return createdBy;
    }

    public void setCreatedBy(final UUID createdBy) {
        this.createdBy = createdBy;
    }

    public Timestamp getCreatedOn() {
        return createdOn;
    }

    public void setCreatedOn(final Timestamp createdOn) {
        this.createdOn = createdOn;
    }

    public String getName() {
        return name;
    }

    public void setName(final String name) {
        this.name = name;
    }

    public UUID getGameVersionId() {
        return gameVersionId;
    }

    public void setGameVersionId(final UUID gameVersionId) {
        this.gameVersionId = gameVersionId;
    }

    public UUID getMappingType() {
        return mappingType;
    }

    public void setMappingType(final UUID mappingType) {
        this.mappingType = mappingType;
    }

    public boolean isSnapshot() {
        return isSnapshot;
    }

    public void setSnapshot(final boolean snapshot) {
        isSnapshot = snapshot;
    }

    public Set<UUID> getPackageMappings() {
        return packageMappings;
    }

    public void setPackageMappings(final Set<UUID> packageMappings) {
        this.packageMappings = packageMappings;
    }

    public Set<UUID> getClassMappings() {
        return classMappings;
    }

    public void setClassMappings(final Set<UUID> classMappings) {
        this.classMappings = classMappings;
    }

    public Set<UUID> getMethodMappings() {
        return methodMappings;
    }

    public void setMethodMappings(final Set<UUID> methodMappings) {
        this.methodMappings = methodMappings;
    }

    public Set<UUID> getFieldMappings() {
        return fieldMappings;
    }

    public void setFieldMappings(final Set<UUID> fieldMappings) {
        this.fieldMappings = fieldMappings;
    }

    public Set<UUID> getParameterMappings() {
        return parameterMappings;
    }

    public void setParameterMappings(final Set<UUID> parameterMappings) {
        this.parameterMappings = parameterMappings;
    }

    public Set<UUID> getCommentsMappings() {
        return commentsMappings;
    }

    public void setCommentsMappings(final Set<UUID> commentsMappings) {
        this.commentsMappings = commentsMappings;
    }
}
