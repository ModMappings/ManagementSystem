using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Core.Release;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Component;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Mapping
{
    public interface IMethodComponentReader
        : IComponentReader
    {
        Task<IQueryable<Component>> GetByClassInLatestGameVersion(Guid classId);

        Task<IQueryable<Component>> GetByClassInGameVersion(Guid classId, Guid gameVersionId);

        Task<IQueryable<Component>> GetByClassInGameVersion(Guid classId, GameVersion gameVersion);

        Task<IQueryable<Component>> GetByClassInLatestRelease(Guid classId);

        Task<IQueryable<Component>> GetByClassInRelease(Guid classId, Guid releaseId);

        Task<IQueryable<Component>> GetByClassInRelease(Guid classId, Release release);
    }
}
