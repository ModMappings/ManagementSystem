using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IMappingReader<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TTypedMapping : AbstractTypedMapping<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TCommittedEntry : AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
        where TReleaseEntry : AbstractReleaseMember<TMapping, TVersionedMapping, TTypedMapping, TCommittedEntry, TProposalEntry, TReleaseEntry>
    {
        Task<TMapping> GetById(Guid id);
        Task<IQueryable<TMapping>> AsMappingQueryable();
        Task<IQueryable<TMapping>> GetByLatestRelease();
        Task<IQueryable<TMapping>> GetByRelease(Guid releaseId);
        Task<IQueryable<TMapping>> GetByRelease(string releaseName);
        Task<IQueryable<TMapping>> GetByRelease(Release release);
        Task<IQueryable<TMapping>> GetByLatestVersion();
        Task<IQueryable<TMapping>> GetByVersion(Guid versionId);
        Task<IQueryable<TMapping>> GetByVersion(string versionName);
        Task<IQueryable<TMapping>> GetByVersion(GameVersion version);
    }
}
