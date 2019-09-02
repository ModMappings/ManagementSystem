using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IReleaseReader
    {
        Task<Release> GetById(Guid id);

        Task<Release> GetByName(string name);

        Task<IQueryable<Release>> AsQueryable();

        Task<IQueryable<Release>> GetMadeBy(Guid userId);

        Task<IQueryable<Release>> GetMadeBy(User user);

        Task<IQueryable<Release>> GetMadeOn(DateTime date);

        Task<IQueryable<Release>> GetMadeForVersion(Guid id);

        Task<IQueryable<Release>> GetMadeForVersion(GameVersion version);

        Task<IQueryable<Release>> GetMadeByForMappingType(Guid userId, Guid mappingType);

        Task<IQueryable<Release>> GetMadeByForMappingType(User user, Guid mappingType);

        Task<IQueryable<Release>> GetMadeOnForMappingType(DateTime date, Guid mappingType);

        Task<IQueryable<Release>> GetMadeForVersionForMappingType(Guid id, Guid mappingType);

        Task<IQueryable<Release>> GetMadeForVersionForMappingType(GameVersion version, Guid mappingType);

        Task<IQueryable<Release>> GetMadeByForMappingType(Guid userId, MappingType mappingType);

        Task<IQueryable<Release>> GetMadeByForMappingType(User user, MappingType mappingType);

        Task<IQueryable<Release>> GetMadeOnForMappingType(DateTime date, MappingType mappingType);

        Task<IQueryable<Release>> GetMadeForVersionForMappingType(Guid id, MappingType mappingType);

        Task<IQueryable<Release>> GetMadeForVersionForMappingType(GameVersion version, MappingType mappingType);

        Task<IQueryable<Release>> GetMadeForMapping(Guid id);

        Task<IQueryable<Release>> GetMadeForMapping(MappingType mapping);

        Task<Release> GetLatest();
    }
}
