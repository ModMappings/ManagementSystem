package org.modmappings.mmms.api.services.utils.exceptions;

/**
 * This is an exception thrown in the business layer that can
 * later be turned into an http response code and a body message if needed.
 */
public class AbstractHttpResponseException extends Throwable {

    private final int responseCode;
    private final String bodyMessage;

    public AbstractHttpResponseException(final int responseCode, final String bodyMessage) {
        super(bodyMessage);

        this.responseCode = responseCode;
        this.bodyMessage = bodyMessage;
    }

    public int getResponseCode() {
        return responseCode;
    }

    public String getBodyMessage() {
        return bodyMessage;
    }
}
