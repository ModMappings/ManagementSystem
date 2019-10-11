using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.EFCore.QueryFilters.Factory;
using Mcms.Api.Business.Core.Manager.Mapping.Component;
using Mcms.Api.Business.Core.Stores;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Manager.Mapping.Component
{
    public class EfCoreVersionedComponentManager
        : IVersionedComponentManager
    {
        private readonly IStore<VersionedComponent> _store;
        private readonly ICallbackBasedQueryFilterFactory<VersionedComponent> _queryFilterFactory;
        private readonly ILogger<EfCoreVersionedComponentManager> _logger;

        public EfCoreVersionedComponentManager(IStore<VersionedComponent> store, ICallbackBasedQueryFilterFactory<VersionedComponent> queryFilterFactory, ILogger<EfCoreVersionedComponentManager> logger)
        {
            _store = store;
            _queryFilterFactory = queryFilterFactory;
            _logger = logger;
        }


        public async Task<IQueryable<VersionedComponent>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find versioned component by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VersionedComponent>> FindByType(ComponentType type)
        {
            _logger.LogDebug($"Attempting to find versioned component by type: '{type}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.Component.Type == type)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VersionedComponent>> FindByComponentId(Guid componentId)
        {
            _logger.LogDebug($"Attempting to find versioned component by component id {componentId}");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(vc => vc.Component.Id == componentId)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VersionedComponent>> FindByMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find versioned component by mapping type name regex: '{mappingTypeNameRegex}' and mapping regex: '{mappingRegex}'");
            var outputMappingQuery = await FindByOutputMapping(mappingTypeNameRegex, mappingRegex);
            var inputMappingQuery = await FindByInputMapping(mappingTypeNameRegex, mappingRegex);

            return outputMappingQuery.Union(inputMappingQuery);
        }

        public async Task<IQueryable<VersionedComponent>> FindByOutputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find versioned component by mapping type name regex: '{mappingTypeNameRegex}' and output mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(vc => vc.Mappings.Any(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.OutputMapping, mappingRegex)))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VersionedComponent>> FindByInputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find versioned component by mapping type name regex: '{mappingTypeNameRegex}' and input mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(vc => vc.Mappings.Any(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.InputMapping, mappingRegex)))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VersionedComponent>> FindByRelease(string releaseNameRegex)
        {
            _logger.LogDebug($"Attempting to find versioned component by release name regex: '{releaseNameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(vc => vc.Mappings.Any(m => m.Releases.Any(rc => Regex.IsMatch(rc.Release.Name, releaseNameRegex))))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VersionedComponent>> FindByGameVersion(string gameVersionRegex)
        {
            _logger.LogDebug($"Attempting to find versioned component by game version name regex: '{gameVersionRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(vc => Regex.IsMatch(vc.GameVersion.Name, gameVersionRegex))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<VersionedComponent>> FindUsingFilter(Guid? id = null, ComponentType? type = null, Guid? componentId = null,
            string mappingTypeNameRegex = null, string mappingRegex = null, string releaseNameRegex = null,
            string gameVersionRegex = null)
        {
            _logger.LogDebug("Attempting to find component by filter data.");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(vc => vc.Id == id)
                );
            }

            if (type != null)
            {
                _logger.LogTrace($" > Type: '{type}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(vc => vc.Component.Type == type)
                );
            }

            if (componentId != null)
            {
                _logger.LogTrace($" > ComponentId: '{componentId}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(vc => vc.Component.Id == componentId)
                );
            }

            if (mappingTypeNameRegex != null && mappingRegex != null)
            {
                _logger.LogTrace($" > MappingTypeNameRegex: '{mappingTypeNameRegex}'");
                _logger.LogTrace($" > MappingRegex: '{mappingRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(vc => vc.Mappings.Any(m =>
                        Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                        (
                            Regex.IsMatch(m.OutputMapping, mappingRegex) ||
                            Regex.IsMatch(m.InputMapping, mappingRegex)
                        )
                    ))
                );
            }

            if (releaseNameRegex != null)
            {
                _logger.LogTrace($" > ReleaseNameRegex: '{releaseNameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(vc =>
                        vc.Mappings.Any(m => m.Releases.Any(rc => Regex.IsMatch(rc.Release.Name, releaseNameRegex))))
                );
            }

            if (gameVersionRegex != null)
            {
                _logger.LogTrace($" > GameVersionRegex: '{gameVersionRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(vc => Regex.IsMatch(vc.GameVersion.Name, gameVersionRegex))
                );
            }

            var filter = _queryFilterFactory.Build();

            return await _store.ReadAsync(filter);
        }

        public async Task CreateVersionedComponent(VersionedComponent component)
        {
            _logger.LogDebug("Creating new component: " + component.Id);
            await _store.Create(component);
        }

        public async Task UpdateVersionedComponent(VersionedComponent component)
        {
            _logger.LogDebug("Updating component: " + component.Id);
            await _store.Update(component);
        }

        public async Task DeleteVersionedComponent(VersionedComponent component)
        {
            _logger.LogDebug("Deleting component: " + component.Id);
            await _store.Delete(component);
        }

        public bool HasPendingChanges => _store.HasPendingChanges;

        public async Task SaveChanges()
        {
            _logger.LogDebug("Saving component changes.");
            await _store.CommitChanges();
        }
    }
}
