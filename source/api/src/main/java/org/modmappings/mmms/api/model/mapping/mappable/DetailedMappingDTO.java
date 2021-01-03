package org.modmappings.mmms.api.model.mapping.mappable;

import io.swagger.v3.oas.annotations.media.Schema;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;


@Schema(name = "DetailedMapping", description = "Represents a single piece of the sourcecode whose name can be remapped in a given version of the game, including its metadata and core information.")
public class DetailedMappingDTO {

    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The mappable metadata related to the mapping.")
    private MappableDTO mappable;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The versioned mappable metadata related to the mapping. Does not contain sub- or supertype as well mapping locking information.")
    private SimpleVersionedMappableDTO versionedMappable;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The actuall mapping.")
    private MappingDTO mappingDTO;

    public DetailedMappingDTO() {
    }

    public DetailedMappingDTO(final MappableDTO mappable, final SimpleVersionedMappableDTO versionedMappable, final MappingDTO mappingDTO) {
        this.mappable = mappable;
        this.versionedMappable = versionedMappable;
        this.mappingDTO = mappingDTO;
    }

    public MappableDTO getMappable() {
        return mappable;
    }

    public SimpleVersionedMappableDTO getVersionedMappable() {
        return versionedMappable;
    }

    public MappingDTO getMappingDTO() {
        return mappingDTO;
    }
}
