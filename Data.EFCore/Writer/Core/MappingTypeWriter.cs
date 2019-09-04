using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Core
{
    public class MappingTypeWriter
        : IMappingTypeWriter
    {

        private readonly MCPContext _mcpContext;

        public MappingTypeWriter(MCPContext mcpContext)
        {
            _mcpContext = mcpContext;
        }

        public async Task<MappingType> GetById(Guid id)
        {
            return await (await AsQueryable()).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MappingType> GetByName(string name)
        {
            return await (await AsQueryable()).FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task<IQueryable<MappingType>> AsQueryable()
        {
            return await Task.FromResult(_mcpContext.MappingTypes
                .Include(m => m.CreatedBy)
                .Include(m => m.Releases));
        }

        public async Task<IQueryable<MappingType>> GetMadeBy(Guid userId)
        {
            return (await AsQueryable()).Where(m => m.CreatedBy.Id == userId);
        }

        public async Task<IQueryable<MappingType>> GetMadeBy(User user)
        {
            return await GetMadeBy(user.Id);
        }

        public async Task<IQueryable<MappingType>> GetMadeOn(DateTime date)
        {
            return (await AsQueryable()).Where(m => m.CreatedOn == date);
        }

        public async Task SaveChanges()
        {
            await _mcpContext.SaveChangesAsync();
        }

        public async Task Add(MappingType mapping)
        {
            await _mcpContext.MappingTypes.AddAsync(mapping);
        }

        public async Task Update(MappingType mapping)
        {
            _mcpContext.MappingTypes.Update(mapping);
            await Task.CompletedTask;
        }
    }
}
