package org.modmapping.mmms.repository.model.mapping.mappings.voting;

import java.sql.Timestamp;
import java.time.Instant;
import java.util.UUID;

import org.springframework.data.annotation.Id;
import org.springframework.data.annotation.PersistenceConstructor;
import org.springframework.data.relational.core.mapping.Table;

/**
 * Represents a single vote for or against a given proposal.
 * Table entries should never be deleted, at least while a vote is in progress.
 */
@Table("voting_record")
public class VotingRecordDMO {

    @Id
    private final UUID id;
    private final UUID createdBy;
    private final Timestamp createdOn;
    private final UUID proposedMappingId;
    private final boolean isForVote;
    private final boolean hasBeenRescinded;

    @PersistenceConstructor
    VotingRecordDMO(final UUID id, final UUID createdBy, final Timestamp createdOn, final UUID proposedMappingId, final boolean isForVote, final boolean hasBeenRescinded) {
        this.id = id;
        this.createdBy = createdBy;
        this.createdOn = createdOn;
        this.proposedMappingId = proposedMappingId;
        this.isForVote = isForVote;
        this.hasBeenRescinded = hasBeenRescinded;
    }

    public VotingRecordDMO(final UUID createdBy, final UUID proposedMappingId, final boolean isForVote) {
        this.id = UUID.randomUUID();
        this.createdBy = createdBy;
        this.createdOn = Timestamp.from(Instant.now());
        this.proposedMappingId = proposedMappingId;
        this.isForVote = isForVote;
        this.hasBeenRescinded = false;
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

    public UUID getProposedMappingId() {
        return proposedMappingId;
    }

    public boolean isForVote() {
        return isForVote;
    }

    public boolean isHasBeenRescinded() {
        return hasBeenRescinded;
    }

    /**
     * Rescinds this vote.
     * When a vote gets rescinded this indicates that the users opinion on the matter
     * is back to neutral.
     *
     * The business layer will need to make sure that for every proposal user combination there is only
     * one none rescinded vote in the database.
     * However it is free to leave as many rescinded votes behind until the vote is closed.
     *
     * This allows for tracking of the votes.
     *
     * @return A rescinded version of this voting record.
     */
    public VotingRecordDMO rescind() {
        return new VotingRecordDMO(
                        id,
                        createdBy,
                        createdOn,
                        proposedMappingId,
                        isForVote,
                        true
        );
    }
}

