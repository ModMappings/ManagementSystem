package org.modmappings.mmms.repository.repositories.comments;

import java.util.UUID;

import org.modmappings.mmms.repository.model.comments.CommentReactionDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository that can provide and store {@link CommentReactionDMO} objects.
 */
@Repository
public interface ICommentReactionRepository extends R2dbcRepository<CommentReactionDMO, UUID> {

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
    @Query("Select * from comment_reaction cr where cr.commentReactionId = $1")
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
    @Query("Select * from comment_reaction cr where cr.createdBy = $1")
    Flux<CommentReactionDMO> findAllForUser(final UUID userId, final Pageable pageable);
}
