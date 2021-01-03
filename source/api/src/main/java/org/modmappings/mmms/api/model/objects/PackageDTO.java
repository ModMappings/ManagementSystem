package org.modmappings.mmms.api.model.objects;

import io.swagger.v3.oas.annotations.media.Schema;

import java.sql.Timestamp;
import java.util.UUID;

@Schema(name = "Package", description = "Represents a single package in one or multiple mappings.")
public class PackageDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The path of the package.")
    private String path;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The name of the package (usually the last part of the path)")
    private String name;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The path of the parent of the package.")
    private String parentPath;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The path of the parent of the parent of the package.")
    private String parentParentPath;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the package.")
    private UUID createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the package was created.")
    private Timestamp createdOn;

    public PackageDTO() {
    }

    public PackageDTO(final String path, final String name, final String parentPath, final String parentParentPath, final UUID createdBy, final Timestamp createdOn) {
        this.path = path;
        this.name = name;
        this.parentPath = parentPath;
        this.parentParentPath = parentParentPath;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
    }

    public String getPath() {
        return path;
    }

    public String getName() {
        return name;
    }

    public String getParentPath() {
        return parentPath;
    }

    public String getParentParentPath() {
        return parentParentPath;
    }

    public UUID getCreatedBy() {
        return createdBy;
    }

    public Timestamp getCreatedOn() {
        return createdOn;
    }
}
