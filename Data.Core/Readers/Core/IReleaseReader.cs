using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IReleaseReader
    {
        Task<IRelease> GetById(Guid id);

        Task<IRelease> GetByName(string name);

        Task<IQueryable<IRelease>> AsQueryable();

        Task<IQueryable<IRelease>> GetMadeBy(Guid userId);

        Task<IQueryable<IRelease>> GetMadeBy(IUser user);

        Task<IQueryable<IRelease>> GetMadeOn(DateTime date);

        Task<IQueryable<IGameVersion>> GetMadeForVersion(Guid id);

        Task<IQueryable<IGameVersion>> GetMadeForVersion(IGameVersion version);
    }
}
