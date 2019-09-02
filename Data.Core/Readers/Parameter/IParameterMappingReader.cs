using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Parameter
{
    public interface IParameterMappingReader : INoneUniqueNamedMappingReader<ParameterMapping, ParameterVersionedMapping, ParameterCommittedMappingEntry, ParameterProposalMappingEntry, ParameterReleaseMember>
    {
        Task<IQueryable<ParameterMapping>> GetByMethodInLatestVersion(Guid methodId);

        Task<IQueryable<ParameterMapping>> GetByMethodInLatestRelease(Guid methodId);

        Task<IQueryable<ParameterMapping>> GetByMethodInLatestVersion(MethodMapping methodMapping);

        Task<IQueryable<ParameterMapping>> GetByMethodInLatestRelease(MethodMapping methodMapping);

        Task<IQueryable<ParameterMapping>> GetByMethodInVersion(Guid methodId, Guid versionId);

        Task<IQueryable<ParameterMapping>> GetByMethodInRelease(Guid methodId, Guid releaseId);

        Task<IQueryable<ParameterMapping>> GetByMethodInVersion(MethodMapping methodMapping, Guid versionId);

        Task<IQueryable<ParameterMapping>> GetByMethodInRelease(MethodMapping methodMapping, Guid releaseId);
    }
}
