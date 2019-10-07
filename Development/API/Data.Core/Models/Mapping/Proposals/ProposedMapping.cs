using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Mapping.Proposals
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
        /// Else voting rights are required
        /// </summary>
        [Required]
        public bool IsPublicVote { get; set; }

        public virtual List<Guid> VotedFor { get; set; }

        public virtual List<Guid> VotedAgainst { get; set; }

        [Required]
        public virtual List<ProposalComment> Comments { get; set; }

        public virtual Guid? ClosedBy { get; set; }

        public DateTime? ClosedOn { get; set; }

        public bool? Merged { get; set; }

        public Guid? WentLiveWithId { get; set; }

        [ForeignKey("WentLiveWithId")]
        public virtual LiveMappingEntry WentLiveWith { get; set; }
    }
}
