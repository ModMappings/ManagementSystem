package org.modmappings.mmms.api.services.utils.user;

import java.util.UUID;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.oauth2.core.OAuth2AuthenticatedPrincipal;
import org.springframework.stereotype.Component;

/**
 * This utility class allows for easy access to and logging in the name of the current user.
 * It pulls the current principle and checks if this is the expected OAuth2 variant, after which it pulls the
 * subject id (sub) from the JWT.
 */
@Component
public class UserService {

    private final Logger logger = LoggerFactory.getLogger(UserService.class);

    /**
     * Create a new log entry in the given logger,
     * prefixed with the id of the current logged in user.
     *
     * The log level is information.
     *
     * @param logger The logger that logs the message.
     * @param message The message to log in the name of the current user.
     */
    public void info(Logger logger, String message) {
        logger.info(String.format("[%s]: %s", getCurrentUserId(), message));
    }

    /**
     * Create a new log entry in the given logger,
     * prefixed with the id of the current logged in user.
     *
     * The log level is warning.
     *
     * @param logger The logger that logs the message.
     * @param message The message to log in the name of the current user.
     */
    public void warn(Logger logger, String message) {
        logger.warn(String.format("[%s]: %s", getCurrentUserId(), message));
    }

    /**
     * Create a new log entry in the given logger,
     * prefixed with the id of the current logged in user.
     *
     * The log level is error.
     *
     * @param logger The logger that logs the message.
     * @param message The message to log in the name of the current user.
     */
    public void error(Logger logger, String message) {
        logger.error(String.format("[%s]: %s", getCurrentUserId(), message));
    }

    /**
     * Looks up the current id of the user.
     * Validates the current principle and then attempts to parse the UUID of the current user from the
     * subject id contained in the JWT.
     *
     * @return The uuid that represents the id of the JWT.
     * @throws IllegalStateException When their is currently no authenticated user or when the subject id is not in valid uuid form.
     */
    public UUID getCurrentUserId() {
        final Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (!(authentication instanceof OAuth2AuthenticatedPrincipal))
        {
            logger.error(String.format("Attempted to request user id, when no user is present! Current principle: %s", authentication), new IllegalStateException());
            throw new IllegalStateException(String.format("There is currently no user logged in. The current principle is: %s", authentication));
        }

        final OAuth2AuthenticatedPrincipal principal = (OAuth2AuthenticatedPrincipal) authentication;
        final String idString = principal.getAttribute("sub");

        try {
            return UUID.fromString(idString);
        } catch (IllegalArgumentException ex) {
            logger.error(String.format("Failed to parse ID from OAuth2 Subject. Token subject: %s", idString), ex);
            throw new IllegalStateException(String.format("The id of the subject contained in the OAuth2 JWT is not a UUID: %s", idString), ex);
        }
    }
}
