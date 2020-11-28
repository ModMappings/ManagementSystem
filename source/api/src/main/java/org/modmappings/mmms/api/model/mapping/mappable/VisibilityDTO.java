package org.modmappings.mmms.api.model.mapping.mappable;

import io.swagger.v3.oas.annotations.media.Schema;

@Schema(name = "Visibility", description = "Indicates which visibility a given versioned mappable has (if applicable).", enumAsRef = true)
public enum VisibilityDTO {
    @Schema(description = "Indicates that the versioned mappable has a public accessor.")
    PUBLIC,
    @Schema(description = "Indicates that the versioned mappable has a package private accessor.")
    PACKAGE,
    @Schema(description = "Indicates that the versioned mappable has a protected accessor.")
    PROTECTED,
    @Schema(description = "Indicates that the versioned mappable has a private accessor.")
    PRIVATE,
    @Schema(description = "Indicates that visibility is not something that is applicable to the versioned mappable.")
    NOT_APPLICABLE,
    @Schema(description = "Indicates that the versioned mappable has am unknown accessor. This might be updated when more data is imported from the source code of the game.")
    UNKNOWN
}
