using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IMappingTypeReader
    {
        Task<MappingType> GetById(Guid id);

        Task<MappingType> GetByName(string name);

        Task<IQueryable<MappingType>> AsQueryable();

        Task<IQueryable<MappingType>> GetMadeBy(Guid userId);

        Task<IQueryable<MappingType>> GetMadeBy(User user);

        Task<IQueryable<MappingType>> GetMadeOn(DateTime date);

        Task<MappingType> GetLatest();
    }
}
