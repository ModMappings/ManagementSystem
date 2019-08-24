using System;
using System.Linq;
using Data.Core.Models.Class;
using Data.Core.Models.Field;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Field
{
    public interface IFieldReader : IMappingReader<IFieldMapping, IFieldVersionedMapping, IFieldCommittedMappingEntry, IFieldProposalMappingEntry>
    {
        IQueryable<IFieldMapping> GetByClassInLatestVersion(Guid classId);

        IQueryable<IFieldMapping> GetByClassInLatestRelease(Guid classId);

        IQueryable<IFieldMapping> GetByClassInLatestVersion(IClassMapping classMapping);

        IQueryable<IFieldMapping> GetByClassInLatestRelease(IClassMapping classMapping);

        IQueryable<IFieldMapping> GetByClassInVersion(Guid classId, Guid versionId);

        IQueryable<IFieldMapping> GetByClassInRelease(Guid classId, Guid releaseId);

        IQueryable<IFieldMapping> GetByClassInVersion(IClassMapping classMapping, Guid versionId);

        IQueryable<IFieldMapping> GetByClassInRelease(IClassMapping classMapping, Guid releaseId);
    }
}
