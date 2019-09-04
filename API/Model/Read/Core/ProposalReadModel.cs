using System;
using System.Collections.Generic;

namespace API.Model.Read.Core
{
    /// <summary>
    /// A view model for a proposal to change a mapping.
    /// </summary>
    public class ProposalReadModel
        : MappingReadModel
    {
        /// <summary>
        /// The id of the type for which the proposal was made.
        /// </summary>
        public Guid ProposedFor { get; set; }

        /// <summary>
        /// The id of the game version for which the proposal is created.
        /// </summary>
        public Guid GameVersion { get; set; }

        /// <summary>
        /// The id of the user for which the proposal was made.
        /// </summary>
        public Guid ProposedBy { get; set; }

        /// <summary>
        /// The date and time on which the proposal was made.
        /// </summary>
        public DateTime ProposedOn { get; set; }

        /// <summary>
        /// Indicates if this proposal is still open.
        /// Open requests can be voted on.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Indicates if this vote is open to the public.
        /// If the vote is public, any authenticated user can vote on this proposal.
        /// If not then only users with a review right can vote on the proposal.
        ///
        /// Default is false.
        /// </summary>
        public bool IsPublicVote { get; set; }

        /// <summary>
        /// A list of ids of users who voted for the proposal.
        /// </summary>
        public IEnumerable<Guid> VotedFor { get; set; }

        /// <summary>
        /// A list of ids of users who voted against the proposal.
        /// </summary>
        public IEnumerable<Guid> VotedAgainst { get; set; }

        /// <summary>
        /// The comment made to explain the proposal.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The user who closed the proposal.
        /// </summary>
        public Guid? ClosedBy { get; set; }

        /// <summary>
        /// The date and time when the proposal was closed.
        /// </summary>
        public DateTime? ClosedOn { get; set; }
    }
}
