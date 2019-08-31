using System.Threading.Tasks;
using API.Services.Core;
using Data.Core.Models.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Services.UserResolving
{
    /// <summary>
    /// Dummy retrieval service, gets the first (and during debugging the only user that is enabled).
    /// </summary>
    public class DummyUserResolvingService : IUserResolvingService
    {

        private readonly MCPContext _context;

        public DummyUserResolvingService(MCPContext context)
        {
            _context = context;
        }

        public async Task<User> Get()
        {
            return await _context.Users.FirstOrDefaultAsync();
        }
    }
}
