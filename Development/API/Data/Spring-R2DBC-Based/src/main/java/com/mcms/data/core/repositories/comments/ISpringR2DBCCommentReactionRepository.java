package com.mcms.data.core.repositories.comments;

import java.util.UUID;

import com.mcms.api.datamodel.comments.CommentReactionDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;

/**
 * This is the R2DBC based implementation of the {@link ICommentReactionRepository}.
 *
 * This interface, although not directly implementing the methods from the {@link ICommentReactionRepository},
 * provides all the functionality via the {@link Query}-Annotation that is put on its methods with the relevant SQL Query implemented.
 */
public interface ISpringR2DBCCommentReactionRepository extends ICommentReactionRepository {

    @Query("Select * from comment_reaction cr where cr.commentReactionId = $1")
    @Override
    Flux<CommentReactionDMO> findAllForComment(final UUID commentReactionId, final Pageable pageable);

    @Query("Select * from comment_reaction cr where cr.createdBy = $1")
    @Override
    Flux<CommentReactionDMO> findAllForUser(final UUID userId, final Pageable pageable);
}
