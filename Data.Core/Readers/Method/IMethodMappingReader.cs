using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Method
{
    public interface IMethodMappingReader : INoneUniqueNamedMappingReader<MethodMapping, MethodVersionedMapping, MethodCommittedMappingEntry, MethodProposalMappingEntry, MethodReleaseMember>
    {
        Task<IQueryable<MethodMapping>> GetByClassInLatestVersion(Guid classId);

        Task<IQueryable<MethodMapping>> GetByClassInLatestRelease(Guid classId);

        Task<IQueryable<MethodMapping>> GetByClassInLatestVersion(ClassMapping classMapping);

        Task<IQueryable<MethodMapping>> GetByClassInLatestRelease(ClassMapping classMapping);

        Task<IQueryable<MethodMapping>> GetByClassInVersion(Guid classId, Guid versionId);

        Task<IQueryable<MethodMapping>> GetByClassInRelease(Guid classId, Guid releaseId);

        Task<IQueryable<MethodMapping>> GetByClassInVersion(ClassMapping classMapping, Guid versionId);

        Task<IQueryable<MethodMapping>> GetByClassInRelease(ClassMapping classMapping, Guid releaseId);
    }
}
