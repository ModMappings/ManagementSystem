using System;
using System.Collections.Generic;
using Data.Core.Models.Class;

namespace Data.Core.Models.Core
{
    /// <summary>
    /// Represents a mapping in a single version of the game.
    /// </summary>
    /// <typeparam name="TCommittedEntry">The type of the committed mapping entries.</typeparam>
    /// <typeparam name="TProposalEntry">The type of the proposal mapping entries.</typeparam>
    public interface IVersionedMapping<TCommittedEntry, TProposalEntry>
        where TCommittedEntry : ICommittedMappingEntry<TProposalEntry>
        where TProposalEntry : IProposalMappingEntry
    {
        /// <summary>
        /// The id of the mapping.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The version of the game this class was created in.
        /// </summary>
        IGameVersion GameVersion { get; set; }

        /// <summary>
        /// The user who created this version of the mapping.
        /// </summary>
        IUser CreatedBy { get; set; }

        /// <summary>
        /// The moment this version was created
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// The committed entries made for this mapping.
        /// </summary>
        ICollection<TCommittedEntry> CommittedMappings { get; set; }

        /// <summary>
        /// The proposals made for the mapping.
        /// </summary>
        ICollection<TProposalEntry> ProposalMappings { get; set; }
    }
}
