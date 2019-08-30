using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IGameVersionReader
    {
        Task<GameVersion> GetById(Guid id);

        Task<GameVersion> GetByName(string name);

        Task<IQueryable<GameVersion>> AsQueryable();

        Task<IQueryable<GameVersion>> GetAllFullReleases();

        Task<IQueryable<GameVersion>> GetAllPreReleases();

        Task<IQueryable<GameVersion>> GetAllSnapshots();

        Task<GameVersion> GetLatest();
    }
}
