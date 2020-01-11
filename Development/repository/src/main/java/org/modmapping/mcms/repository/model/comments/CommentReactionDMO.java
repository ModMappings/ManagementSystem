package org.modmapping.mcms.repository.model.comments;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

/**
 * Represents a single mini reaction on a comment via a small set of integrated and supported emoji.
 *
 * To delete a comment reaction, delete its DB entry.
 * A comment reaction can not be edited, or better yet should not be edited.
 * If a user wants to apply a different comment reaction, he or she can delete the first one, and add a new one.
 *
 * A user can have more then one comment reaction on a given comment.
 * The business layer wil need to decide how to deal with that situation and which combinations are allowed if any.
 */
@Table("comment_reaction")
public class CommentReactionDMO {

    @Id
    private final UUID      id;
    private final UUID      createdBy;
    private final Timestamp              createdOn;
    private final CommentReactionTypeDMO type;
    private final UUID                   commentId;

    @PersistenceConstructor
    CommentReactionDMO(final UUID id, final UUID createdBy, final Timestamp createdOn, final CommentReactionTypeDMO type, final UUID commentId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.type = type;
        this.commentId = commentId;
    }

    public CommentReactionDMO(final UUID createdBy, final CommentReactionTypeDMO type, final UUID commentId) {
        this.id = UUID.randomUUID();
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.type = type;
        this.commentId = commentId;
    }

    public UUID getId() {
        return id;
    }

    public UUID getCreatedBy() {
        return createdBy;
    }

    public Timestamp getCreatedOn() {
        return createdOn;
    }

    public CommentReactionTypeDMO getType() {
        return type;
    }

    public UUID getCommentId() {
        return commentId;
    }
}
