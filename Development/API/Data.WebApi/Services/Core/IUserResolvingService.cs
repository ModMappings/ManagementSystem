using System.Security.Claims;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core;
using Data.WebApi.Model;

namespace Data.WebApi.Services.Core
{
    public interface IUserResolvingService
    {
        Task<User> Get();
    }
}
