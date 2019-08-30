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

        Task<Release> GetLatest();
    }
}
