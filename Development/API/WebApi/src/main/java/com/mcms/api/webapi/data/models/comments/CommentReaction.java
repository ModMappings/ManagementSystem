package com.mcms.api.webapi.data.models.comments;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

@Table
public class CommentReaction {

    @Id
    private final UUID      id;
    private final UUID      createdBy;
    private final Timestamp createdOn;
    private final CommentReactionType type;
    private final UUID commentId;

    @PersistenceConstructor
    CommentReaction(final UUID id, final UUID createdBy, final Timestamp createdOn, final CommentReactionType type, final UUID commentId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.type = type;
        this.commentId = commentId;
    }

    public CommentReaction(final UUID createdBy, final CommentReactionType type, final UUID commentId) {
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

    public CommentReactionType getType() {
        return type;
    }

    public UUID getCommentId() {
        return commentId;
    }
}
