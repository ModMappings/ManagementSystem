using System;
using System.Linq;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    /// <summary>
    /// Represents a reader for a given mapping.
    /// </summary>
    /// <typeparam name="TMapping">The type of the mapping that is being read.</typeparam>
    /// <typeparam name="TCommittedEntry">The type that represents the committed mapping entries.</typeparam>
    /// <typeparam name="TProposalEntry">The type that represents the proposal mapping entries.</typeparam>
    /// <typeparam name="TVersionedMapping">The versioned mapping type.</typeparam>
    public interface IMappingReader<out TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TMapping : IMapping<TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : IVersionedMapping<TCommittedEntry, TProposalEntry>
        where TCommittedEntry : ICommittedMappingEntry<TProposalEntry>
        where TProposalEntry : IProposalMappingEntry
    {

        TMapping GetById(Guid id);

        TMapping GetByLatestVersion();

        TMapping GetByVersion(Guid versionId);

        TMapping GetByVersion(IGameVersion gameVersion);

        TMapping GetByLatestMapping(string name);

        TMapping GetByMappingInVersion(string name, Guid versionId);

        TMapping GetByMappingInVersion(string name, IGameVersion gameVersion);

        TMapping GetByMappingInRelease(string name, Guid releaseId);

        TMapping GetByMappingInRelease(string name, IRelease release);

        IQueryable<TMapping> AsMappingQueryable();

        IQueryable<TMapping> GetByLatestRelease();

        IQueryable<TMapping> GetByReleaseVersion(Guid releaseId);

        IQueryable<TMapping> GetByRelease(string releaseName);

        IQueryable<TMapping> GetByRelease(IRelease release);
    }
}
