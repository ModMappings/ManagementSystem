using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Metadata;
using Data.Core.Writers.Mapping;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Data.EFCore.Writer.Mapping
{
    public class ClassWriter
        : ComponentWriterBase, IClassComponentWriter
    {
        public ClassWriter(MCMSContext mcmsContext) : base(mcmsContext)
        {
        }

        public override async Task<IQueryable<Component>> AsQueryable()
        {
            return await Task.FromResult(MCMSContext.Components
                .Where(c => c.Type == ComponentType.CLASS)
                .Include(c => c.VersionedMappings)
                .Include("VersionedMappings.GameVersion")
                .Include("VersionedMappings.CreatedBy")
                .Include("VersionedMappings.Component")
                .Include("VersionedMappings.Mappings")
                .Include("VersionedMappings.Proposals")
                .Include("VersionedMappings.Metadata")
                .Include("VersionedMappings.GameVersion.User")
                .Include("VersionedMappings.Mappings.Proposal")
                .Include("VersionedMappings.Mappings.Releases")
                .Include("VersionedMappings.Mappings.Releases.Release")
                .Include("VersionedMappings.Proposals.ProposedBy")
                .Include("VersionedMappings.Proposals.VotedFor")
                .Include("VersionedMappings.Proposals.VotedAgainst")
                .Include("VersionedMappings.Proposals.ClosedBy")
                .Include("VersionedMappings.Proposals.WentLiveWith")
                .Include("VersionedMappings.Metadata.Component")
                .Include("VersionedMappings.Metadata.Outer")
                .Include("VersionedMappings.Metadata.InheritsFrom"));
        }

        public async Task<IQueryable<Component>> GetByPackageInLatestGameVersion(string packagePattern)
        {
            var latestGameVersion =
                await MCMSContext.GameVersions.OrderByDescending(gv => gv.CreatedOn).FirstOrDefaultAsync();

            if (latestGameVersion == null)
                return new List<Component>().AsQueryable();

            return await GetByPackageInGameVersion(packagePattern, latestGameVersion);
        }

        public async Task<IQueryable<Component>> GetByPackageInGameVersion(string packagePattern, Guid gameVersionId)
        {
            var targetGameVersion =
                await MCMSContext.GameVersions.FirstOrDefaultAsync(gv => gv.Id == gameVersionId);

            if (targetGameVersion == null)
                return new List<Component>().AsQueryable();

            return await GetByPackageInGameVersion(packagePattern, targetGameVersion);
        }

        public async Task<IQueryable<Component>> GetByPackageInGameVersion(string packagePattern, GameVersion gameVersion)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedMappings.Any(vc =>
                vc.GameVersion == gameVersion &&
                Regex.IsMatch((vc.Metadata as ClassMetadata).Package, packagePattern)));
        }

        public async Task<IQueryable<Component>> GetByPackageInLatestRelease(string packagePattern)
        {
            var latestRelease =
                await MCMSContext.Releases.OrderByDescending(r => r.CreatedOn).FirstOrDefaultAsync();

            if (latestRelease == null)
                return new List<Component>().AsQueryable();

            return await GetByPackageInRelease(packagePattern, latestRelease);
        }

        public async Task<IQueryable<Component>> GetByPackageInRelease(string packagePattern, Guid releaseId)
        {
            var targetRelease =
                await MCMSContext.Releases.FirstOrDefaultAsync(r => r.Id == releaseId);

            if (targetRelease == null)
                return new List<Component>().AsQueryable();

            return await GetByPackageInRelease(packagePattern, targetRelease);
        }

        public async Task<IQueryable<Component>> GetByPackageInRelease(string packagePattern, Release release)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedMappings.Any(vc =>
                Regex.IsMatch((vc.Metadata as ClassMetadata).Package, packagePattern) &&
                vc.Mappings.Any(m => m.Releases.Any(r => r.Release == release))));
        }
    }
}
