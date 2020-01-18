package org.modmappings.mmms.api.services.utils.exceptions;

import java.util.UUID;

public class EntryNotFoundException extends AbstractHttpResponseException {

    public EntryNotFoundException(UUID entryId, String entryTypeName) {
        super(404, String.format("Could not find: %s with id: %s", entryTypeName, entryId));
    }
}
