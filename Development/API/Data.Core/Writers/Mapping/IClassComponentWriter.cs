using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Mapping
{
    public interface IClassComponentWriter
        : IComponentWriter
    {

        Task<IQueryable<Component>> GetByPackageInVersion(string package, Guid versionId);

        Task<IQueryable<Component>> GetByPackageInVersion(string package, GameVersion gameVersion);

        Task<IQueryable<Component>> GetByPackageInRelease(string package, Guid releaseId);

        Task<IQueryable<Component>> GetByPackageInRelease(string package, Release release);
    }
}
