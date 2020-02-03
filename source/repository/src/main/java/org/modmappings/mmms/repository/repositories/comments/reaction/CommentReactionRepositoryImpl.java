package org.modmappings.mmms.repository.repositories.comments.reaction;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.model.comments.CommentReactionDMO;
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
 * Represents a repository that can provide and store {@link CommentReactionDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class CommentReactionRepositoryImpl extends AbstractModMappingRepository<CommentReactionDMO> implements CommentReactionRepository {

    public CommentReactionRepositoryImpl(DatabaseClient databaseClient, ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, CommentReactionDMO.class);
    }

    /**
     * Finds all comment reactions which where directly made on the comment with the given id.
     *
     * @param commentId The id of the comment to look up comment reactions for.
     * @param pageable The paging and sorting information.
     * @return The comment reactions on the given comment.
     * @throws IllegalArgumentException in case the given {@literal commentId} is {@literal null}.
     */
    @Override
    public Mono<Page<CommentReactionDMO>> findAllByCommentId(final UUID commentId, Pageable pageable) {
        return createPagedStarSingleWhereRequest("comment_id", commentId, pageable);
    }

    /**
     * Finds all comment reactions which where directly made by the user with the given id.
     *
     * The comment reactions are sorted newest to oldest.
     *
     * @param userId The id of the user to look up comment reactions for.
     * @return The comment reactions by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    @Override
    public Mono<Page<CommentReactionDMO>> findAllForUser(final UUID userId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest("created_by", userId, pageable);
    }
}
