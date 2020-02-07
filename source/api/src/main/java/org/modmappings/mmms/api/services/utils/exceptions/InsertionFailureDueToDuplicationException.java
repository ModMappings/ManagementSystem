package org.modmappings.mmms.api.services.utils.exceptions;

public class InsertionFailureDueToDuplicationException extends AbstractHttpResponseException {

    public InsertionFailureDueToDuplicationException(final String entryTypeName, final String fieldName) {
        super(400, "Could not insert: " + entryTypeName + " cause " + fieldName + " is already in use!");
    }
}
