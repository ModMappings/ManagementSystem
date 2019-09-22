using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Mapping;
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
            return await Task.FromResult(McmsContext.Components
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
                .Include("VersionedMappings.Metadata.MemberOf")
                .Include("VersionedMappings.Metadata.IsStatic"));
        }
    }
}
