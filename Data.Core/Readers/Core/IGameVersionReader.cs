using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IGameVersionReader
    {
        Task<IGameVersion> GetById(Guid id);

        Task<IGameVersion> GetByName(string name);

        Task<IQueryable<IGameVersion>> AsQueryable();

        Task<IQueryable<IGameVersion>> GetAllReleases();

        Task<IQueryable<IGameVersion>> GetAllReleaseCandidates();

        Task<IQueryable<IGameVersion>> GetAllSnapshots();
    }
}
