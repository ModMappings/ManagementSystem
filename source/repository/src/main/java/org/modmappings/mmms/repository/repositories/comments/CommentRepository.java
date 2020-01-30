package org.modmappings.mmms.repository.repositories.comments;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Represents a repository which can provide and store {@link CommentDMO} objects.
 */
@Repository
public class CommentRepository extends ModMappingR2DBCRepository<CommentDMO> {

    public CommentRepository(RelationalEntityInformation<CommentDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all comments which where directly made on the release with the given id.
     *
     * @param releaseId The id of the release to look up comments for.
     * @param pageable The paging information for the query.
     * @return The comments on the given release.
     * @throws IllegalArgumentException in case the given {@literal releaseId} is {@literal null}.
     */
    public Mono<Page<CommentDMO>> findAllForRelease(final UUID releaseId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("release_id", releaseId, pageable);
    }

    /**
     * Finds all comments which where directly made on the proposed mapping with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param proposedMappingId The id of the proposed mapping to look up comments for.
     * @param pageable The paging information for the query.
     * @return The comments on the given proposed mapping.
     * @throws IllegalArgumentException in case the given {@literal proposedMappingId} is {@literal null}.
     */
    public Mono<Page<CommentDMO>> findAllForProposedMapping(final UUID proposedMappingId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("proposed_mapping_id", proposedMappingId, pageable);
    }

    /**
     * Finds all comments which where directly made on the comment with the given id.
     *
     * The comments are sorted oldest to newest.
     *
     * @param commentId The id of the comment to look up comments for.
     * @param pageable The paging information for the query.
     * @return The comments on the given comment.
     * @throws IllegalArgumentException in case the given {@literal commentId} is {@literal null}.
     */
    public Mono<Page<CommentDMO>> findAllForComment(final UUID commentId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("parent_comment_id", commentId, pageable);
    }

    /**
     * Finds all comments which where directly made by the user with the given id.
     *
     * The comments are sorted newest to oldest.
     *
     * @param userId The id of the user to look up comments for.
     * @param pageable The paging information for the query.
     * @return The comments by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    public Mono<Page<CommentDMO>> findAllForUser(final UUID userId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("created_by", userId, pageable);
    }
}
