using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Parameter
{
    public interface IParameterReader : IMappingReader<IParameterMapping, IParameterVersionedMapping, IParameterCommittedMappingEntry, IParameterProposalMappingEntry>
    {
        Task<IQueryable<IParameterMapping>> GetByMethodInLatestVersion(Guid methodId);

        Task<IQueryable<IParameterMapping>> GetByMethodInLatestRelease(Guid methodId);

        Task<IQueryable<IParameterMapping>> GetByMethodInLatestVersion(IMethodMapping methodMapping);

        Task<IQueryable<IParameterMapping>> GetByMethodInLatestRelease(IMethodMapping methodMapping);

        Task<IQueryable<IParameterMapping>> GetByMethodInVersion(Guid methodId, Guid versionId);

        Task<IQueryable<IParameterMapping>> GetByMethodInRelease(Guid methodId, Guid releaseId);

        Task<IQueryable<IParameterMapping>> GetByMethodInVersion(IMethodMapping methodMapping, Guid versionId);

        Task<IQueryable<IParameterMapping>> GetByMethodInRelease(IMethodMapping methodMapping, Guid releaseId);

    }
}
