package org.modmappings.mmms.repository.model.comments;

/**
 * Represents all possible reactions on a comment that can be made by any user.
 * <p>
 * TODO: Figure out if we should expand this somehow.
 * TODO: Is there a nice list of emoji out there somewhere which we want to support?
 */
public enum CommentReactionTypeDMO {
    /**
     * :+1:
     */
    PLUS_ONE,

    /**
     * :-1:
     */
    MINUS_ONE,

    /**
     * :smile:
     */
    SMILE,

    /**
     * :confused:
     */
    CONFUSED,

    /**
     * :heart:
     */
    HEART,

    /**
     * :tada:
     */
    HOORAY,

    /**
     * :rocket:
     */
    ROCKET,

    /**
     * :eyes:
     */
    EYES
}
