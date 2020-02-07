package org.modmappings.mmms.api.services.utils.user;

import org.springframework.security.oauth2.server.resource.authentication.JwtAuthenticationToken;
import org.springframework.stereotype.Component;

import java.security.Principal;
import java.util.UUID;

/**
 * This utility class allows for easy access to and logging in the name of the current user.
 * It pulls the current principle and checks if this is the expected OAuth2 variant, after which it pulls the
 * subject id (sub) from the JWT.
 */
@Component
public class UserService {



    /**
     * Looks up the current id of the user.
     * Validates the current principle and then attempts to parse the UUID of the current user from the
     * subject id contained in the JWT.
     *
     * @param principal The principal information to pull the id from.
     * @return The uuid that represents the id of the JWT.
     * @throws IllegalStateException When their is currently no authenticated user or when the subject id is not in valid uuid form.
     */
    public UUID getCurrentUserId(final Principal principal) {
        if (principal == null)
            throw new IllegalStateException("No principal provided.");

        if (!(principal instanceof JwtAuthenticationToken))
            throw new IllegalStateException("Principal not performed via JWT.");

        final JwtAuthenticationToken twtPrincipal = (JwtAuthenticationToken) principal;
        final String idString = twtPrincipal.getName();
        if (idString == null)
            throw new IllegalStateException("OAuth2 JWT does not contain subject.");

        try {
            return UUID.fromString(idString);
        }
        catch (final IllegalArgumentException ex)
        {
            throw new IllegalStateException("Subject id does not contain UUID", ex);
        }
    }
}
