using System.Threading.Tasks;
using Mcms.Api.WebApi.Http.Model;

namespace Mcms.Api.WebApi.Http.Services.Core
{
    public interface IUserResolvingService
    {
        Task<User> Get();
    }
}
