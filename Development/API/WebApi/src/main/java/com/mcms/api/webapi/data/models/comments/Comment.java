package com.mcms.api.webapi.data.models.comments;

import java.sql.Timestamp;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

@Table
public class Comment {

    @Id
    private final UUID id;
    private final UUID createdBy;
    private final Timestamp createdOn;
    private final String content;
    private final boolean hasBeenEdited;
    private final boolean isDeleted;
    private final UUID deletedBy;
    private final Timestamp deletedOn;
    private final UUID proposedMappingId;
    private final UUID releaseId;
    private final UUID parentCommentId;

    @PersistenceConstructor
    Comment(
                    final UUID id,
                    final UUID createdBy,
                    final Timestamp createdOn,
                    final String content,
                    final boolean hasBeenEdited,
                    final boolean isDeleted,
                    final UUID deletedBy,
                    final Timestamp deletedOn,
                    final UUID proposedMappingId,
                    final UUID releaseId,
                    final UUID parentCommentId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.content = content;
        this.hasBeenEdited = hasBeenEdited;
        this.isDeleted = isDeleted;
        this.deletedBy = deletedBy;
        this.deletedOn = deletedOn;
        this.proposedMappingId = proposedMappingId;
        this.releaseId = releaseId;
        this.parentCommentId = parentCommentId;
    }


}
