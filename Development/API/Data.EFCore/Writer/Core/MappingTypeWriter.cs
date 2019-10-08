using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Writers.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Core
{
    public class MappingTypeWriter
        : IMappingTypeWriter
    {

        private readonly MCMSContext _mcmsContext;

        public MappingTypeWriter(MCMSContext mcmsContext)
        {
            _mcmsContext = mcmsContext;
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
            return await Task.FromResult(_mcmsContext.MappingTypes
                .Include(m => m.CreatedBy)
                .Include(m => m.Releases));
        }

        public async Task<IQueryable<MappingType>> GetMadeBy(Guid userId)
        {
            return (await AsQueryable()).Where(m => m.CreatedBy == userId);
        }

        public async Task<IQueryable<MappingType>> GetMadeOn(DateTime date)
        {
            return (await AsQueryable()).Where(m => m.CreatedOn == date);
        }

        public async Task SaveChanges()
        {
            await _mcmsContext.SaveChangesAsync();
        }

        public async Task Add(MappingType mapping)
        {
            await _mcmsContext.MappingTypes.AddAsync(mapping);
        }

        public async Task Update(MappingType mapping)
        {
            _mcmsContext.MappingTypes.Update(mapping);
            await Task.CompletedTask;
        }
    }
}
