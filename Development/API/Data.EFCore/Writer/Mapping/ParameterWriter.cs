using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Writers.Mapping;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer.Mapping
{
    public class ParameterWriter
        : ComponentWriterBase, IParameterComponentWriter
    {
        public ParameterWriter(MCMSContext mcmsContext) : base(mcmsContext)
        {
        }

        public override async Task<IQueryable<Component>> AsQueryable()
        {
            return await Task.FromResult(MCMSContext.Components
                .Where(c => c.Type == ComponentType.CLASS)
                .Include(c => c.VersionedComponents)
                .Include("VersionedComponents.GameVersion")
                .Include("VersionedComponents.Component")
                .Include("VersionedComponents.Mappings")
                .Include("VersionedComponents.Proposals")
                .Include("VersionedComponents.Metadata")
                .Include("VersionedComponents.Mappings.Proposal")
                .Include("VersionedComponents.Mappings.Releases")
                .Include("VersionedComponents.Mappings.Releases.Release")
                .Include("VersionedComponents.Proposals.VotedFor")
                .Include("VersionedComponents.Proposals.VotedAgainst")
                .Include("VersionedComponents.Proposals.WentLiveWith")
                .Include("VersionedComponents.Metadata.VersionedComponent")
                .Include("VersionedComponents.Metadata.VersionedComponent.Component")
                .Include("VersionedComponents.Metadata.ParameterOf")
                .Include("VersionedComponents.Metadata.ParameterOf.VersionedComponent")
                .Include("VersionedComponents.Metadata.ParameterOf.VersionedComponent.Component")
                .Include("VersionedComponents.Metadata.Index"));
        }


        public async Task<IQueryable<Component>> GetByMethodInLatestGameVersion(Guid methodId)
        {
            var latestGameVersion =
                await MCMSContext.GameVersions.OrderByDescending(gv => gv.CreatedOn).FirstOrDefaultAsync();

            if (latestGameVersion == null)
                return new List<Component>().AsQueryable();

            return await GetByMethodInGameVersion(methodId, latestGameVersion);
        }

        public async Task<IQueryable<Component>> GetByMethodInGameVersion(Guid methodId, Guid gameVersionId)
        {
            var targetGameVersion =
                await MCMSContext.GameVersions.FirstOrDefaultAsync(gv => gv.Id == gameVersionId);

            if (targetGameVersion == null)
                return new List<Component>().AsQueryable();

            return await GetByMethodInGameVersion(methodId, targetGameVersion);
        }

        public async Task<IQueryable<Component>> GetByMethodInGameVersion(Guid methodId, GameVersion gameVersion)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedComponents.Any(vc =>
                vc.GameVersion == gameVersion &&
                ((vc.Metadata as ParameterMetadata).ParameterOf.VersionedComponent.Id == methodId
                ||
                (vc.Metadata as ParameterMetadata).ParameterOf.VersionedComponent.Component.Id == methodId)));
        }

        public async Task<IQueryable<Component>> GetByMethodInLatestRelease(Guid methodId)
        {
            var latestRelease =
                await MCMSContext.Releases.OrderByDescending(r => r.CreatedOn).FirstOrDefaultAsync();

            if (latestRelease == null)
                return new List<Component>().AsQueryable();

            return await GetByMethodInRelease(methodId, latestRelease);
        }

        public async Task<IQueryable<Component>> GetByMethodInRelease(Guid methodId, Guid releaseId)
        {
            var targetRelease =
                await MCMSContext.Releases.FirstOrDefaultAsync(r => r.Id == releaseId);

            if (targetRelease == null)
                return new List<Component>().AsQueryable();

            return await GetByMethodInRelease(methodId, targetRelease);
        }

        public async Task<IQueryable<Component>> GetByMethodInRelease(Guid methodId, Release release)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedComponents.Any(vc =>
                vc.Mappings.Any(m => m.Releases.Any(r => r.Release == release))&&
                ((vc.Metadata as ParameterMetadata).ParameterOf.VersionedComponent.Id == methodId
                 ||
                 (vc.Metadata as ParameterMetadata).ParameterOf.VersionedComponent.Component.Id == methodId)
                ));
        }
    }
}
