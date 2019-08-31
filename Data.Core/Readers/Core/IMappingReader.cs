using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IMappingReader<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TMapping : AbstractMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TVersionedMapping : AbstractVersionedMapping<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TCommittedEntry : AbstractCommittedMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
        where TProposalEntry : AbstractProposalMappingEntry<TMapping, TVersionedMapping, TCommittedEntry, TProposalEntry>
    {
        Task<TMapping> GetById(Guid id);
        Task<IQueryable<TMapping>> AsMappingQueryable();
        Task<IQueryable<TMapping>> GetByLatestRelease();
        Task<IQueryable<TMapping>> GetByRelease(Guid releaseId);
        Task<IQueryable<TMapping>> GetByRelease(string releaseName);
        Task<IQueryable<TMapping>> GetByRelease(Release release);
        Task<IQueryable<TMapping>> GetByVersion(Guid versionId);
        Task<IQueryable<TMapping>> GetByVersion(string versionName);
        Task<IQueryable<TMapping>> GetByVersion(GameVersion version);
    }
}
