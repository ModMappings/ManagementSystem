using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping;
using Mcms.Api.Data.Poco.Models.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Writers.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Mapping
{
    public abstract class ComponentWriterBase
        : IComponentWriter
    {
        protected readonly MCMSContext MCMSContext;

        protected ComponentWriterBase(MCMSContext mcmsContext)
        {
            MCMSContext = mcmsContext;
        }

        public async Task<Component> GetById(Guid id)
        {
            var componentQueryable = await AsQueryable();

            return await componentQueryable.FirstOrDefaultAsync(c => c.Id == id);
        }

        public abstract Task<IQueryable<Component>> AsQueryable();

        public async Task<IQueryable<Component>> GetByLatestRelease()
        {
            var latestRelease = await MCMSContext.Releases.OrderByDescending(r => r.CreatedOn).FirstOrDefaultAsync();

            return await GetByRelease(latestRelease);
        }

        public async Task<IQueryable<Component>> GetByRelease(Guid releaseId)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c =>
                c.VersionedComponents.Any(v => v.Mappings.Any(m => m.Releases.Any(r => r.Release.Id == releaseId))));
        }

        public async Task<IQueryable<Component>> GetByRelease(string releaseName)
        {
            var namedRelease = await MCMSContext.Releases.FirstOrDefaultAsync(r => r.Name.Equals(releaseName));

            return await GetByRelease(namedRelease);
        }

        public async Task<IQueryable<Component>> GetByRelease(Release release)
        {
            return await GetByRelease(release.Id);
        }

        public async Task<IQueryable<Component>> GetByLatestVersion()
        {
            var latestVersion =
                await MCMSContext.GameVersions.OrderByDescending(v => v.CreatedOn).FirstOrDefaultAsync();

            return await GetByVersion(latestVersion);
        }

        public async Task<IQueryable<Component>> GetByVersion(Guid versionId)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c => c.VersionedComponents.Any(v => v.GameVersion.Id == versionId));
        }

        public async Task<IQueryable<Component>> GetByVersion(string versionName)
        {
            var namedVersion = await MCMSContext.GameVersions.FirstOrDefaultAsync(v => v.Name == versionName);

            return await GetByVersion(namedVersion);
        }

        public async Task<IQueryable<Component>> GetByVersion(GameVersion version)
        {
            return await GetByVersion(version.Id);
        }

        public async Task<IQueryable<Component>> GetByLatestMapping(string name)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c => c.VersionedComponents.Any(v =>
                v.Mappings.Any() && v.Mappings.OrderByDescending(m => m.CreatedOn).FirstOrDefault().InputMapping ==
                name));
        }

        public async Task<IQueryable<Component>> GetByMappingInVersion(string name, Guid versionId)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c => c.VersionedComponents.Any(v =>
                v.GameVersion.Id == versionId && v.Mappings.Any() &&
                v.Mappings.OrderByDescending(m => m.CreatedOn).FirstOrDefault().InputMapping ==
                name));
        }

        public async Task<IQueryable<Component>> GetByMappingInVersion(string name, GameVersion gameVersion)
        {
            return await GetByMappingInVersion(name, gameVersion.Id);
        }

        public async Task<IQueryable<Component>> GetByMappingInRelease(string name, Guid releaseId)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c => c.VersionedComponents.Any(v =>
                v.Mappings.Any() &&
                v.Mappings.OrderByDescending(m => m.CreatedOn).First().InputMapping == name &&
                v.Mappings.OrderByDescending(m => m.CreatedOn).First().Releases.Any(r => r.Id == releaseId)));
        }

        public async Task<IQueryable<Component>> GetByMappingInRelease(string name, Release release)
        {
            return await GetByMappingInRelease(name, release.Id);
        }

        public async Task<VersionedComponent> GetVersionedComponent(Guid id)
        {
            return await MCMSContext.VersionedComponents
                .Include(vc => vc.Component)
                .Include(vc => vc.Mappings)
                .Include(vc => vc.Metadata)
                .Include(vc => vc.Proposals)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<ProposedMapping> GetProposalMapping(Guid id)
        {
            return await MCMSContext.ProposalMappingEntries.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<CommittedMapping> GetLiveMapping(Guid id)
        {
            return await MCMSContext.LiveMappingEntries.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Add(Component mapping)
        {
            await MCMSContext.Components.AddAsync(mapping);
        }

        public async Task Update(Component mapping)
        {
            MCMSContext.Components.Update(mapping);
            await Task.CompletedTask;
        }

        public async Task Add(VersionedComponent mapping)
        {
            await MCMSContext.VersionedComponents.AddAsync(mapping);
        }

        public async Task Update(VersionedComponent mapping)
        {
            MCMSContext.VersionedComponents.Update(mapping);
            await Task.CompletedTask;
        }

        public async Task Add(CommittedMapping mapping)
        {
            await MCMSContext.LiveMappingEntries.AddAsync(mapping);
        }

        public async Task Update(CommittedMapping mapping)
        {
            MCMSContext.LiveMappingEntries.Update(mapping);
            await Task.CompletedTask;
        }

        public async Task Add(ProposedMapping proposedMapping)
        {
            await MCMSContext.ProposalMappingEntries.AddAsync(proposedMapping);
        }

        public async Task Update(ProposedMapping proposedMapping)
        {
            MCMSContext.ProposalMappingEntries.Update(proposedMapping);
            await Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            await MCMSContext.SaveChangesAsync();
        }
    }
}
