using System;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    /// <summary>
    /// Represents a reader for a given mapping.
    /// The mapping has a unique name, for example classes with their package name included.
    /// </summary>
    public interface
        IUniqueNamedMappingReader<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        : IMappingReader<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TCommittedEntry : AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
    {
        Task<TMapping> GetByLatestMapping(string name);

        Task<TMapping> GetByMappingInVersion(string name, Guid versionId);

        Task<TMapping> GetByMappingInVersion(string name, GameVersion gameVersion);

        Task<TMapping> GetByMappingInRelease(string name, Guid releaseId);

        Task<TMapping> GetByMappingInRelease(string name, Release release);

        Task<TVersionedMapping> GetVersionedMapping(Guid id);

        Task<TProposalEntry> GetProposal(Guid proposalEntry);

        Task<TCommittedEntry> GetCommittedEntry(Guid committedEntryId);
    }
}
