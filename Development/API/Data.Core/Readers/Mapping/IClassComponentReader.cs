using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Mapping
{
    public interface IClassComponentReader
        : IComponentReader
    {
        Task<IQueryable<Component>> GetByPackageInLatestGameVersion(string packagePattern);

        Task<IQueryable<Component>> GetByPackageInGameVersion(string packagePattern, Guid gameVersionId);

        Task<IQueryable<Component>> GetByPackageInGameVersion(string packagePattern, GameVersion gameVersion);

        Task<IQueryable<Component>> GetByPackageInLatestRelease(string packagePattern);

        Task<IQueryable<Component>> GetByPackageInRelease(string packagePattern, Guid releaseId);

        Task<IQueryable<Component>> GetByPackageInRelease(string packagePattern, Release release);
    }
}
