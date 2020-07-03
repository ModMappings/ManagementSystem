package org.modmappings.mmms.api.model.mapping.mappable;

import io.swagger.v3.oas.annotations.media.Schema;

@Schema(name = "MappableType", description="Indicates which types (as in kind of pieces of the source code) can be remapped.", enumAsRef = true)
public enum MappableTypeDTO {
    @Schema(description = "Indicates that the mappable is a class.")
    CLASS,
    @Schema(description = "Indicates that the mappable is a method.")
    METHOD,
    @Schema(description = "Indicates that the mappable is a field.")
    FIELD,
    @Schema(description = "Indicates that the mappable is a parameter.")
    PARAMETER
}
