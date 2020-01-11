package org.modmapping.mmms.repository.model.comments;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

/**
 * Represents a single comment in the database that is made on either
 * a release, proposed mapping, or an other comment.
 *
 * A comment can be edited, and the last editor will be tracked.
 * A comment can be "hidden", by setting its delete data without deleting it from the database.
 * This last element is useful for archiving purposes.
 *
 * Objects of this type are stored in the "comment" table.
 */
@Table("comment")
public class CommentDMO {

    /**
     * Creates a new default comment for a given release.
     *
     * @param createdBy The uuid of the user who created the comment.
     * @param content The content of the comment.
     * @param releaseId The id of the release on which the comment was made.
     *
     * @return The comment data.
     */
    public static CommentDMO createNewForRelease(UUID createdBy, String content, UUID releaseId)
    {
        return new CommentDMO(
                        UUID.randomUUID(),
                        createdBy,
                        Timestamp.from(Instant.now()),
                        content,
                        null,
                        null,
                        null,
                        null,
                        null,
                        releaseId,
                        null
        );
    }

    /**
     * Creates a new default comment for a given proposed mapping.
     *
     * @param createdBy The uuid of the user who created the comment.
     * @param content The content of the comment.
     * @param proposedMappingId The id of the proposed mapping on which the comment was made.
     *
     * @return The comment data.
     */
    public static CommentDMO createNewForProposedMapping(UUID createdBy, String content, UUID proposedMappingId)
    {
        return new CommentDMO(
                        UUID.randomUUID(),
                        createdBy,
                        Timestamp.from(Instant.now()),
                        content,
                        null,
                        null,
                        null,
                        null,
                        proposedMappingId,
                        null,
                        null
        );
    }

    /**
     * Creates a new default comment for a given comment.
     *
     * @param createdBy The uuid of the user who created the comment.
     * @param content The content of the comment.
     * @param parentCommentId The id of the parent comment on which the comment was made.
     *
     * @return The comment data.
     */
    public static CommentDMO createNewForComment(UUID createdBy, String content, UUID parentCommentId)
    {
        return new CommentDMO(
                        UUID.randomUUID(),
                        createdBy,
                        Timestamp.from(Instant.now()),
                        content,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        parentCommentId
        );
    }

    @Id
    private final UUID id;
    private final UUID createdBy;
    private final Timestamp createdOn;
    private final String content;
    private final UUID lastEditBy;
    private final Timestamp lastEditOn;
    private final UUID deletedBy;
    private final Timestamp deletedOn;
    private final UUID proposedMappingId;
    private final UUID releaseId;
    private final UUID parentCommentId;

    @PersistenceConstructor
    CommentDMO(
                    final UUID id,
                    final UUID createdBy,
                    final Timestamp createdOn,
                    final String content,
                    final UUID lastEditBy,
                    final Timestamp lastEditOn,
                    final UUID deletedBy,
                    final Timestamp deletedOn,
                    final UUID proposedMappingId,
                    final UUID releaseId,
                    final UUID parentCommentId) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.content = content;
        this.lastEditBy = lastEditBy;
        this.lastEditOn = lastEditOn;
        this.deletedBy = deletedBy;
        this.deletedOn = deletedOn;
        this.proposedMappingId = proposedMappingId;
        this.releaseId = releaseId;
        this.parentCommentId = parentCommentId;
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

    public String getContent() {
        return content;
    }

    public UUID getLastEditBy() {
        return lastEditBy;
    }

    public Timestamp getLastEditOn() {
        return lastEditOn;
    }

    public boolean hasBeenEdited() {
        return lastEditBy != null || lastEditOn != null;
    }

    public UUID getDeletedBy() {
        return deletedBy;
    }

    public Timestamp getDeletedOn() {
        return deletedOn;
    }

    public boolean hasBeenDeleted() {
        return deletedBy != null || deletedOn != null;
    }

    public UUID getProposedMappingId() {
        return proposedMappingId;
    }

    public UUID getReleaseId() {
        return releaseId;
    }

    public UUID getParentCommentId() {
        return parentCommentId;
    }

    /**
     * Creates a new comment with the edited contents, as well as the edited flags set.
     *
     * @param editedBy The user id of the person who edited this comment.
     * @param newContents The new contents.
     *
     * @return The new comment with the edited data.
     */
    public CommentDMO edit(UUID editedBy, String newContents)
    {
        return new CommentDMO(
                        id,
                        createdBy,
                        createdOn,
                        newContents,
                        editedBy,
                        Timestamp.from(Instant.now()),
                        deletedBy,
                        deletedOn,
                        proposedMappingId,
                        releaseId,
                        parentCommentId
        );
    }

    /**
     * Creates a new comment with the delete flags set.
     *
     * @param deletedBy The user who deleted this comment.
     *
     * @return The comment with the delete flags.
     */
    public CommentDMO delete(UUID deletedBy)
    {
        return new CommentDMO(
                        id,
                        createdBy,
                        createdOn,
                        content,
                        lastEditBy,
                        lastEditOn,
                        deletedBy,
                        Timestamp.from(Instant.now()),
                        proposedMappingId,
                        releaseId,
                        parentCommentId
        );
    }
}
