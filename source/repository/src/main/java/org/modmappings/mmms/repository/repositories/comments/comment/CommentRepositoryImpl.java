package org.modmappings.mmms.repository.repositories.comments.comment;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

/**
 * Implementation of the {@link CommentRepository} interface.
 *
 * Uses the methods that are part of the {@link AbstractModMappingRepository} to build querries if possible.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class CommentRepositoryImpl extends AbstractModMappingRepository<CommentDMO> implements CommentRepository {

    public CommentRepositoryImpl(DatabaseClient databaseClient, ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, CommentDMO.class);
    }

    /**
     * Finds all comments which where directly made on the release with the given id.
     *
     * @param releaseId The id of the release to look up comments for.
     * @param pageable The paging information for the query.
     * @return The comments on the given release.
     * @throws IllegalArgumentException in case the given {@literal releaseId} is {@literal null}.
     */
    @Override
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
    @Override
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
    @Override
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
    @Override
    public Mono<Page<CommentDMO>> findAllForUser(final UUID userId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("created_by", userId, pageable);
    }
}
