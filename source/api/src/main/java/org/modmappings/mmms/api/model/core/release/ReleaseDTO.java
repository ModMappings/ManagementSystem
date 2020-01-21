package org.modmappings.mmms.api.model.core.release;

import io.swagger.v3.oas.annotations.media.Schema;

import javax.validation.constraints.NotBlank;
import javax.validation.constraints.NotNull;
import java.sql.Timestamp;
import java.util.Set;
import java.util.UUID;

@Schema(name = "Release")
public class ReleaseDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private UUID id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private UUID createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Timestamp createdOn;

    @NotBlank
    private String name;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private UUID gameVersionId;
    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private UUID mappingType;

    private boolean isSnapshot;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Set<UUID> packageMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Set<UUID> classMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Set<UUID> methodMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Set<UUID> fieldMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Set<UUID> parameterMappings;

    @NotNull
    @Schema(accessMode = Schema.AccessMode.READ_ONLY)
    private Set<UUID> commentsMappings;

    public ReleaseDTO(UUID id, UUID createdBy, Timestamp createdOn, @NotBlank String name, @NotNull UUID gameVersionId, @NotNull UUID mappingType, boolean isSnapshot, @NotNull Set<UUID> packageMappings, @NotNull Set<UUID> classMappings, @NotNull Set<UUID> methodMappings, @NotNull Set<UUID> fieldMappings, @NotNull Set<UUID> parameterMappings, @NotNull Set<UUID> commentsMappings) {
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

    public void setId(UUID id) {
        this.id = id;
    }

    public UUID getCreatedBy() {
        return createdBy;
    }

    public void setCreatedBy(UUID createdBy) {
        this.createdBy = createdBy;
    }

    public Timestamp getCreatedOn() {
        return createdOn;
    }

    public void setCreatedOn(Timestamp createdOn) {
        this.createdOn = createdOn;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public UUID getGameVersionId() {
        return gameVersionId;
    }

    public void setGameVersionId(UUID gameVersionId) {
        this.gameVersionId = gameVersionId;
    }

    public UUID getMappingType() {
        return mappingType;
    }

    public void setMappingType(UUID mappingType) {
        this.mappingType = mappingType;
    }

    public boolean isSnapshot() {
        return isSnapshot;
    }

    public void setSnapshot(boolean snapshot) {
        isSnapshot = snapshot;
    }

    public Set<UUID> getPackageMappings() {
        return packageMappings;
    }

    public void setPackageMappings(Set<UUID> packageMappings) {
        this.packageMappings = packageMappings;
    }

    public Set<UUID> getClassMappings() {
        return classMappings;
    }

    public void setClassMappings(Set<UUID> classMappings) {
        this.classMappings = classMappings;
    }

    public Set<UUID> getMethodMappings() {
        return methodMappings;
    }

    public void setMethodMappings(Set<UUID> methodMappings) {
        this.methodMappings = methodMappings;
    }

    public Set<UUID> getFieldMappings() {
        return fieldMappings;
    }

    public void setFieldMappings(Set<UUID> fieldMappings) {
        this.fieldMappings = fieldMappings;
    }

    public Set<UUID> getParameterMappings() {
        return parameterMappings;
    }

    public void setParameterMappings(Set<UUID> parameterMappings) {
        this.parameterMappings = parameterMappings;
    }

    public Set<UUID> getCommentsMappings() {
        return commentsMappings;
    }

    public void setCommentsMappings(Set<UUID> commentsMappings) {
        this.commentsMappings = commentsMappings;
    }
}
