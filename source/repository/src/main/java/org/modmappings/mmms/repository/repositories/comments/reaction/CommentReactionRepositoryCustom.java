package org.modmappings.mmms.repository.repositories.comments.reaction;

import org.modmappings.mmms.repository.model.comments.CommentReactionDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.NoRepositoryBean;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Represents a comment reaction repository, that provides access to its data
 * via custom accessor methods.
 * <p>
 * In particular lookups via comments and the user who made the reactions is possible.
 */
@NoRepositoryBean
public interface CommentReactionRepositoryCustom {
    /**
     * Finds all comment reactions which where directly made on the comment with the given id.
     *
     * @param commentId The id of the comment to look up comment reactions for.
     * @param pageable  The paging and sorting information.
     * @return The comment reactions on the given comment.
     * @throws IllegalArgumentException in case the given {@literal commentId} is {@literal null}.
     */
    Mono<Page<CommentReactionDMO>> findAllByCommentId(UUID commentId, Pageable pageable);

    /**
     * Finds all comment reactions which where directly made by the user with the given id.
     * <p>
     * The comment reactions are sorted newest to oldest.
     *
     * @param userId The id of the user to look up comment reactions for.
     * @return The comment reactions by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Mono<Page<CommentReactionDMO>> findAllForUser(UUID userId, Pageable pageable);
}
