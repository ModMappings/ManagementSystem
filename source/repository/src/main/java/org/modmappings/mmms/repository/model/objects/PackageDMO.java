package org.modmappings.mmms.repository.model.objects;

import java.util.List;
import java.util.UUID;

public class PackageDMO {

    private UUID mappingTypeId;
    private String packagePath;
    private List<UUID> members;

    public PackageDMO(final UUID mappingTypeId, final String packagePath, final List<UUID> members) {
        this.mappingTypeId = mappingTypeId;
        this.packagePath = packagePath;
        this.members = members;
    }

    public UUID getMappingTypeId() {
        return mappingTypeId;
    }

    public String getPackagePath() {
        return packagePath;
    }

    public List<UUID> getMembers() {
        return members;
    }
}
