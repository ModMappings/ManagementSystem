package org.modmappings.mmms.api.model.mapping.mappable;

import io.swagger.v3.oas.annotations.media.Schema;

import java.sql.Timestamp;
import java.util.UUID;

@Schema(name = "SimpleVersionedMappable", description = "Represents a single piece of the sourcecode whose name can be remapped in a given version of the game, without the super- and subtype information, as well as the mapping locking information.")
public class SimpleVersionedMappableDTO {
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the versioned mappable.")
    protected UUID id;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The id of the user who created the versioned mappable.")
    protected UUID createdBy;
    @Schema(accessMode = Schema.AccessMode.READ_ONLY, description = "The moment the versioned mappable was created.")
    protected Timestamp createdOn;
    @Schema(description = "The id of the game versioned in which this versioned mappable exists.")
    protected UUID gameVersionId;
    @Schema(description = "The id of the mappable which is represented by this versioned mappable in the game version.")
    protected UUID mappableId;
    @Schema(description = "The id of the class this versioned mappable resides in. Might be null if the mappable this versioned mappable represents is not representing a method or field.")
    protected UUID parentClassId;
    @Schema(description = "The id of the method this versioned mappable resides in. Might be null if the mappable this versioned mappable represents is not representing a parameter.")
    protected UUID parentMethodId;
    @Schema(description = "The visibility of the versioned mappable. If the visibility is not applicable to the type of mappable that this versioned mappable represents then NOT_APPLICABLE will be the value of this property.")
    protected VisibilityDTO visibility;
    @Schema(description = "Indicates if this versioned mappable is static. If this versioned mappable represents a mappable of which the type can not be static this will be always false.")
    protected boolean isStatic;
    @Schema(description = "Indicates the type the field. This type is in an obfuscated form and any client will need to parse the type and convert it itself into human readable language, by requesting it in the mapping type it wants to display. This field will contain an empty string if the type of mappable that this versioned mappable represents is not a field or parameter.")
    protected String type;
    @Schema(description = "The descriptor that describes (ha) this versioned mappable. As with the type this descriptor is a raw obfuscated data entry. If the client wants to display this to a human in readable form he will need to parse this descriptor himself and request the human readable form of the mapping in the mapping type he wishes to display. This field will contain an empty string when the type of mappable that this versioned mappable represents is not a method.")
    protected String descriptor;
    @Schema(description = "The signature that describes this versioned mappable. Includes the generics of the parameter and returned type, if applicable. As with the descriptor this field contains raw obfuscated data. If the client wants to display this to a human in readable form he will need to parse this signature himself and request the human readable form of the mapping in the mapping type he wishes to display. This field will contain an empty string when the type of the mappable that this versioned mappable represents is not a method.")
    protected String signature;
    @Schema(description = "The signature that describes this versioned mappable. Includes the generics of the parameter and returned type, if applicable. As with the descriptor this field contains raw obfuscated data. If the client wants to display this to a human in readable form he will need to parse this signature himself and request the human readable form of the mapping in the mapping type he wishes to display. This field will contain an empty string when the type of the mappable that this versioned mappable represents is not a method.")
    protected boolean isExternal;
    @Schema(description = "This indicates the parameter number for a parameter. The numbers are JVM parameter indices, for a lack of a better word, and describe both the position, as well as the width of the parameter. If the method is not static then the parameter with 0 is implicitly the keyword this, and counting starts at 1. If the method is 0 then counting starts at 0. In general all parameters are 1 wide. However doubles and longs are 2. If this versioned mappable does not represent a parameter then this is null or some random value.")
    protected Integer index;

    public SimpleVersionedMappableDTO(final UUID id, final UUID createdBy, final Timestamp createdOn, final UUID gameVersionId, final UUID mappableId, final UUID parentClassId, final UUID parentMethodId, final VisibilityDTO visibility, final boolean isStatic, final String type, final String descriptor, final String signature, final boolean isExternal, final Integer index) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.gameVersionId = gameVersionId;
        this.mappableId = mappableId;
        this.parentClassId = parentClassId;
        this.parentMethodId = parentMethodId;
        this.visibility = visibility;
        this.isStatic = isStatic;
        this.type = type;
        this.descriptor = descriptor;
        this.signature = signature;
        this.isExternal = isExternal;
        this.index = index;
    }

    public SimpleVersionedMappableDTO() {
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

    public UUID getGameVersionId() {
        return gameVersionId;
    }

    public UUID getMappableId() {
        return mappableId;
    }

    public UUID getParentClassId() {
        return parentClassId;
    }

    public UUID getParentMethodId() {
        return parentMethodId;
    }

    public VisibilityDTO getVisibility() {
        return visibility;
    }

    public String getType() {
        return type;
    }

    public String getDescriptor() {
        return descriptor;
    }

    public String getSignature() {
        return signature;
    }

    public boolean isStatic() {
        return isStatic;
    }

    public boolean isExternal() {
        return isExternal;
    }

    public Integer getIndex() {
        return index;
    }
}
