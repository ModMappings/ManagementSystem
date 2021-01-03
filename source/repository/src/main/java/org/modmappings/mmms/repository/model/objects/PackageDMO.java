package org.modmappings.mmms.repository.model.objects;

import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

import java.sql.Timestamp;
import java.util.UUID;

@Table("packages")
public class PackageDMO {

    private final String path;
    private final String name;
    private final String parentPath;
    private final String parentParentPath;
    private final UUID createdBy;
    private final Timestamp createdOn;

    @PersistenceConstructor
    public PackageDMO(final String path, final String name, final String parentPath, final String parentParentPath, final UUID createdBy, final Timestamp createdOn) {
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
