using System.Linq;
using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Comments;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class ProposedMappingMappingProfile
        : Profile
    {

        public ProposedMappingMappingProfile()
        {
            SetupProposedMappingToDtoMapping();
            SetupDtoToProposedMappingMapping();
        }

        private void SetupProposedMappingToDtoMapping()
        {
            var proposedMappingToDtoMapping = CreateMap<ProposedMapping, ProposedMappingDto>();
            proposedMappingToDtoMapping.ForAllMembers(d => d.Ignore());
            proposedMappingToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            proposedMappingToDtoMapping.ForMember(d => d.VersionedComponent,
                opts => opts.MapFrom(d => d.VersionedComponent.Id));
            proposedMappingToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            proposedMappingToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            proposedMappingToDtoMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            proposedMappingToDtoMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            proposedMappingToDtoMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            proposedMappingToDtoMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            proposedMappingToDtoMapping.ForMember(d => d.MappingType,
                opts => opts.MapFrom(d => d.MappingType.Id));
            proposedMappingToDtoMapping.ForMember(d => d.IsOpen,
                opts => opts.MapFrom(d => d.IsOpen));
            proposedMappingToDtoMapping.ForMember(d => d.IsPublicVote,
                opts => opts.MapFrom(d => d.IsPublicVote));
            proposedMappingToDtoMapping.ForMember(d => d.Votes,
                opts => opts.MapFrom(d => d.Votes.Select(v => v.Id).ToHashSet()));
            proposedMappingToDtoMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d => d.Comments.Select(c => c.Id).ToHashSet()));
            proposedMappingToDtoMapping.ForMember(d => d.ClosedBy,
                opts => opts.MapFrom(d => d.ClosedBy));
            proposedMappingToDtoMapping.ForMember(d => d.ClosedOn,
                opts => opts.MapFrom(d => d.ClosedOn));
            proposedMappingToDtoMapping.ForMember(d => d.Merged,
                opts => opts.MapFrom(d => d.Merged));
            proposedMappingToDtoMapping.ForMember(d => d.CommittedWith,
                opts => opts.MapFrom(d => d.CommittedWithId));

        }

        private void SetupDtoToProposedMappingMapping()
        {
            var dtoToProposedMappingMapping = CreateMap<ProposedMappingDto, ProposedMapping>();
            dtoToProposedMappingMapping.ForAllMembers(d => d.Ignore());
            dtoToProposedMappingMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            dtoToProposedMappingMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            dtoToProposedMappingMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            dtoToProposedMappingMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            dtoToProposedMappingMapping.ForMember(d => d.IsOpen,
                opts => opts.MapFrom(d => d.IsOpen));
            dtoToProposedMappingMapping.ForMember(d => d.IsPublicVote,
                opts => opts.MapFrom(d => d.IsPublicVote));
            dtoToProposedMappingMapping.ForMember(d => d.Votes,
                opts => opts.MapFrom(d => d.Votes.Select(v => new VotingRecord {Id = v}).ToList()));
            dtoToProposedMappingMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d => d.Comments.Select(c => new Comment {Id = c}).ToList()));
            dtoToProposedMappingMapping.ForMember(d => d.ClosedBy,
                opts => opts.MapFrom(d => d.ClosedBy));
            dtoToProposedMappingMapping.ForMember(d => d.ClosedOn,
                opts => opts.MapFrom(d => d.ClosedOn));
            dtoToProposedMappingMapping.ForMember(d => d.Merged,
                opts => opts.MapFrom(d => d.Merged));
            dtoToProposedMappingMapping.ForMember(d => d.CommittedWithId,
                opts => opts.MapFrom(d => d.CommittedWith));
            dtoToProposedMappingMapping.ForMember(d => d.CommittedWith,
                opts => opts.MapFrom(d =>
                    d.CommittedWith.HasValue ? new CommittedMapping {Id = d.CommittedWith.Value} : null));
        }
    }
}
