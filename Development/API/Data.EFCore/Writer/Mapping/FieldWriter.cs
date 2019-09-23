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
    public class FieldWriter
        : ComponentWriterBase, IFieldComponentWriter
    {
        public FieldWriter(MCMSContext mcmsContext) : base(mcmsContext)
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
                .Include("VersionedMappings.Proposals.ProposedBy")
                .Include("VersionedMappings.Proposals.VotedFor")
                .Include("VersionedMappings.Proposals.VotedAgainst")
                .Include("VersionedMappings.Proposals.ClosedBy")
                .Include("VersionedMappings.Proposals.WentLiveWith")
                .Include("VersionedMappings.Metadata.VersionedComponent")
                .Include("VersionedMappings.Metadata.VersionedComponent.Component")
                .Include("VersionedMappings.Metadata.MemberOf")
                .Include("VersionedMappings.Metadata.MemberOf.VersionedComponent")
                .Include("VersionedMappings.Metadata.MemberOf.VersionedComponent.Component")
                .Include("VersionedMappings.Metadata.IsStatic"));
        }

        public async Task<IQueryable<Component>> GetByClassInLatestGameVersion(Guid classId)
        {
            var latestGameVersion =
                await MCMSContext.GameVersions.OrderByDescending(gv => gv.CreatedOn).FirstOrDefaultAsync();

            if (latestGameVersion == null)
                return new List<Component>().AsQueryable();

            return await GetByClassInGameVersion(classId, latestGameVersion);
        }

        public async Task<IQueryable<Component>> GetByClassInGameVersion(Guid classId, Guid gameVersionId)
        {
            var targetGameVersion =
                await MCMSContext.GameVersions.FirstOrDefaultAsync(gv => gv.Id == gameVersionId);

            if (targetGameVersion == null)
                return new List<Component>().AsQueryable();

            return await GetByClassInGameVersion(classId, targetGameVersion);
        }

        public async Task<IQueryable<Component>> GetByClassInGameVersion(Guid classId, GameVersion gameVersion)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedMappings.Any(vc =>
                vc.GameVersion == gameVersion &&
                ((vc.Metadata as FieldMetadata).MemberOf.VersionedComponent.Id == classId
                ||
                (vc.Metadata as FieldMetadata).MemberOf.VersionedComponent.Component.Id == classId)));
        }

        public async Task<IQueryable<Component>> GetByClassInLatestRelease(Guid classId)
        {
            var latestRelease =
                await MCMSContext.Releases.OrderByDescending(r => r.CreatedOn).FirstOrDefaultAsync();

            if (latestRelease == null)
                return new List<Component>().AsQueryable();

            return await GetByClassInRelease(classId, latestRelease);
        }

        public async Task<IQueryable<Component>> GetByClassInRelease(Guid classId, Guid releaseId)
        {
            var targetRelease =
                await MCMSContext.Releases.FirstOrDefaultAsync(r => r.Id == releaseId);

            if (targetRelease == null)
                return new List<Component>().AsQueryable();

            return await GetByClassInRelease(classId, targetRelease);
        }

        public async Task<IQueryable<Component>> GetByClassInRelease(Guid classId, Release release)
        {
            var queryable = await AsQueryable();

            return queryable.Where(c => c.VersionedMappings.Any(vc =>
                vc.Mappings.Any(m => m.Releases.Any(r => r.Release == release))&&
                ((vc.Metadata as FieldMetadata).MemberOf.VersionedComponent.Id == classId
                 ||
                 (vc.Metadata as FieldMetadata).MemberOf.VersionedComponent.Component.Id == classId)
                ));
        }
    }
}
