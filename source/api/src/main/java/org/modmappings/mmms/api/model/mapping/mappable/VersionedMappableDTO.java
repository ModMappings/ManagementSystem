package org.modmappings.mmms.api.model.mapping.mappable;

import io.swagger.v3.oas.annotations.media.Schema;

import java.sql.Timestamp;
import java.util.List;
import java.util.UUID;

/**
 * This represents a single mappable in a given version of the game.
 * This table contains also all metadata related to a given type of mappable.
 * <p>
 * This means that not all fields will be populated with data, regardless of mappable type.
 * EG, a package has no visibility, so it will be populated with NOT_APPLICABLE, however it also has no type as such that field will be null.
 * <p>
 * See the static factory methods in this class for the fields that are populated depending on what type of mappable this represents.
 */
@Schema(name = "VersionedMappable", description = "Represents a single piece of the sourcecode whose name can be remapped in a given version of the game.")
public class VersionedMappableDTO extends SimpleVersionedMappableDTO {

    @Schema(description = "A list of all mapping types for which no changes can be made via proposals. Only changes can be made via directly committing a mapping.")
    private List<UUID> lockedIn;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "A list of ids of versioned mappables that represent the super types of the versioned mappable if this represents a class. If this is not a class, then this field will be null. If this is a class and the field is empty then no super types are known.")
    private List<UUID> superTypes;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "A list of ids of versioned mappables that represent the sub types of the versioned mappables if this represents a class. If this is not a class, then this field will be null. If this is a class and the field is empty then no sub types are known.")
    private List<UUID> subTypes;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "A list of ids of versioned mappables that represent the methods overriden by this method. If this versioned mappable represents a method. If this is not a method, this field will be null. If this is a method and the field is empty then no overriden methods are known.")
    private List<UUID> overrides;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "A list of ids of versioned mappables that represent the methods that override this method. If this versioned mappable represents a method. If this is not a method, this field will be null. If this is a method and the field is empty then no methods that override this method could be found.")
    private List<UUID> overridenBy;

    public VersionedMappableDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, final UUID gameVersionId, final UUID mappableId, final UUID parentClassId, final UUID parentMethodId, final VisibilityDTO visibility, final boolean isStatic, final String type, final String descriptor, final String signature, final boolean isExternal, final Integer index, final List<UUID> lockedIn, final List<UUID> superTypes, final List<UUID> subTypes, final List<UUID> overrides, final List<UUID> overridenBy) {
        super(id, createdBy, createdOn, gameVersionId, mappableId, parentClassId, parentMethodId, visibility, isStatic, type, descriptor, signature, isExternal, index);
        this.lockedIn = lockedIn;
        this.superTypes = superTypes;
        this.subTypes = subTypes;
        this.overrides = overrides;
        this.overridenBy = overridenBy;
    }

    public VersionedMappableDTO() {
    }

    public List<UUID> getLockedIn() {
        return lockedIn;
    }

    public List<UUID> getSuperTypes() {
        return superTypes;
    }

    public List<UUID> getSubTypes() {
        return subTypes;
    }

    public List<UUID> getOverrides() {
        return overrides;
    }

    public List<UUID> getOverridenBy() {
        return overridenBy;
    }
}
