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
            return await (await AsQueryable()).FirstOrDefaultAsync(release => release.Id == id);
        }

        public async Task<Release> GetByName(string name)
        {
            return await (await AsQueryable()).FirstOrDefaultAsync(release => release.Name == name);
        }

        public async Task<IQueryable<Release>> AsQueryable()
        {
            return await Task.FromResult(_context.Releases.Include(r => r.Classes)
                .Include(r => r.Fields)
                .Include(r => r.Methods)
                .Include(r => r.Parameters)
                .Include(r => r.CreatedBy)
                .Include(r => r.MappingType));
        }

        public async Task<IQueryable<Release>> GetMadeBy(Guid userId)
        {
            return (await AsQueryable()).Where(release => release.CreatedBy.Id == userId);
        }

        public async Task<IQueryable<Release>> GetMadeBy(User user)
        {
            return (await AsQueryable()).Where(release => release.CreatedBy == user);
        }

        public async Task<IQueryable<Release>> GetMadeOn(DateTime date)
        {
            return (await AsQueryable()).Where(release => release.CreatedOn == date.Date);
        }

        public async Task<IQueryable<Release>> GetMadeForVersion(Guid id)
        {
            return (await AsQueryable()).Where(release => release.GameVersion.Id == id);
        }

        public async Task<IQueryable<Release>> GetMadeForVersion(GameVersion version)
        {
            return (await AsQueryable()).Where(release => release.GameVersion == version);
        }

        public async Task<IQueryable<Release>> GetMadeByForMappingType(Guid userId, Guid mappingType)
        {
            return await Task.FromResult((await AsQueryable()).Where(r =>
                r.CreatedBy.Id == userId && r.MappingType.Id == mappingType));
        }

        public async Task<IQueryable<Release>> GetMadeByForMappingType(User user, Guid mappingType)
        {
            return await GetMadeByForMappingType(user.Id, mappingType);
        }

        public async Task<IQueryable<Release>> GetMadeOnForMappingType(DateTime date, Guid mappingType)
        {

            return await Task.FromResult((await AsQueryable()).Where(r =>
                r.CreatedOn.Date == date.Date && r.MappingType.Id == mappingType));
        }

        public async Task<IQueryable<Release>> GetMadeForVersionForMappingType(Guid id, Guid mappingType)
        {
            return await Task.FromResult((await AsQueryable()).Where(r =>
                r.GameVersion.Id == id && r.MappingType.Id == mappingType));
        }

        public async Task<IQueryable<Release>> GetMadeForVersionForMappingType(GameVersion version, Guid mappingType)
        {
            return await GetMadeForVersionForMappingType(version.Id, mappingType);
        }

        public async Task<IQueryable<Release>> GetMadeByForMappingType(Guid userId, MappingType mappingType)
        {
            return await GetMadeByForMappingType(userId, mappingType.Id);
        }

        public async Task<IQueryable<Release>> GetMadeByForMappingType(User user, MappingType mappingType)
        {
            return await GetMadeByForMappingType(user.Id, mappingType.Id);
        }

        public async Task<IQueryable<Release>> GetMadeOnForMappingType(DateTime date, MappingType mappingType)
        {
            return await GetMadeOnForMappingType(date, mappingType.Id);
        }

        public async Task<IQueryable<Release>> GetMadeForVersionForMappingType(Guid id, MappingType mappingType)
        {
            return await GetMadeForVersionForMappingType(id, mappingType.Id);
        }

        public async Task<IQueryable<Release>> GetMadeForVersionForMappingType(GameVersion version, MappingType mappingType)
        {
            return await GetMadeForVersionForMappingType(version.Id, mappingType.Id);
        }

        public async Task<IQueryable<Release>> GetMadeForMappingType(Guid id)
        {
            return await Task.FromResult((await AsQueryable()).Where(r => r.MappingType.Id == id));
        }

        public async Task<IQueryable<Release>> GetMadeForMappingType(MappingType mapping)
        {
            return await GetMadeForMappingType(mapping.Id);
        }

        public async Task<Release> GetLatest()
        {
            return await (await AsQueryable()).OrderByDescending(release => release.CreatedOn).FirstOrDefaultAsync();
        }

        public async Task<Release> GetLatestForMapping(Guid id)
        {
            return await (await AsQueryable()).OrderByDescending(r => r.CreatedOn).Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Release> GetLatestForMapping(MappingType mappingType)
        {
            return await GetLatestForMapping(mappingType.Id);
        }

        public async Task Add(Release mapping)
        {
            if (mapping is Release release)
            {
                await _context.Releases.AddAsync(mapping);
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
