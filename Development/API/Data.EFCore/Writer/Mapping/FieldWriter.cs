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
                .Include(c => c.VersionedComponents)
                .Include("VersionedComponents.GameVersion")
                .Include("VersionedComponents.CreatedBy")
                .Include("VersionedComponents.Component")
                .Include("VersionedComponents.Mappings")
                .Include("VersionedComponents.Proposals")
                .Include("VersionedComponents.Metadata")
                .Include("VersionedComponents.GameVersion.User")
                .Include("VersionedComponents.Mappings.Proposal")
                .Include("VersionedComponents.Mappings.Releases")
                .Include("VersionedComponents.Proposals.ProposedBy")
                .Include("VersionedComponents.Proposals.VotedFor")
                .Include("VersionedComponents.Proposals.VotedAgainst")
                .Include("VersionedComponents.Proposals.ClosedBy")
                .Include("VersionedComponents.Proposals.WentLiveWith")
                .Include("VersionedComponents.Metadata.VersionedComponent")
                .Include("VersionedComponents.Metadata.VersionedComponent.Component")
                .Include("VersionedComponents.Metadata.MemberOf")
                .Include("VersionedComponents.Metadata.MemberOf.VersionedComponent")
                .Include("VersionedComponents.Metadata.MemberOf.VersionedComponent.Component")
                .Include("VersionedComponents.Metadata.IsStatic"));
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

            return queryable.Where(c => c.VersionedComponents.Any(vc =>
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

            return queryable.Where(c => c.VersionedComponents.Any(vc =>
                vc.Mappings.Any(m => m.Releases.Any(r => r.Release == release))&&
                ((vc.Metadata as FieldMetadata).MemberOf.VersionedComponent.Id == classId
                 ||
                 (vc.Metadata as FieldMetadata).MemberOf.VersionedComponent.Component.Id == classId)
                ));
        }
    }
}
