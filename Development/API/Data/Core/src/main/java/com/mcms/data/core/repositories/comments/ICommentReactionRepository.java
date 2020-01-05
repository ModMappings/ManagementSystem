package com.mcms.data.core.repositories.comments;

import java.util.UUID;

import com.mcms.api.datamodel.comments.CommentReactionDMO;
import com.mcms.data.core.repositories.IRepository;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;

/**
 * Represents a repository that can provide and store {@link CommentReactionDMO} objects.
 */
public interface ICommentReactionRepository extends IRepository<CommentReactionDMO> {

    /**
     * Finds all comment reactions which where directly made on the comment reaction with the given id.
     *
     * The comment reactions are sorted oldest to newest.
     *
     * @param commentReactionId The id of the comment reaction to look up comment reactions for.
     * @return The comment reactions on the given comment reaction.
     * @throws IllegalArgumentException in case the given {@literal commentReactionId} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForComment(final UUID commentReactionId);

    /**
     * Finds all comment reactions which where directly made on the comment reactions with the given ids.
     *
     * The comment reactions are sorted oldest to newest.
     *
     * @param commentReactionIds The ids of the comment reactions to look up comment reactions for.
     * @return The comment reactions on the given comment reactions.
     * @throws IllegalArgumentException in case the given {@literal commentReactionIds} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForComments(final Iterable<UUID> commentReactionIds);

    /**
     * Finds all comment reactions which where directly made on the comment reactions with the given ids.
     *
     * The comment reactions are sorted oldest to newest.
     *
     * @param commentReactionIds The ids of the comment reactions to look up comment reactions for.
     * @return The comment reactions on the given comment reactions.
     * @throws IllegalArgumentException in case the given {@literal commentReactionIds} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForComments(final Publisher<UUID> commentReactionIds);

    /**
     * Finds all comment reactions which where directly made by the user with the given id.
     *
     * The comment reactions are sorted newest to oldest.
     *
     * @param userId The id of the user to look up comment reactions for.
     * @return The comment reactions by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForUser(final UUID userId);

    /**
     * Finds all comment reactions which where directly made by the users with the given ids.
     *
     * The comment reactions are sorted newest to oldest.
     *
     * @param userIds The ids of the users to look up comment reactions for.
     * @return The comment reactions by the given users.
     * @throws IllegalArgumentException in case the given {@literal userIds} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForUsers(final Iterable<UUID> userIds);

    /**
     * Finds all comment reactions which where directly made by the users with the given ids.
     *
     * The comment reactions are sorted newest to oldest.
     *
     * @param userIds The ids of the users to look up comment reactions for.
     * @return The comment reactions by the given users.
     * @throws IllegalArgumentException in case the given {@literal userIds} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForUsers(final Publisher<UUID> userIds);
}
