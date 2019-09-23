using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;

namespace Data.Core.Readers.Core
{
    public interface IComponentReader
    {
        Task<Component> GetById(Guid id);

        Task<IQueryable<Component>> AsQueryable();

        Task<IQueryable<Component>> GetByLatestRelease();

        Task<IQueryable<Component>> GetByRelease(Guid releaseId);

        Task<IQueryable<Component>> GetByRelease(string releaseName);

        Task<IQueryable<Component>> GetByRelease(Release release);

        Task<IQueryable<Component>> GetByLatestVersion();

        Task<IQueryable<Component>> GetByVersion(Guid versionId);

        Task<IQueryable<Component>> GetByVersion(string versionName);

        Task<IQueryable<Component>> GetByVersion(GameVersion version);

        Task<IQueryable<Component>> GetByLatestMapping(string name);

        Task<IQueryable<Component>> GetByMappingInVersion(string name, Guid versionId);

        Task<IQueryable<Component>> GetByMappingInVersion(string name, GameVersion gameVersion);

        Task<IQueryable<Component>> GetByMappingInRelease(string name, Guid releaseId);

        Task<IQueryable<Component>> GetByMappingInRelease(string name, Release release);

        Task<VersionedComponent> GetVersionedComponent(Guid id);

        Task<ProposalMappingEntry> GetProposalMapping(Guid id);

        Task<LiveMappingEntry> GetLiveMapping(Guid id);
    }
}
