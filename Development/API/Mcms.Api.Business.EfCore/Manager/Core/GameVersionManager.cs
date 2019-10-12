using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.EFCore.QueryFilters.Factory;
using Mcms.Api.Business.Core.Manager.Core;
using Mcms.Api.Business.Core.Stores;
using Mcms.Api.Data.Poco.Models.Core;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Manager.Core
{
    public class GameVersionManager
        : IGameVersionManager
    {

        private readonly ICallbackBasedQueryFilterFactory<GameVersion> _queryFilterFactory;
        private readonly IStore<GameVersion> _store;
        private readonly ILogger<GameVersionManager> _logger;

        public GameVersionManager(ICallbackBasedQueryFilterFactory<GameVersion> queryFilterFactory, IStore<GameVersion> store, ILogger<GameVersionManager> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }

        public async Task<IQueryable<GameVersion>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find game version by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(g => g.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<GameVersion>> FindByName(string nameRegex)
        {
            _logger.LogDebug($"Attempting to find game version by regex name: '{nameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(g => Regex.IsMatch(g.Name, nameRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<GameVersion>> FindUsingFilter(Guid? id = null, string nameRegex = null)
        {
            _logger.LogDebug("Attempting to find game version by filter data.");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(g => g.Id == id)
                );
            }

            if (nameRegex != null)
            {
                _logger.LogTrace($" > Name regex: '{nameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(g => Regex.IsMatch(g.Name, nameRegex))
                );
            }

            var filter = _queryFilterFactory.Build();
            return await _store.ReadAsync(filter);
        }

        public async Task CreateGameVersion(GameVersion gameVersion)
        {
            _logger.LogDebug($"Creating new game version: '{gameVersion.Id}'");
            await _store.Create(gameVersion);
        }

        public async Task UpdateGameVersion(GameVersion gameVersion)
        {
            _logger.LogDebug($"Updating new game version: '{gameVersion.Id}'");
            await _store.Update(gameVersion);
        }

        public async Task DeleteGameVersion(GameVersion gameVersion)
        {
            _logger.LogDebug($"Deleting new game version: '{gameVersion.Id}'");
            await _store.Delete(gameVersion);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Attempting to save pending game version changes.");
            await _store.CommitChanges();
        }
    }
}
