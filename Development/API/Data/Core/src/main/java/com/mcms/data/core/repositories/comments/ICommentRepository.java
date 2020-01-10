package com.mcms.data.core.repositories.comments;

import java.util.UUID;

import com.mcms.api.datamodel.comments.CommentDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link CommentDMO} objects.
 */
public interface ICommentRepository extends CrudRepository<CommentDMO, UUID> {
    /**
     * Finds all comments which where directly made on the release with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param releaseId The id of the release to look up comments for.
     * @param pageable The pagination information for the query.
     * @return The comments on the given release.
     * @throws IllegalArgumentException in case the given {@literal releaseId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForRelease(final UUID releaseId, final Pageable pageable);

    /**
     * Finds all comments which where directly made on the proposed mapping with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param proposedMappingId The id of the proposed mapping to look up comments for.
     * @param pageable The pagination information for the query.
     * @return The comments on the given proposed mapping.
     * @throws IllegalArgumentException in case the given {@literal proposedMappingId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForProposedMapping(final UUID proposedMappingId, final Pageable pageable);

    /**
     * Finds all comments which where directly made on the comment with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param commentId The id of the comment to look up comments for.
     * @param pageable The pagination information for the query.
     * @return The comments on the given comment.
     * @throws IllegalArgumentException in case the given {@literal commentId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForComment(final UUID commentId, final Pageable pageable);

    /**
     * Finds all comments which where directly made by the user with the given id.
     *
     * The comments are sorted newest to oldest.
     *
     * @param userId The id of the user to look up comments for.
     * @param pageable The pagination information for the query.
     * @return The comments by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForUser(final UUID userId, final Pageable pageable);
}