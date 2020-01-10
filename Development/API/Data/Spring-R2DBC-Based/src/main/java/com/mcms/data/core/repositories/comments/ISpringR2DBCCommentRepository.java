package com.mcms.data.core.repositories.comments;

import java.util.UUID;

import com.mcms.api.datamodel.comments.CommentDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;

/**
 * This is the R2DBC based implementation of the {@link ICommentRepository}.
 *
 * This interface, although not directly implementing the methods from the {@link ICommentRepository},
 * provides all the functionality via the {@link Query}-Annotation that is put on its methods with the relevant SQL Query implemented.
 */
public interface ISpringR2DBCCommentRepository extends ICommentRepository {

    @Query("Select * from comments c where c.releaseId = $1")
    @Override
    Flux<CommentDMO> findAllForRelease(
                    final UUID releaseId, final Pageable pageable);

    @Query("Select * from comments c where c.proposedMappingId = $1")
    @Override
    Flux<CommentDMO> findAllForProposedMapping(
                    final UUID proposedMappingId, final Pageable pageable);

    @Query("Select * from comments c where c.parentCommentId = $1")
    @Override
    Flux<CommentDMO> findAllForComment(
                    final UUID commentId, final Pageable pageable);

    @Query("Select * from comments c where c.createdBy = $1")
    @Override
    Flux<CommentDMO> findAllForUser(final UUID userId, final Pageable pageable);
}
