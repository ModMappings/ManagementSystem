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
        private readonly MCPContext _context;

        public GameVersionWriter(MCPContext context)
        {
            _context = context;
        }

        public async Task<GameVersion> GetById(Guid id)
        {
            return await _context.GameVersions.FirstOrDefaultAsync(version => version.Id == id);
        }

        public async Task<GameVersion> GetByName(string name)
        {
            return await _context.GameVersions.FirstOrDefaultAsync(version => version.Name == name);
        }

        public async Task<IQueryable<GameVersion>> AsQueryable()
        {
            return _context.GameVersions;
        }

        public async Task<IQueryable<GameVersion>> GetAllFullReleases()
        {
            return _context.GameVersions.Where(version => !version.IsSnapshot && !version.IsPreRelease);
        }

        public async Task<IQueryable<GameVersion>> GetAllPreReleases()
        {
            return _context.GameVersions.Where(version => version.IsPreRelease);
        }

        public async Task<IQueryable<GameVersion>> GetAllSnapshots()
        {
            return _context.GameVersions.Where(version => version.IsSnapshot);
        }

        public async Task<GameVersion> GetLatest()
        {
            return await _context.GameVersions.OrderByDescending(version => version.CreatedOn).FirstOrDefaultAsync();
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
