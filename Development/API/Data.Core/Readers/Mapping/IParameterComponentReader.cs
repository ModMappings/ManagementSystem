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
    public interface IParameterComponentReader
        : IComponentReader
    {
        Task<IQueryable<Component>> GetByMethodInLatestGameVersion(Guid methodId);

        Task<IQueryable<Component>> GetByMethodInGameVersion(Guid methodId, Guid gameVersionId);

        Task<IQueryable<Component>> GetByMethodInGameVersion(Guid methodId, GameVersion gameVersion);

        Task<IQueryable<Component>> GetByMethodInLatestRelease(Guid methodId);

        Task<IQueryable<Component>> GetByMethodInRelease(Guid methodId, Guid releaseId);

        Task<IQueryable<Component>> GetByMethodInRelease(Guid methodId, Release release);
    }
}
