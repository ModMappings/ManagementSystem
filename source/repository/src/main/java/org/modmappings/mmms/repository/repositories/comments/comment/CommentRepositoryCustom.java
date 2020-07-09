package org.modmappings.mmms.repository.repositories.comments.comment;

import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.NoRepositoryBean;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a comment repository which gives access to its data using custom methods.
 * <p>
 * In particular it allows for lookup of comments on releases, proposed mappings and other comments.
 * As well as looking which comments where made by a given user.
 */
@NoRepositoryBean
public interface CommentRepositoryCustom {
    /**
     * Finds all comments which where directly made on the release with the given id.
     *
     * @param releaseId The id of the release to look up comments for.
     * @param pageable  The paging information for the query.
     * @return The comments on the given release.
     * @throws IllegalArgumentException in case the given {@literal releaseId} is {@literal null}.
     */
    Mono<Page<CommentDMO>> findAllForRelease(UUID releaseId, Pageable pageable);

    /**
     * Finds all comments which where directly made on the proposed mapping with the given id.
     * <p>
     * The comments are sorted oldest to newest.
     *
     * @param proposedMappingId The id of the proposed mapping to look up comments for.
     * @param pageable          The paging information for the query.
     * @return The comments on the given proposed mapping.
     * @throws IllegalArgumentException in case the given {@literal proposedMappingId} is {@literal null}.
     */
    Mono<Page<CommentDMO>> findAllForProposedMapping(UUID proposedMappingId, Pageable pageable);

    /**
     * Finds all comments which where directly made on the comment with the given id.
     * <p>
     * The comments are sorted oldest to newest.
     *
     * @param commentId The id of the comment to look up comments for.
     * @param pageable  The paging information for the query.
     * @return The comments on the given comment.
     * @throws IllegalArgumentException in case the given {@literal commentId} is {@literal null}.
     */
    Mono<Page<CommentDMO>> findAllForComment(UUID commentId, Pageable pageable);

    /**
     * Finds all comments which where directly made by the user with the given id.
     * <p>
     * The comments are sorted newest to oldest.
     *
     * @param userId   The id of the user to look up comments for.
     * @param pageable The paging information for the query.
     * @return The comments by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Mono<Page<CommentDMO>> findAllForUser(UUID userId, Pageable pageable);
}
