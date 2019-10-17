using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.EfCore.QueryFilters.Factory;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Microsoft.Extensions.Logging;

namespace Mcms.Api.Data.EfCore.Manager.Core
{
    public class ReleaseDataManager
        : IReleaseDataManager
    {

        private readonly ICallbackBasedQueryFilterFactory<Release> _queryFilterFactory;
        private readonly IStore<Release> _store;
        private readonly ILogger<ReleaseDataManager> _logger;

        public ReleaseDataManager(ICallbackBasedQueryFilterFactory<Release> queryFilterFactory, IStore<Release> store, ILogger<ReleaseDataManager> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }

        public async Task<IQueryable<Release>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find releases by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(r => r.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Release>> FindByName(string nameRegex)
        {
            _logger.LogDebug($"Attempting to find releases by name regex: '{nameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(r => Regex.IsMatch(r.Name, nameRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Release>> FindByMappingType(string mappingTypeNameRegex)
        {
            _logger.LogDebug($"Attempting to find releases by mapping type name regex: '{mappingTypeNameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(r => Regex.IsMatch(r.MappingType.Name, mappingTypeNameRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Release>> FindByGameVersion(string gameVersionRegex)
        {
            _logger.LogDebug($"Attempting to find releases by game version name regex: '{gameVersionRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(r => Regex.IsMatch(r.GameVersion.Name, gameVersionRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Release>> FindUsingFilter(Guid? id = null, string nameRegex = null, string mappingTypeNameRegex = null,
            string gameVersionRegex = null)
        {
            _logger.LogDebug("Attempting to find releases by filter data.");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(r => r.Id == id)
                );
            }

            if (nameRegex != null)
            {
                _logger.LogTrace($" > Name regex: '{nameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(r => Regex.IsMatch(r.Name, nameRegex))
                );
            }

            if (mappingTypeNameRegex != null)
            {
                _logger.LogTrace($" > Mapping type name regex: '{mappingTypeNameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(r => Regex.IsMatch(r.MappingType.Name, mappingTypeNameRegex))
                );
            }

            if (gameVersionRegex != null)
            {
                _logger.LogTrace($" > Game version name regex: '{gameVersionRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(r => Regex.IsMatch(r.GameVersion.Name, gameVersionRegex))
                );
            }

            var filter = _queryFilterFactory.Build();
            return await _store.ReadAsync(filter);
        }

        public async Task CreateRelease(Release release)
        {
            _logger.LogDebug($"Creating new release: '{release.Id}'");
            await _store.Create(release);
        }

        public async Task UpdateRelease(Release release)
        {
            _logger.LogDebug($"Updating release: '{release.Id}'");
            await _store.Update(release);
        }

        public async Task DeleteRelease(Release release)
        {
            _logger.LogDebug($"Deleting release: '{release.Id}'");
            await _store.Delete(release);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Attempting to save changes to releases.");
            await _store.CommitChanges();
        }
    }
}
