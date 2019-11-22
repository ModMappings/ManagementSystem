using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting
{
    /// <summary>
    /// Represents a single vote for or against a given proposal.
    /// </summary>
    public class VotingRecord
    {
        /// <summary>
        /// The id of the vote.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The proposal for which a voting took place.
        /// </summary>
        [Required]
        public virtual ProposedMapping Proposal { get; set; }

        /// <summary>
        /// The moment the voting record was created.
        /// </summary>
        [Required]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The id of the user who voted.
        /// </summary>
        [Required]
        public Guid VotedBy { get; set; }

        /// <summary>
        /// Indicates if the voter voted for or against the proposal.
        /// </summary>
        [Required]
        public bool IsForVote { get; set; }

        /// <summary>
        /// Indicates if this vote has been rescinded.
        /// </summary>
        [Required]
        public bool HasBeenRescinded { get; set; }
    }
}

