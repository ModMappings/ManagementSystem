package org.modmappings.mmms.api.services.utils.user;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;

import java.util.UUID;
import java.util.function.Supplier;

/**
 * Handles logging on a user based level.
 */
@Component
public class UserLoggingService {

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
    public void info(Logger logger, Supplier<UUID> principalSupplier, String message) {
        logger.info(String.format("[%s]: %s", principalSupplier, message));
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
    public void warn(Logger logger, Supplier<UUID> principalSupplier, String message) {
        logger.warn(String.format("[%s]: %s", principalSupplier.get(), message));
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
    public void error(Logger logger, Supplier<UUID> principalSupplier, String message) {
        logger.error(String.format("[%s]: %s", principalSupplier.get(), message));
    }
}
