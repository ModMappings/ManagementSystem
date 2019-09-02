using System.Linq;
using Data.Core.Models.Class;
using Microsoft.EntityFrameworkCore;

namespace Data.EFCore.Writer
{
    public static class IncludeExtensions
    {

        public static IQueryable<ClassMapping> IncludeForDefaultClassMappingLogic(
            this IQueryable<ClassMapping> queryable)
        {
            return queryable
                .Include(cls => cls.VersionedMappings)
                .Include("VersionedMappings.GameVersion")
                .Include("VersionedMappings.CreatedBy")
                .Include("VersionedMappings.CommittedMappings")
                .Include("VersionedMappings.ProposalMappings")
                .Include("VersionedMappings.InheritsFrom")
                .Include("VersionedMappings.Outer")
                .Include("VersionedMappings.ProposalMappings.ProposedBy")
                .Include("VersionedMappings.ProposalMappings.VersionedMapping")
                .Include("VersionedMappings.ProposalMappings.VotedFor")
                .Include("VersionedMappings.ProposalMappings.VotedAgainst")
                .Include("VersionedMappings.ProposalMappings.ClosedBy")
                .Include("VersionedMappings.ProposalMappings.VersionedMapping.GameVersion");
        }

        public static IQueryable<ClassVersionedMapping> IncludeForDefaultClassVersionedMappingLogic(
            this IQueryable<ClassVersionedMapping> queryable)
        {
            return queryable
                .Include("Mapping")
                .Include("GameVersion")
                .Include("User")
                .Include("CommittedMappings")
                .Include("ProposalMappings")
                .Include("InheritsFrom")
                .Include("ProposalMappings.ProposedBy")
                .Include("ProposalMappings.GameVersion")
                .Include("ProposalMappings.VotedFor")
                .Include("ProposalMappings.VotedAgainst")
                .Include("ProposalMappings.ClosedBy");
        }
    }
}
