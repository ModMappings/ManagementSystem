using System.Security.Claims;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.WebApi.Model;

namespace Data.WebApi.Services.Core
{
    public interface IUserResolvingService
    {
        Task<User> Get();
    }
}
