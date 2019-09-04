using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Writers.Core;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Mapping
{
    public abstract class ComponentWriterBase
        : IComponentWriter
    {
        protected readonly MCPContext McpContext;

        protected ComponentWriterBase(MCPContext mcpContext)
        {
            McpContext = mcpContext;
        }

        public async Task<Component> GetById(Guid id)
        {
            var componentQueryable = await AsQueryable();

            return await componentQueryable.FirstOrDefaultAsync(c => c.Id == id);
        }

        public abstract Task<IQueryable<Component>> AsQueryable();

        public async Task<IQueryable<Component>> GetByLatestRelease()
        {
            var latestRelease = await McpContext.Releases.OrderByDescending(r => r.CreatedOn).FirstOrDefaultAsync();

            return await GetByRelease(latestRelease);
        }

        public async Task<IQueryable<Component>> GetByRelease(Guid releaseId)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c =>
                c.VersionedMappings.Any(v => v.Mappings.Any(m => m.Releases.Any(r => r.Release.Id == releaseId))));
        }

        public async Task<IQueryable<Component>> GetByRelease(string releaseName)
        {
            var namedRelease = await McpContext.Releases.FirstOrDefaultAsync(r => r.Name.Equals(releaseName));

            return await GetByRelease(namedRelease);
        }

        public async Task<IQueryable<Component>> GetByRelease(Release release)
        {
            return await GetByRelease(release.Id);
        }

        public async Task<IQueryable<Component>> GetByLatestVersion()
        {
            var latestVersion =
                await McpContext.GameVersions.OrderByDescending(v => v.CreatedOn).FirstOrDefaultAsync();

            return await GetByVersion(latestVersion);
        }

        public async Task<IQueryable<Component>> GetByVersion(Guid versionId)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c => c.VersionedMappings.Any(v => v.GameVersion.Id == versionId));
        }

        public async Task<IQueryable<Component>> GetByVersion(string versionName)
        {
            var namedVersion = await McpContext.GameVersions.FirstOrDefaultAsync(v => v.Name == versionName);

            return await GetByVersion(namedVersion);
        }

        public async Task<IQueryable<Component>> GetByVersion(GameVersion version)
        {
            return await GetByVersion(version.Id);
        }

        public async Task<IQueryable<Component>> GetByLatestMapping(string name)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c => c.VersionedMappings.Any(v =>
                v.Mappings.Any() && v.Mappings.OrderByDescending(m => m.CreatedOn).FirstOrDefault().InputMapping ==
                name));
        }

        public async Task<IQueryable<Component>> GetByMappingInVersion(string name, Guid versionId)
        {
            var componentQueryable = await AsQueryable();

            return componentQueryable.Where(c => c.VersionedMappings.Any(v =>
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

            return componentQueryable.Where(c => c.VersionedMappings.Any(v =>
                v.Mappings.Any() &&
                v.Mappings.OrderByDescending(m => m.CreatedOn).First().InputMapping == name &&
                v.Mappings.OrderByDescending(m => m.CreatedOn).First().Releases.Any(r => r.Id == releaseId)));
        }

        public async Task<IQueryable<Component>> GetByMappingInRelease(string name, Release release)
        {
            return await GetByMappingInRelease(name, release.Id);
        }

        public async Task<VersionedComponent> GetVersionedMapping(Guid id)
        {
            return await McpContext.VersionedComponents.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<ProposalMappingEntry> GetProposalMapping(Guid id)
        {
            return await McpContext.ProposalMappingEntries.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<LiveMappingEntry> GetLiveMapping(Guid id)
        {
            return await McpContext.LiveMappingEntries.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Add(Component mapping)
        {
            await McpContext.Components.AddAsync(mapping);
        }

        public async Task Update(Component mapping)
        {
            McpContext.Components.Update(mapping);
            await Task.CompletedTask;
        }

        public async Task Add(VersionedComponent mapping)
        {
            await McpContext.VersionedComponents.AddAsync(mapping);
        }

        public async Task Update(VersionedComponent mapping)
        {
            McpContext.VersionedComponents.Update(mapping);
            await Task.CompletedTask;
        }

        public async Task Add(LiveMappingEntry mapping)
        {
            await McpContext.LiveMappingEntries.AddAsync(mapping);
        }

        public async Task Update(LiveMappingEntry mapping)
        {
            McpContext.LiveMappingEntries.Update(mapping);
            await Task.CompletedTask;
        }

        public async Task Add(ProposalMappingEntry mapping)
        {
            await McpContext.ProposalMappingEntries.AddAsync(mapping);
        }

        public async Task Update(ProposalMappingEntry mapping)
        {
            McpContext.ProposalMappingEntries.Update(mapping);
            await Task.CompletedTask;
        }

        public async Task SaveChanges()
        {
            await McpContext.SaveChangesAsync();
        }
    }
}
