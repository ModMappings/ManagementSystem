package com.mcms.data.core.repositories.comments;

import java.util.UUID;

import com.mcms.api.datamodel.comments.CommentDMO;
import com.mcms.data.core.repositories.IRepository;
import org.reactivestreams.Publisher;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link CommentDMO} objects.
 */
public interface ICommentRepository extends IRepository<CommentDMO> {
    /**
     * Finds all comments which where directly made on the release with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param releaseId The id of the release to look up comments for.
     * @return The comments on the given release.
     * @throws IllegalArgumentException in case the given {@literal releaseId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForRelease(final UUID releaseId);

    /**
     * Finds all comments which where directly made on the releases with the given ids.
     *
     * The comments are sorted oldest to newest.
     *
     * @param releaseIds The ids of the releases to look up comments for.
     * @return The comments on the given releases.
     * @throws IllegalArgumentException in case the given {@literal releaseIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForReleases(final Iterable<UUID> releaseIds);

    /**
     * Finds all comments which where directly made on the releases with the given ids.
     *
     * The comments are sorted oldest to newest.
     *
     * @param releaseIds The ids of the releases to look up comments for.
     * @return The comments on the given releases.
     * @throws IllegalArgumentException in case the given {@literal releaseIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForReleases(final Publisher<UUID> releaseIds);

    /**
     * Finds all comments which where directly made on the proposed mapping with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param proposedMappingId The id of the proposed mapping to look up comments for.
     * @return The comments on the given proposed mapping.
     * @throws IllegalArgumentException in case the given {@literal proposedMappingId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForProposedMapping(final UUID proposedMappingId);

    /**
     * Finds all comments which where directly made on the proposed mappings with the given ids.
     *
     * The comments are sorted oldest to newest.
     *
     * @param proposedMappingIds The ids of the proposed mappings to look up comments for.
     * @return The comments on the given proposed mappings.
     * @throws IllegalArgumentException in case the given {@literal proposedMappingIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForProposedMappings(final Iterable<UUID> proposedMappingIds);

    /**
     * Finds all comments which where directly made on the proposed mappings with the given ids.
     *
     * The comments are sorted oldest to newest.
     *
     * @param proposedMappingIds The ids of the proposed mappings to look up comments for.
     * @return The comments on the given proposed mappings.
     * @throws IllegalArgumentException in case the given {@literal proposedMappingIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForProposedMappings(final Publisher<UUID> proposedMappingIds);

    /**
     * Finds all comments which where directly made on the comment with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param commentId The id of the comment to look up comments for.
     * @return The comments on the given comment.
     * @throws IllegalArgumentException in case the given {@literal commentId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForComment(final UUID commentId);

    /**
     * Finds all comments which where directly made on the comments with the given ids.
     *
     * The comments are sorted oldest to newest.
     *
     * @param commentIds The ids of the comments to look up comments for.
     * @return The comments on the given comments.
     * @throws IllegalArgumentException in case the given {@literal commentIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForComments(final Iterable<UUID> commentIds);

    /**
     * Finds all comments which where directly made on the comments with the given ids.
     *
     * The comments are sorted oldest to newest.
     *
     * @param commentIds The ids of the comments to look up comments for.
     * @return The comments on the given comments.
     * @throws IllegalArgumentException in case the given {@literal commentIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForComments(final Publisher<UUID> commentIds);

    /**
     * Finds all comments which where directly made by the user with the given id.
     *
     * The comments are sorted newest to oldest.
     *
     * @param userId The id of the user to look up comments for.
     * @return The comments by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForUser(final UUID userId);

    /**
     * Finds all comments which where directly made by the users with the given ids.
     *
     * The comments are sorted newest to oldest.
     *
     * @param userIds The ids of the users to look up comments for.
     * @return The comments by the given users.
     * @throws IllegalArgumentException in case the given {@literal userIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForUsers(final Iterable<UUID> userIds);

    /**
     * Finds all comments which where directly made by the users with the given ids.
     *
     * The comments are sorted newest to oldest.
     *
     * @param userIds The ids of the users to look up comments for.
     * @return The comments by the given users.
     * @throws IllegalArgumentException in case the given {@literal userIds} is {@literal null}.
     */
    Flux<CommentDMO> findAllForUsers(final Publisher<UUID> userIds);
}
