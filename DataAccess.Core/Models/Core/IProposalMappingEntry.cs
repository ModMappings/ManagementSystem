using System;
using System.Collections.Generic;

namespace DataAccess.Core.Models.Core
{
    /// <summary>
    /// An interface that indicates that this object is not a committed change.
    /// </summary>
    public interface IProposalMappingEntry : IMappingEntry
    {
        /// <summary>
        /// The id of the proposal.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The user who made this proposal.
        /// </summary>
        IUser ProposedBy { get; set; }

        /// <summary>
        /// The moment that this proposal was created on.
        /// </summary>
        DateTime ProposedOn { get; set; }

        /// <summary>
        /// Indicates if this proposal can be voted on by the general public.
        /// A user account is still required, but no reviewer rights are needed.
        /// </summary>
        bool IsPublicVote { get; set; }

        /// <summary>
        /// The users who voted for a given proposal.
        /// </summary>
        ICollection<IUser> VotedFor { get; set; }

        /// <summary>
        /// The users who voted against a given proposal.
        /// </summary>
        ICollection<IUser> VotedAgainst { get; set; }

        /// <summary>
        /// Comment made during proposal creation.
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// The user who merged the changes and turned this proposal into a mapping entry.
        /// </summary>
        IUser CommittedBy { get; set; }

        /// <summary>
        /// The moment this proposal was merged and turned into a mapping entry.
        /// </summary>
        DateTime CommittedOn { get; set; }
    }
}
