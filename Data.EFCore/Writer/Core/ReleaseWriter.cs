using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Core
{
    public class ReleaseWriter
        : IReleaseWriter
    {

        private readonly MCPContext _context;

        public ReleaseWriter(MCPContext context)
        {
            _context = context;
        }

        public async Task<Release> GetById(Guid id)
        {
            return await _context.Releases.FirstOrDefaultAsync(release => release.Id == id);
        }

        public async Task<Release> GetByName(string name)
        {
            return await _context.Releases.FirstOrDefaultAsync(release => release.Name == name);
        }

        public async Task<IQueryable<Release>> AsQueryable()
        {
            return _context.Releases;
        }

        public async Task<IQueryable<Release>> GetMadeBy(Guid userId)
        {
            return _context.Releases.Where(release => release.CreatedBy.Id == userId);
        }

        public async Task<IQueryable<Release>> GetMadeBy(User user)
        {
            return _context.Releases.Where(release => release.CreatedBy == user);
        }

        public async Task<IQueryable<Release>> GetMadeOn(DateTime date)
        {
            return _context.Releases.Where(release => release.CreatedOn == date.Date);
        }

        public async Task<IQueryable<Release>> GetMadeForVersion(Guid id)
        {
            return _context.Releases.Where(release => release.GameVersion.Id == id);
        }

        public async Task<IQueryable<Release>> GetMadeForVersion(GameVersion version)
        {
            return _context.Releases.Where(release => release.GameVersion == version);
        }

        public async Task<Release> GetLatest()
        {
            return await _context.Releases.OrderByDescending(release => release.CreatedOn).FirstOrDefaultAsync();
        }

        public async Task Add(Release mapping)
        {
            if (mapping is Release release)
            {
                await _context.Releases.AddAsync(release);
            }
        }

        public async Task Update(Release mapping)
        {
            if (mapping is Release release)
            {
                _context.Entry(release).State = EntityState.Modified;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
