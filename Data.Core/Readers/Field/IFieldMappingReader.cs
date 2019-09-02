using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Field;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Field
{
    public interface IFieldMappingReader : INoneUniqueNamedMappingReader<FieldMapping, FieldVersionedMapping, FieldCommittedMappingEntry, FieldProposalMappingEntry, FieldReleaseMember>
    {
        Task<IQueryable<FieldMapping>> GetByClassInLatestVersion(Guid classId);

        Task<IQueryable<FieldMapping>> GetByClassInLatestRelease(Guid classId);

        Task<IQueryable<FieldMapping>> GetByClassInLatestVersion(ClassMapping classMapping);

        Task<IQueryable<FieldMapping>> GetByClassInLatestRelease(ClassMapping classMapping);

        Task<IQueryable<FieldMapping>> GetByClassInVersion(Guid classId, Guid versionId);

        Task<IQueryable<FieldMapping>> GetByClassInRelease(Guid classId, Guid releaseId);

        Task<IQueryable<FieldMapping>> GetByClassInVersion(ClassMapping classMapping, Guid versionId);

        Task<IQueryable<FieldMapping>> GetByClassInRelease(ClassMapping classMapping, Guid releaseId);
    }
}
