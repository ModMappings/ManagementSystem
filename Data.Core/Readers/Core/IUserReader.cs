using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;

namespace Data.Core.Readers.Core
{
    public interface IUserReader
    {
        Task<IUser> GetById { get; set; }

        Task<IUser> GetByName { get; set; }

        Task<IQueryable<IUser>> AsQueryable();

        Task<IQueryable<IUser>> GetAllEditers();

        Task<IQueryable<IUser>> GetAllReviewers();

        Task<IQueryable<IUser>> GetAllCommitters();

        Task<IQueryable<IUser>> GetAllReleasers();

        Task<IQueryable<IUser>> GetAllGameVersionCreators();
    }
}
