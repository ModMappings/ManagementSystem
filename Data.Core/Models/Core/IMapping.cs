using System;
using System.Collections.Generic;

namespace Data.Core.Models.Core
{
    /// <summary>
    /// Represents a single mapped object across its existence in the games sourcecode.
    /// </summary>
    public interface IMapping<TVersionedMapping, TCommittedEntries, TProposalEntries>
        where TVersionedMapping : IVersionedMapping<TCommittedEntries, TProposalEntries>
        where TCommittedEntries : ICommittedMappingEntry<TProposalEntries>
        where TProposalEntries : IProposalMappingEntry
    {
        /// <summary>
        /// The id of the mapping.
        /// </summary>
        Guid Id { get; set;}

        /// <summary>
        /// Holds all the mappings across the different versions of the game.
        /// </summary>
        ICollection<TVersionedMapping> VersionedMappings { get; set; }
    }
}
