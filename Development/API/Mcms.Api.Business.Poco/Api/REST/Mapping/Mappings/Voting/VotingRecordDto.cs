using System;

namespace Mcms.Api.Business.Poco.Api.REST.Mapping.Mappings.Voting
{
    /// <summary>
    /// Represents a single voting action on a proposal.
    /// </summary>
    public class VotingRecordDto
    {
        /// <summary>
        /// The id of the vote.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The proposal for which a voting took place.
        /// </summary>
        public Guid Proposal { get; set; }

        /// <summary>
        /// The moment the voting record was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// The id of the user who voted.
        /// </summary>
        public Guid VotedBy { get; }

        /// <summary>
        /// Indicates if the voter voted for or against the proposal.
        /// </summary>
        public bool IsForVote { get; set; }

        /// <summary>
        /// Indicates if this vote has been rescinded.
        /// </summary>
        public bool HasBeenRescinded { get; set; }
    }
}
