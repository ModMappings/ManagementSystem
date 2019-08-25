using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Field;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Field
{
    public interface IFieldReader : IMappingReader<IFieldMapping, IFieldVersionedMapping, IFieldCommittedMappingEntry, IFieldProposalMappingEntry>
    {
        Task<IQueryable<IFieldMapping>> GetByClassInLatestVersion(Guid classId);

        Task<IQueryable<IFieldMapping>> GetByClassInLatestRelease(Guid classId);

        Task<IQueryable<IFieldMapping>> GetByClassInLatestVersion(IClassMapping classMapping);

        Task<IQueryable<IFieldMapping>> GetByClassInLatestRelease(IClassMapping classMapping);

        Task<IQueryable<IFieldMapping>> GetByClassInVersion(Guid classId, Guid versionId);

        Task<IQueryable<IFieldMapping>> GetByClassInRelease(Guid classId, Guid releaseId);

        Task<IQueryable<IFieldMapping>> GetByClassInVersion(IClassMapping classMapping, Guid versionId);

        Task<IQueryable<IFieldMapping>> GetByClassInRelease(IClassMapping classMapping, Guid releaseId);
    }
}
