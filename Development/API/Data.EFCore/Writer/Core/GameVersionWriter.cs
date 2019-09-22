using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Core
{
    public class GameVersionWriter
        : IGameVersionWriter
    {
        private readonly MCMSContext _context;

        public GameVersionWriter(MCMSContext context)
        {
            _context = context;
        }

        public async Task<GameVersion> GetById(Guid id)
        {
            return await (await AsQueryable()).FirstOrDefaultAsync(version => version.Id == id);
        }

        public async Task<GameVersion> GetByName(string name)
        {
            return await (await AsQueryable()).FirstOrDefaultAsync(version => version.Name == name);
        }

        public async Task<IQueryable<GameVersion>> AsQueryable()
        {
            return await Task.FromResult(_context.GameVersions.Include(g => g.CreatedBy));
        }

        public async Task<IQueryable<GameVersion>> GetAllFullReleases()
        {
            return (await AsQueryable()).Where(version => !version.IsSnapshot && !version.IsPreRelease);
        }

        public async Task<IQueryable<GameVersion>> GetAllPreReleases()
        {
            return (await AsQueryable()).Where(version => version.IsPreRelease);
        }

        public async Task<IQueryable<GameVersion>> GetAllSnapshots()
        {
            return (await AsQueryable()).Where(version => version.IsSnapshot);
        }

        public async Task<GameVersion> GetLatest()
        {
            return await (await AsQueryable()).OrderByDescending(version => version.CreatedOn).FirstOrDefaultAsync();
        }

        public async Task Add(GameVersion mapping)
        {
            if (mapping is GameVersion gameVersion)
            {
                await this._context.GameVersions.AddAsync(gameVersion);
            }
        }

        public async Task Update(GameVersion mapping)
        {
            if (mapping is GameVersion gameVersion)
            {
                this._context.Entry(gameVersion).State = EntityState.Modified;
            }
        }

        public async Task SaveChanges()
        {
            await this._context.SaveChangesAsync();
        }
    }
}
