package org.modmappings.mmms.repository.repositories.comments;

import java.util.List;
import java.util.UUID;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.r2dbc.core.ReactiveDataAccessStrategy;
import org.springframework.data.r2dbc.core.StatementMapper;
import org.springframework.data.r2dbc.query.Criteria;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which can provide and store {@link CommentDMO} objects.
 */
@Repository
public class CommentRepository extends ModMappingR2DBCRepository<CommentDMO> {

    private final ExtendedDataAccessStrategy accessStrategy;

    public CommentRepository(RelationalEntityInformation<CommentDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
        this.accessStrategy = accessStrategy;
    }

    /**
     * Finds all comments which where directly made on the release with the given id.
     *
     * @param releaseId The id of the release to look up comments for.
     * @param pageable The paging information for the query.
     * @return The comments on the given release.
     * @throws IllegalArgumentException in case the given {@literal releaseId} is {@literal null}.
     */
    Flux<CommentDMO> findAllForRelease(final UUID releaseId, final Pageable pageable)
    {
        return createSelectWithSingleWhereRequest("release_id", releaseId, pageable);
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
    Flux<CommentDMO> findAllForProposedMapping(final UUID proposedMappingId, final Pageable pageable)
    {
        return createSelectWithSingleWhereRequest("proposed_mapping_id", proposedMappingId, pageable);
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
    Flux<CommentDMO> findAllForComment(final UUID commentId, final Pageable pageable)
    {
        return createSelectWithSingleWhereRequest("parent_comment_id", commentId, pageable);
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
    Flux<CommentDMO> findAllForUser(final UUID userId, final Pageable pageable)
    {
        return createSelectWithSingleWhereRequest("created_by", userId, pageable);
    }
}
