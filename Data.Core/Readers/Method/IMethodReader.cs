using System;
using System.Linq;
using Data.Core.Models.Class;
using Data.Core.Models.Method;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Method
{
    public interface IMethodReader : IMappingReader<IMethodMapping, IMethodVersionedMapping, IMethodCommittedMappingEntry, IMethodProposalMappingEntry>
    {
        IQueryable<IMethodMapping> GetByClassInLatestVersion(Guid classId);

        IQueryable<IMethodMapping> GetByClassInLatestRelease(Guid classId);

        IQueryable<IMethodMapping> GetByClassInLatestVersion(IClassMapping classMapping);

        IQueryable<IMethodMapping> GetByClassInLatestRelease(IClassMapping classMapping);

        IQueryable<IMethodMapping> GetByClassInVersion(Guid classId, Guid versionId);

        IQueryable<IMethodMapping> GetByClassInRelease(Guid classId, Guid releaseId);

        IQueryable<IMethodMapping> GetByClassInVersion(IClassMapping classMapping, Guid versionId);

        IQueryable<IMethodMapping> GetByClassInRelease(IClassMapping classMapping, Guid releaseId);

    }
}
