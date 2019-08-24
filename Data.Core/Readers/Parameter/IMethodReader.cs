using System;
using System.Linq;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Parameter
{
    public interface IParameterReader : IMappingReader<IParameterMapping, IParameterVersionedMapping, IParameterCommittedMappingEntry, IParameterProposalMappingEntry>
    {
        IQueryable<IParameterMapping> GetByMethodInLatestVersion(Guid methodId);

        IQueryable<IParameterMapping> GetByMethodInLatestRelease(Guid methodId);

        IQueryable<IParameterMapping> GetByMethodInLatestVersion(IMethodMapping methodMapping);

        IQueryable<IParameterMapping> GetByMethodInLatestRelease(IMethodMapping methodMapping);

        IQueryable<IParameterMapping> GetByMethodInVersion(Guid methodId, Guid versionId);

        IQueryable<IParameterMapping> GetByMethodInRelease(Guid methodId, Guid releaseId);

        IQueryable<IParameterMapping> GetByMethodInVersion(IMethodMapping methodMapping, Guid versionId);

        IQueryable<IParameterMapping> GetByMethodInRelease(IMethodMapping methodMapping, Guid releaseId);

    }
}
