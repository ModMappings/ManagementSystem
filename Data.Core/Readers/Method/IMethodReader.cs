using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Method;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Method
{
    public interface IMethodReader : IMappingReader<IMethodMapping, IMethodVersionedMapping, IMethodCommittedMappingEntry, IMethodProposalMappingEntry>
    {
        Task<IQueryable<IMethodMapping>> GetByClassInLatestVersion(Guid classId);

        Task<IQueryable<IMethodMapping>> GetByClassInLatestRelease(Guid classId);

        Task<IQueryable<IMethodMapping>> GetByClassInLatestVersion(IClassMapping classMapping);

        Task<IQueryable<IMethodMapping>> GetByClassInLatestRelease(IClassMapping classMapping);

        Task<IQueryable<IMethodMapping>> GetByClassInVersion(Guid classId, Guid versionId);

        Task<IQueryable<IMethodMapping>> GetByClassInRelease(Guid classId, Guid releaseId);

        Task<IQueryable<IMethodMapping>> GetByClassInVersion(IClassMapping classMapping, Guid versionId);

        Task<IQueryable<IMethodMapping>> GetByClassInRelease(IClassMapping classMapping, Guid releaseId);

    }
}
