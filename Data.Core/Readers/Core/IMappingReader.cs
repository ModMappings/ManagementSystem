using System;
using System.Linq;
using System.Threading.Tasks;
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
    public interface IMappingReader<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TMapping : IMapping<TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : IVersionedMapping<TCommittedEntry, TProposalEntry>
        where TCommittedEntry : ICommittedMappingEntry<TProposalEntry>
        where TProposalEntry : IProposalMappingEntry
    {

        Task<TMapping> GetById(Guid id);

        Task<TMapping> GetByLatestVersion();

        Task<TMapping> GetByVersion(Guid versionId);

        Task<TMapping> GetByVersion(IGameVersion gameVersion);

        Task<TMapping> GetByLatestMapping(string name);

        Task<TMapping> GetByMappingInVersion(string name, Guid versionId);

        Task<TMapping> GetByMappingInVersion(string name, IGameVersion gameVersion);

        Task<TMapping> GetByMappingInRelease(string name, Guid releaseId);

        Task<TMapping> GetByMappingInRelease(string name, IRelease release);

        Task<IQueryable<TMapping>> AsMappingQueryable();

        Task<IQueryable<TMapping>> GetByLatestRelease();

        Task<IQueryable<TMapping>> GetByReleaseVersion(Guid releaseId);

        Task<IQueryable<TMapping>> GetByRelease(string releaseName);

        Task<IQueryable<TMapping>> GetByRelease(IRelease release);
    }
}
