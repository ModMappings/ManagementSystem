using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Core
{
    public class UserWriter
        : IUserWriter
    {

        private readonly MCPContext _context;

        public UserWriter(MCPContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetByName(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Name == name);
        }

        public async Task<IQueryable<User>> AsQueryable()
        {
            return _context.Users;
        }

        public async Task<IQueryable<User>> GetAllEditers()
        {
            return _context.Users.Where(user => user.CanEdit);
        }

        public async Task<IQueryable<User>> GetAllReviewers()
        {
            return _context.Users.Where(user => user.CanReview);
        }

        public async Task<IQueryable<User>> GetAllCommitters()
        {
            return _context.Users.Where(user => user.CanCommit);
        }

        public async Task<IQueryable<User>> GetAllReleasers()
        {
            return _context.Users.Where(user => user.CanRelease);
        }

        public async Task<IQueryable<User>> GetAllGameVersionCreators()
        {
            return _context.Users.Where(user => user.CanCreateGameVersions);
        }

        public async Task Add(User mapping)
        {
            if (mapping is User user)
            {
                await _context.Users.AddAsync(user);
            }
        }

        public async Task Update(User mapping)
        {
            if (mapping is User user)
            {
                _context.Entry(user).State = EntityState.Modified;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
