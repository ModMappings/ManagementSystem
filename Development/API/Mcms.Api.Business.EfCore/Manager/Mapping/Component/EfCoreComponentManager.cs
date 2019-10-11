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
    public class EfCoreComponentManager
        : IComponentManager
    {
        private readonly IStore<Mcms.Api.Data.Poco.Models.Mapping.Component.Component> _store;
        private readonly ICallbackBasedQueryFilterFactory<Mcms.Api.Data.Poco.Models.Mapping.Component.Component> _queryFilterFactory;
        private readonly ILogger<EfCoreComponentManager> _logger;

        public EfCoreComponentManager(IStore<Mcms.Api.Data.Poco.Models.Mapping.Component.Component> store, ICallbackBasedQueryFilterFactory<Mcms.Api.Data.Poco.Models.Mapping.Component.Component> queryFilterFactory, ILogger<EfCoreComponentManager> logger)
        {
            _store = store;
            _queryFilterFactory = queryFilterFactory;
            _logger = logger;
        }


        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindById(Guid id)
        {
            _logger.LogDebug($"Attempting to find component by id: '{id}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.Id == id)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindByType(ComponentType type)
        {
            _logger.LogDebug($"Attempting to find component by type: '{type}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.Type == type)
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindByMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find component by mapping type name regex: '{mappingTypeNameRegex}' and mapping regex: '{mappingRegex}'");
            var outputMappingQuery = await this.FindByOutputMapping(mappingTypeNameRegex, mappingRegex);
            var inputMappingQuery = await this.FindByInputMapping(mappingTypeNameRegex, mappingRegex);

            return outputMappingQuery.Union(inputMappingQuery);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindByOutputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find component by mapping type name regex: '{mappingTypeNameRegex}' and output mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.VersionedComponents.Any(vc => vc.Mappings.Any(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.OutputMapping, mappingRegex))))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindByInputMapping(string mappingTypeNameRegex, string mappingRegex)
        {
            _logger.LogDebug($"Attempting to find component by mapping type name regex: '{mappingTypeNameRegex}' and input mapping regex: '{mappingRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.VersionedComponents.Any(vc => vc.Mappings.Any(m =>
                    Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                    Regex.IsMatch(m.InputMapping, mappingRegex))))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindByRelease(string releaseNameRegex)
        {
            _logger.LogDebug($"Attempting to find component by release name regex: '{releaseNameRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c => c.VersionedComponents.Any(vc => vc.Mappings.Any(m => m.Releases.Any(rc => Regex.IsMatch(rc.Release.Name, releaseNameRegex)))))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindByGameVersion(string gameVersionRegex)
        {
            _logger.LogDebug($"Attempting to find component by game version name regex: '{gameVersionRegex}'");
            var filter = _queryFilterFactory.AddCallback(
                (q) => q.Where(c =>
                    c.VersionedComponents.Any(vc => Regex.IsMatch(vc.GameVersion.Name, gameVersionRegex)))
            ).Build();

            return await _store.ReadAsync(filter);
        }

        public async Task<IQueryable<Mcms.Api.Data.Poco.Models.Mapping.Component.Component>> FindUsingFilter(Guid? id = null, ComponentType? type = null, string mappingTypeNameRegex = null,
            string mappingRegex = null, string releaseNameRegex = null, string gameVersionRegex = null)
        {
            _logger.LogDebug($"Attempting to find component by filter data.");
            if (id != null)
            {
                _logger.LogTrace($" > Id: '{id}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => c.Id == id)
                );
            }

            if (type != null)
            {
                _logger.LogTrace($" > Type: '{type}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => c.Type == type)
                );
            }

            if (mappingTypeNameRegex != null && mappingRegex != null)
            {
                _logger.LogTrace($" > MappingTypeNameRegex: '{mappingTypeNameRegex}'");
                _logger.LogTrace($" > MappingRegex: '{mappingRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => c.VersionedComponents.Any(vc => vc.Mappings.Any(m =>
                        Regex.IsMatch(m.MappingType.Name, mappingTypeNameRegex) &&
                        (
                            Regex.IsMatch(m.OutputMapping, mappingRegex) ||
                            Regex.IsMatch(m.InputMapping, mappingRegex)
                        )
                    )))
                );
            }

            if (releaseNameRegex != null)
            {
                _logger.LogTrace($" > ReleaseNameRegex: '{releaseNameRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c => c.VersionedComponents.Any(vc =>
                        vc.Mappings.Any(m => m.Releases.Any(rc => Regex.IsMatch(rc.Release.Name, releaseNameRegex)))))
                );
            }

            if (gameVersionRegex != null)
            {
                _logger.LogTrace($" > GameVersionRegex: '{gameVersionRegex}'");
                _queryFilterFactory.AddCallback(
                    (q) => q.Where(c =>
                        c.VersionedComponents.Any(vc => Regex.IsMatch(vc.GameVersion.Name, gameVersionRegex)))
                );
            }

            var filter = _queryFilterFactory.Build();

            return await _store.ReadAsync(filter);
        }

        public async Task CreateComponent(Mcms.Api.Data.Poco.Models.Mapping.Component.Component component)
        {
            _logger.LogDebug("Creating new component: " + component.Id);
            await _store.Create(component);
        }

        public async Task UpdateComponent(Mcms.Api.Data.Poco.Models.Mapping.Component.Component component)
        {
            _logger.LogDebug("Updating component: " + component.Id);
            await _store.Update(component);
        }

        public async Task DeleteComponent(Mcms.Api.Data.Poco.Models.Mapping.Component.Component component)
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
