package org.modmappings.mmms.repository.model.mapping.mappable;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

/**
 * This represents a single mappable in a given version of the game.
 * This table contains also all metadata related to a given type of mappable.
 *
 * This means that not all fields will be populated with data, regardless of mappable type.
 * EG, a package has no visibility, so it will be populated with NOT_APPLICABLE, however it also has no type as such that field will be null.
 *
 * See the static factory methods in this class for the fields that are populated depending on what type of mappable this represents.
 */
@Table("versioned_mappable")
public class VersionedMappableDMO {

    public static VersionedMappableDMO newClass(
            final UUID createdBy,
            final UUID gameVersionId,
            final UUID mappableId,
            final VisibilityDMO visibility,
            final boolean isStatic
    )
    {
        return new VersionedMappableDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        null,
                        null,
                        null,
                        null
        );
    }

    public static VersionedMappableDMO newInnerClass(
            final UUID createdBy,
            final UUID gameVersionId,
            final UUID mappableId,
            final VisibilityDMO visibility,
            final boolean isStatic,
            final UUID parentClassId
    )
    {
        return new VersionedMappableDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        null,
                        parentClassId,
                        null,
                        null
        );
    }

    public static VersionedMappableDMO newMethod(
            final UUID createdBy,
            final UUID gameVersionId,
            final UUID mappableId,
            final VisibilityDMO visibility,
            final boolean isStatic,
            final UUID parentClassId,
            final String descriptor
    )
    {
        return new VersionedMappableDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        null,
                        parentClassId,
                        descriptor,
                        null
        );
    }

    public static VersionedMappableDMO newField(
            final UUID createdBy,
            final UUID gameVersionId,
            final UUID mappableId,
            final VisibilityDMO visibility,
            final boolean isStatic,
            final String type,
            final UUID parentClassId
    )
    {
        return new VersionedMappableDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        type,
                        parentClassId,
                        null,
                        null
        );
    }

    public static VersionedMappableDMO newParameter(
            final UUID createdBy,
            final UUID gameVersionId,
            final UUID mappableId,
            final VisibilityDMO visibility,
            final boolean isStatic,
            final String type,
            final UUID parentMethodId
    )
    {
        return new VersionedMappableDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        type,
                        null,
                        null,
                        parentMethodId
        );
    }

    @Id
    private UUID id;
    private UUID createdBy;
    private Timestamp createdOn;
    private UUID gameVersionId;
    private UUID mappableId;

    private UUID parentClassId;
    private UUID parentMethodId;

    private VisibilityDMO visibility;
    private boolean isStatic;
    private String type;
    private String descriptor;



    @PersistenceConstructor
    VersionedMappableDMO(
            final UUID id,
            final UUID createdBy,
            final Timestamp createdOn,
            final UUID gameVersionId,
            final UUID mappableId,
            final VisibilityDMO visibility,
            final boolean isStatic,
            final String type,
            final UUID parentClassId,
            final String descriptor,
            final UUID parentMethodId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.gameVersionId = gameVersionId;
        this.mappableId = mappableId;
        this.visibility = visibility;
        this.isStatic = isStatic;
        this.type = type;
        this.parentClassId = parentClassId;
        this.descriptor = descriptor;
        this.parentMethodId = parentMethodId;
    }

    VersionedMappableDMO(
            final UUID createdBy,
            final UUID gameVersionId,
            final UUID mappableId,
            final VisibilityDMO visibility,
            final boolean isStatic,
            final String type,
            final UUID parentClassId,
            final String descriptor,
            final UUID parentMethodId) {
        this.id = null;
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.gameVersionId = gameVersionId;
        this.mappableId = mappableId;
        this.visibility = visibility;
        this.isStatic = isStatic;
        this.type = type;
        this.parentClassId = parentClassId;
        this.descriptor = descriptor;
        this.parentMethodId = parentMethodId;
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

    public VisibilityDMO getVisibility() {
        return visibility;
    }

    public boolean isStatic() {
        return isStatic;
    }

    public String getType() {
        return type;
    }

    public UUID getParentClassId() {
        return parentClassId;
    }

    public String getDescriptor() {
        return descriptor;
    }

    public UUID getParentMethodId() {
        return parentMethodId;
    }
}
