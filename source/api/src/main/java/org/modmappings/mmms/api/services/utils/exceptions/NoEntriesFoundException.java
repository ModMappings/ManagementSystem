package org.modmappings.mmms.api.services.utils.exceptions;

public class NoEntriesFoundException extends AbstractHttpResponseException {

    public NoEntriesFoundException(final String entryTypeName) {
        super(404, String.format("Could not find any %s.", entryTypeName));
    }
}
