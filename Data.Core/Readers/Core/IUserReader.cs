using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IUserReader
    {
        Task<User> GetById(Guid id);

        Task<User> GetByName(string name);

        Task<IQueryable<User>> AsQueryable();

        Task<IQueryable<User>> GetAllEditers();

        Task<IQueryable<User>> GetAllReviewers();

        Task<IQueryable<User>> GetAllCommitters();

        Task<IQueryable<User>> GetAllReleasers();

        Task<IQueryable<User>> GetAllGameVersionCreators();
    }
}
