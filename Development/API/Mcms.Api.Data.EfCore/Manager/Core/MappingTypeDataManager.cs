using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.EFCore.QueryFilters.Factory;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.Poco.Models.Core;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Manager.Core
{
    public class MappingTypeDataManager
        : IMappingTypeDataManager
    {

        private readonly ICallbackBasedQueryFilterFactory<MappingType> _queryFilterFactory;
        private readonly IStore<MappingType> _store;
        private readonly ILogger<MappingTypeDataManager> _logger;

        public MappingTypeDataManager(ICallbackBasedQueryFilterFactory<MappingType> queryFilterFactory, IStore<MappingType> store, ILogger<MappingTypeDataManager> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }

        public async Task<IQueryable<MappingType>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find mapping type by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<MappingType>> FindByName(string nameRegex)
        {
            _logger.LogDebug($"Attempting to find mapping type by name regex: '{nameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => Regex.IsMatch(m.Name, nameRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<MappingType>> FindByRelease(string releaseNameRegex)
        {
            _logger.LogDebug($"Attempting to find mapping type by release name regex: '{releaseNameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.Releases.Any(r => Regex.IsMatch(r.Name, releaseNameRegex)))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<MappingType>> FindUsingFilter(Guid? id = null, string nameRegex = null, string releaseNameRegex = null)
        {
            _logger.LogDebug("Attempting to find mapping type by filter data.");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => m.Id == id)
                );
            }

            if (nameRegex != null)
            {
                _logger.LogTrace($" > Name regex: '{nameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => Regex.IsMatch(m.Name, nameRegex))
                );
            }

            if (releaseNameRegex != null)
            {
                _logger.LogTrace($" > Release name regex: '{releaseNameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => m.Releases.Any(r => Regex.IsMatch(r.Name, releaseNameRegex)))
                );
            }

            var filter = _queryFilterFactory.Build();
            return await _store.ReadAsync(filter);
        }

        public async Task CreateMappingType(MappingType mappingType)
        {
            _logger.LogDebug($"Creating new mapping type: '{mappingType.Id}'");
            await _store.Create(mappingType);
        }

        public async Task UpdateMappingType(MappingType mappingType)
        {
            _logger.LogDebug($"Updating mapping type: '{mappingType.Id}'");
            await _store.Update(mappingType);
        }

        public async Task DeleteMappingType(MappingType mappingType)
        {
            _logger.LogDebug($"Deleting mapping type: '{mappingType.Id}'");
            await _store.Delete(mappingType);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;
        public async Task SaveChanges()
        {
            _logger.LogDebug("Attempting to save mapping type changes");
            await _store.CommitChanges();
        }
    }
}
