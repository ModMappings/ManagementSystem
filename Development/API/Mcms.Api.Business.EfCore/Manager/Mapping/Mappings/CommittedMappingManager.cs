using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.EFCore.QueryFilters.Factory;
using Mcms.Api.Business.Core.Manager.Mapping.Mappings;
using Mcms.Api.Business.Core.Stores;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Manager.Mapping.Mappings
{
    public class CommittedMappingManager
        : ICommittedMappingManager
    {

        private readonly ICallbackBasedQueryFilterFactory<CommittedMapping> _queryFilterFactory;
        private readonly IStore<CommittedMapping> _store;
        private readonly ILogger<CommittedMappingManager> _logger;

        public CommittedMappingManager(ICallbackBasedQueryFilterFactory<CommittedMapping> queryFilterFactory, IStore<CommittedMapping> store, ILogger<CommittedMappingManager> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }


        public async Task<IQueryable<CommittedMapping>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find committed mapping with id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindByType(ComponentType type)
        {
            _logger.LogDebug($"Attempting to find committed mapping with type: '{type}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.VersionedComponent.Component.Type == type)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindByComponentId(Guid componentId)
        {
            _logger.LogDebug($"Attempting to find committed mapping by component id: '{componentId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.VersionedComponent.Component.Id == componentId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindByVersionedComponentId(Guid versionedComponentId)
        {
            _logger.LogDebug($"Attempting to find committed mapping by versioned component id: '{versionedComponentId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.VersionedComponent.Id == versionedComponentId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindByMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find committed mapping by mapping type name regex: '{mappingTypeNameRegex}' and mapping regex: '{mappingRegex}'");

            var outputMappings = await FindByOutputMapping(mappingTypeNameRegex, mappingRegex);
            var inputMappings = await FindByInputMapping(mappingTypeNameRegex, mappingRegex);

            return outputMappings.Union(inputMappings);
        }

        public async Task<IQueryable<CommittedMapping>> FindByOutputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find committed mapping by mapping type name regex: '{mappingTypeNameRegex}' and output mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.OutputMapping, mappingRegex)
                )
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindByInputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find committed mapping by mapping type name regex: '{mappingTypeNameRegex}' and input mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.InputMapping, mappingRegex)
                )
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindByRelease(string releaseNameRegex)
        {
            _logger.LogDebug($"Attempting to find committed mapping by release name regex: '{releaseNameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.Releases.Any(rc => Regex.IsMatch(rc.Release.Name, releaseNameRegex)))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindByGameVersion(string gameVersionRegex)
        {
            _logger.LogDebug($"Attempting to find committed mapping by game version name regex: '{gameVersionRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => Regex.IsMatch(m.VersionedComponent.GameVersion.Name, gameVersionRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<CommittedMapping>> FindUsingFilter(Guid? id = null, ComponentType? type = null, Guid? componentId = null,
            Guid? versionedComponentId = null, string mappingTypeNameRegex = null, string mappingRegex = null,
            string releaseNameRegex = null, string gameVersionRegex = null)
        {
            _logger.LogDebug("Attempting to find committed mappings with filter data.");
            if (type != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => m.Id == id)
                );
            }

            if (type != null)
            {
                _logger.LogTrace($" > Type: '{type}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => m.VersionedComponent.Component.Type == type)
                );
            }

            if (componentId != null)
            {
                _logger.LogTrace($" > ComponentId: '{componentId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => m.VersionedComponent.Component.Id == componentId)
                );
            }

            if (versionedComponentId != null)
            {
                _logger.LogTrace($" > VersionedComponentId: '{versionedComponentId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => m.VersionedComponent.Id == versionedComponentId)
                );
            }

            if (mappingTypeNameRegex != null && mappingRegex != null)
            {
                _logger.LogTrace($" > MappingTypeNameRegex: '{mappingTypeNameRegex}'");
                _logger.LogTrace($" > MappingRegex: '{mappingRegex}'");

                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m =>
                        Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                        (
                            Regex.IsMatch(m.OutputMapping, mappingRegex) ||
                            Regex.IsMatch(m.InputMapping, mappingRegex)
                        )
                    )
                );
            }

            if (releaseNameRegex != null)
            {
                _logger.LogTrace($" > ReleaseNameRegex: '{releaseNameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => m.Releases.Any(rc => Regex.IsMatch(rc.Release.Name, releaseNameRegex)))
                );
            }

            if (gameVersionRegex != null)
            {
                _logger.LogTrace($" > GameVersionNameRegex: '{gameVersionRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(m => Regex.IsMatch(m.VersionedComponent.GameVersion.Name, gameVersionRegex))
                );
            }

            var filter = _queryFilterFactory.Build();
            return await _store.ReadAsync(filter);
        }

        public async Task CreateCommittedMapping(CommittedMapping committedMapping)
        {
            _logger.LogDebug($"Creating new committed mapping: '{committedMapping.Id}'");
            await _store.Create(committedMapping);
        }

        public async Task UpdateCommittedMapping(CommittedMapping committedMapping)
        {
            _logger.LogDebug($"Updating committed mapping: '{committedMapping.Id}'");
            await _store.Update(committedMapping);
        }

        public async Task DeleteCommittedMapping(CommittedMapping committedMapping)
        {
            _logger.LogDebug($"Deleting committed mapping: '{committedMapping.Id}'");
            await _store.Delete(committedMapping);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Saving committed mapping changes.");
            await _store.CommitChanges();
        }
    }
}
