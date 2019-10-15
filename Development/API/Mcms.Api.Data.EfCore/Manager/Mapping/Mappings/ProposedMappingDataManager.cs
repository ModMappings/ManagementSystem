using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.EFCore.QueryFilters.Factory;
using Mcms.Api.Data.Core.Manager.Mapping.Mappings;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Manager.Mapping.Mappings
{
    public class ProposedMappingDataManager
        : IProposedMappingDataManager
    {

        private readonly ICallbackBasedQueryFilterFactory<ProposedMapping> _queryFilterFactory;
        private readonly IStore<ProposedMapping> _store;
        private readonly ILogger<ProposedMappingDataManager> _logger;

        public ProposedMappingDataManager(ICallbackBasedQueryFilterFactory<ProposedMapping> queryFilterFactory, IStore<ProposedMapping> store, ILogger<ProposedMappingDataManager> logger)
        {
            _queryFilterFactory = queryFilterFactory;
            _store = store;
            _logger = logger;
        }


        public async Task<IQueryable<ProposedMapping>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find proposed mapping with id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<ProposedMapping>> FindByType(ComponentType type)
        {
            _logger.LogDebug($"Attempting to find proposed mapping with type: '{type}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.VersionedComponent.Component.Type == type)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<ProposedMapping>> FindByComponentId(Guid componentId)
        {
            _logger.LogDebug($"Attempting to find proposed mapping by component id: '{componentId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.VersionedComponent.Component.Id == componentId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<ProposedMapping>> FindByVersionedComponentId(Guid versionedComponentId)
        {
            _logger.LogDebug($"Attempting to find proposed mapping by versioned component id: '{versionedComponentId}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => m.VersionedComponent.Id == versionedComponentId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<ProposedMapping>> FindByMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find proposed mapping by mapping type name regex: '{mappingTypeNameRegex}' and mapping regex: '{mappingRegex}'");

            var outputMappings = await FindByOutputMapping(mappingTypeNameRegex, mappingRegex);
            var inputMappings = await FindByInputMapping(mappingTypeNameRegex, mappingRegex);

            return outputMappings.Union(inputMappings);
        }

        public async Task<IQueryable<ProposedMapping>> FindByOutputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find proposed mapping by mapping type name regex: '{mappingTypeNameRegex}' and output mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.OutputMapping, mappingRegex)
                )
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<ProposedMapping>> FindByInputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find proposed mapping by mapping type name regex: '{mappingTypeNameRegex}' and input mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.InputMapping, mappingRegex)
                )
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<ProposedMapping>> FindByGameVersion(string gameVersionRegex)
        {
            _logger.LogDebug($"Attempting to find proposed mapping by game version name regex: '{gameVersionRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(m => Regex.IsMatch(m.VersionedComponent.GameVersion.Name, gameVersionRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<ProposedMapping>> FindUsingFilter(Guid? id = null, ComponentType? type = null, Guid? componentId = null,
            Guid? versionedComponentId = null, string mappingTypeNameRegex = null, string mappingRegex = null, string gameVersionRegex = null)
        {
            _logger.LogDebug("Attempting to find proposed mappings with filter data.");
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

        public async Task CreateProposedMapping(ProposedMapping proposedMapping)
        {
            _logger.LogDebug($"Creating new proposed mapping: '{proposedMapping.Id}'");
            await _store.Create(proposedMapping);
        }

        public async Task UpdateProposedMapping(ProposedMapping proposedMapping)
        {
            _logger.LogDebug($"Updating proposed mapping: '{proposedMapping.Id}'");
            await _store.Update(proposedMapping);
        }

        public async Task DeleteProposedMapping(ProposedMapping proposedMapping)
        {
            _logger.LogDebug($"Deleting proposed mapping: '{proposedMapping.Id}'");
            await _store.Delete(proposedMapping);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Saving proposed mapping changes.");
            await _store.CommitChanges();
        }
    }
}
