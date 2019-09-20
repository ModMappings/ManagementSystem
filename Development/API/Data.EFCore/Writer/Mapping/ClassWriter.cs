using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.MetaData;
using Data.Core.Writers.Mapping;
using Data.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Data.EFCore.Writer.Mapping
{
    public class ClassWriter
        : ComponentWriterBase, IClassComponentWriter
    {
        public ClassWriter(MCPContext mcpContext) : base(mcpContext)
        {
        }

        public override async Task<IQueryable<Component>> AsQueryable()
        {
            return await Task.FromResult(McpContext.Components
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
                .Include("VersionedMappings.Proposals.ProposedBy")
                .Include("VersionedMappings.Proposals.VotedFor")
                .Include("VersionedMappings.Proposals.VotedAgainst")
                .Include("VersionedMappings.Proposals.ClosedBy")
                .Include("VersionedMappings.Proposals.WentLiveWith")
                .Include("VersionedMappings.Metadata.Component")
                .Include("VersionedMappings.Metadata.Outer")
                .Include("VersionedMappings.Metadata.InheritsFrom"));
        }

        public async Task<IQueryable<Component>> GetByPackageInVersion(string package, Guid versionId)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedMappings.Any(vc =>
                vc.GameVersion.Id == versionId && (vc.Metadata as ClassMetadata).Package == package));
        }

        public async Task<IQueryable<Component>> GetByPackageInVersion(string package, GameVersion gameVersion)
        {
            return await GetByPackageInVersion(package, gameVersion.Id);
        }

        public async Task<IQueryable<Component>> GetByPackageInRelease(string package, Guid releaseId)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedMappings.Any(vc =>
                (vc.Metadata as ClassMetadata).Package == package &&
                vc.Mappings.Any(m => m.Releases.Any(rc => rc.Release.Id == releaseId))));
        }

        public async Task<IQueryable<Component>> GetByPackageInRelease(string package, Release release)
        {
            return await GetByPackageInRelease(package, release.Id);
        }
    }
}
