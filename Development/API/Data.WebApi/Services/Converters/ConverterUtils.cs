using System.Linq;
using Data.Core.Models.Mapping;
using Data.WebApi.Model.Read.Core;

namespace Data.WebApi.Services.Converters
{
    public static class ConverterUtils
    {
        public static ProposalReadModel ConvertProposalDbModelToProposalReadModel(this ProposalMappingEntry proposalMappingEntry)
        {
            return new ProposalReadModel()
            {
                Id = proposalMappingEntry.Id,
                ProposedFor = proposalMappingEntry.VersionedComponent.Id,
                GameVersion = proposalMappingEntry.VersionedComponent.GameVersion.Id,
                ProposedBy = proposalMappingEntry.ProposedBy,
                ProposedOn = proposalMappingEntry.ProposedOn,
                IsOpen = proposalMappingEntry.IsOpen,
                IsPublicVote = proposalMappingEntry.IsPublicVote,
                VotedFor = proposalMappingEntry.VotedFor.ToList(),
                VotedAgainst = proposalMappingEntry.VotedAgainst.ToList(),
                Comment = proposalMappingEntry.Comment,
                ClosedBy = proposalMappingEntry.ClosedBy,
                ClosedOn = proposalMappingEntry.ClosedOn,
                In = proposalMappingEntry.InputMapping,
                Out = proposalMappingEntry.OutputMapping,
                Documentation = proposalMappingEntry.Documentation,
                MappingName = proposalMappingEntry.MappingType.Name,
                Distribution = proposalMappingEntry.Distribution
            };
        }

        public static MappingReadModel ConvertLiveDbModelToMappingReadModel(this LiveMappingEntry liveMappingEntry)
        {
            return new MappingReadModel()
            {
                Id = liveMappingEntry.Id,
                In = liveMappingEntry.InputMapping,
                Out = liveMappingEntry.OutputMapping,
                Documentation = liveMappingEntry.Documentation,
                MappingName = liveMappingEntry.MappingType.Name,
                Distribution = liveMappingEntry.Distribution
            };
        }
    }
}
