package com.mcms.data.core.repositories.comments;

import java.util.UUID;

import com.mcms.api.datamodel.comments.CommentReactionDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository that can provide and store {@link CommentReactionDMO} objects.
 */
public interface ICommentReactionRepository extends CrudRepository<CommentReactionDMO, UUID> {

    /**
     * Finds all comment reactions which where directly made on the comment reaction with the given id.
     *
     * The comment reactions are sorted oldest to newest.
     *
     * @param commentReactionId The id of the comment reaction to look up comment reactions for.
     * @param pageable The pagination information for the query.
     * @return The comment reactions on the given comment reaction.
     * @throws IllegalArgumentException in case the given {@literal commentReactionId} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForComment(final UUID commentReactionId, final Pageable pageable);

    /**
     * Finds all comment reactions which where directly made by the user with the given id.
     *
     * The comment reactions are sorted newest to oldest.
     *
     * @param userId The id of the user to look up comment reactions for.
     * @param pageable The pagination information for the query.
     * @return The comment reactions by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<CommentReactionDMO> findAllForUser(final UUID userId, final Pageable pageable);
}
