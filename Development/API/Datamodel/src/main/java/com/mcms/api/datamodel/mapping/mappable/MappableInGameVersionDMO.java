package com.mcms.api.datamodel.mapping.mappable;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

@Table("versioned_mappable")
public class MappableInGameVersionDMO {

    public static MappableInGameVersionDMO newRootPackage(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId
    )
    {
        return new MappableInGameVersionDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        VisibilityDMO.NOT_APPLICABLE,
                        false,
                        null,
                        null,
                        null,
                        null,
                        null
        );
    }

    public static MappableInGameVersionDMO newPackage(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final UUID parentPackageId
    )
    {
        return new MappableInGameVersionDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        VisibilityDMO.NOT_APPLICABLE,
                        false,
                        null,
                        parentPackageId,
                        null,
                        null,
                        null
        );
    }

    public static MappableInGameVersionDMO newClass(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final VisibilityDMO visibility,
                    final boolean isStatic,
                    final UUID parentPackageId
    )
    {
        return new MappableInGameVersionDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        null,
                        parentPackageId,
                        null,
                        null,
                        null
        );
    }

    public static MappableInGameVersionDMO newInnerClass(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final VisibilityDMO visibility,
                    final boolean isStatic,
                    final UUID parentPackageId,
                    final UUID partOfClassId
    )
    {
        return new MappableInGameVersionDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        null,
                        parentPackageId,
                        partOfClassId,
                        null,
                        null
        );
    }

    public static MappableInGameVersionDMO newMethod(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final VisibilityDMO visibility,
                    final boolean isStatic,
                    final UUID partOfClassId,
                    final String descriptor
    )
    {
        return new MappableInGameVersionDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        null,
                        null,
                        partOfClassId,
                        descriptor,
                        null
        );
    }

    public static MappableInGameVersionDMO newField(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final VisibilityDMO visibility,
                    final boolean isStatic,
                    final String type,
                    final UUID partOfClassId
    )
    {
        return new MappableInGameVersionDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        type,
                        null,
                        partOfClassId,
                        null,
                        null
        );
    }

    public static MappableInGameVersionDMO newParameter(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final VisibilityDMO visibility,
                    final boolean isStatic,
                    final String type,
                    final UUID partOfMethodId
    )
    {
        return new MappableInGameVersionDMO(
                        createdBy,
                        gameVersionId,
                        mappableId,
                        visibility,
                        isStatic,
                        type,
                        null,
                        null,
                        null,
                        partOfMethodId
        );
    }

    @Id
    private final UUID id;
    private final UUID createdBy;
    private final Timestamp createdOn;
    private final UUID gameVersionId;
    private final UUID mappableId;

    private final VisibilityDMO visibility;
    private final boolean isStatic;
    private final String type;

    private final UUID parentPackageId;

    private final UUID partOfClassId;

    private final String descriptor;

    private final UUID partOfMethodId;

    @PersistenceConstructor
    MappableInGameVersionDMO(
                    final UUID id,
                    final UUID createdBy,
                    final Timestamp createdOn,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final VisibilityDMO visibility,
                    final boolean isStatic,
                    final String type,
                    final UUID parentPackageId,
                    final UUID partOfClassId,
                    final String descriptor, final UUID partOfMethodId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.gameVersionId = gameVersionId;
        this.mappableId = mappableId;
        this.visibility = visibility;
        this.isStatic = isStatic;
        this.type = type;
        this.parentPackageId = parentPackageId;
        this.partOfClassId = partOfClassId;
        this.descriptor = descriptor;
        this.partOfMethodId = partOfMethodId;
    }

    MappableInGameVersionDMO(
                    final UUID createdBy,
                    final UUID gameVersionId,
                    final UUID mappableId,
                    final VisibilityDMO visibility,
                    final boolean isStatic,
                    final String type,
                    final UUID parentPackageId,
                    final UUID partOfClassId,
                    final String descriptor,
                    final UUID partOfMethodId) {
        this.id = UUID.randomUUID();
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.gameVersionId = gameVersionId;
        this.mappableId = mappableId;
        this.visibility = visibility;
        this.isStatic = isStatic;
        this.type = type;
        this.parentPackageId = parentPackageId;
        this.partOfClassId = partOfClassId;
        this.descriptor = descriptor;
        this.partOfMethodId = partOfMethodId;
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

    public UUID getParentPackageId() {
        return parentPackageId;
    }

    public UUID getPartOfClassId() {
        return partOfClassId;
    }

    public String getDescriptor() {
        return descriptor;
    }

    public UUID getPartOfMethodId() {
        return partOfMethodId;
    }
}
