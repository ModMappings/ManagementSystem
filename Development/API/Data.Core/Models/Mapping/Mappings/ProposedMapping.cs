using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Core.Models.Comments;
using Data.Core.Models.Mapping.Mappings.Voting;

namespace Data.Core.Models.Mapping.Mappings
{
    /// <summary>
    /// Represents a single proposal for a mapping.
    /// </summary>
    public class ProposedMapping
        : MappingBase
    {
        /// <summary>
        /// The user who proposed the new mapping
        /// </summary>
        [Required]
        public virtual Guid ProposedBy { get; set; }

        /// <summary>
        /// The moment the proposal was made.
        /// </summary>
        [Required]
        public DateTime ProposedOn { get; set; }

        /// <summary>
        /// Indicates if the proposal is still open, or if it has been closed.
        /// </summary>
        [Required]
        public bool IsOpen { get; set; }

        /// <summary>
        /// Indicates if the proposal is a public vote.
        /// Else voting rights are required.
        /// </summary>
        [Required]
        public bool IsPublicVote { get; set; }

        /// <summary>
        /// The votes for or against this proposal made by users.
        /// </summary>
        public virtual List<VotingRecord> Votes { get; set; }

        /// <summary>
        /// The comments made by users, at least one comment is required (the comment made by the opener).
        /// </summary>
        [Required]
        public virtual List<Comment> Comments { get; set; }

        /// <summary>
        /// The id of the user who closed the proposal.
        /// </summary>
        public virtual Guid? ClosedBy { get; set; }

        /// <summary>
        /// The moment the proposal was closed.
        /// </summary>
        public DateTime? ClosedOn { get; set; }

        /// <summary>
        /// Indicates if the proposal was merged into a committed mapping during closing.
        /// </summary>
        public bool? Merged { get; set; }

        /// <summary>
        /// The id of the committed mapping (if it exists, and the proposal has been merged successfully), with whom this proposal was merged into the committed data.
        /// </summary>
        public Guid? CommittedWithId { get; set; }

        /// <summary>
        /// The committed mapping (if it exists, and the proposal has been merged successfully), with whom this proposal was merged into the committed data.
        /// </summary>
        [ForeignKey("CommittedWithId")]
        public virtual CommittedMapping CommittedWith { get; set; }
    }
}
