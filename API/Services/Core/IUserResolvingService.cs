using System.Security.Claims;
using System.Threading.Tasks;
using API.Model;
using Data.Core.Models.Core;

namespace API.Services.Core
{
    public interface IUserResolvingService
    {
        Task<User> Get();
    }
}
