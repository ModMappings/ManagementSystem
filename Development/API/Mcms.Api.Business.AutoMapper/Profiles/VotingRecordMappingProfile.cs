using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Mappings.Voting;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class VotingRecordMappingProfile
        : Profile
    {
        public VotingRecordMappingProfile()
        {
            SetupVotingRecordToDtoMapping();
            SetupDtoToVotingRecordMapping();
        }

        private void SetupVotingRecordToDtoMapping()
        {
            var votingRecordToDtoMapping = CreateMap<VotingRecord, VotingRecordDto>();
            votingRecordToDtoMapping.ForAllMembers(d => d.Ignore());
            votingRecordToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            votingRecordToDtoMapping.ForMember(d => d.Proposal,
                opts => opts.MapFrom(d => d.Proposal.Id));
            votingRecordToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            votingRecordToDtoMapping.ForMember(d => d.VotedBy,
                opts => opts.MapFrom(d => d.VotedBy));
            votingRecordToDtoMapping.ForMember(d => d.IsForVote,
                opts => opts.MapFrom(d => d.IsForVote));
            votingRecordToDtoMapping.ForMember(d => d.HasBeenRescinded,
                opts => opts.MapFrom(d => d.HasBeenRescinded));
        }

        private void SetupDtoToVotingRecordMapping()
        {
            var dtoToVotingRecordMapping = CreateMap<VotingRecordDto, VotingRecord>();
            dtoToVotingRecordMapping.ForAllMembers(d => d.Ignore());
            dtoToVotingRecordMapping.ForMember(d => d.Proposal,
                opts => opts.MapFrom(d => new ProposedMapping {Id = d.Proposal}));
            dtoToVotingRecordMapping.ForMember(d => d.IsForVote,
                opts => opts.MapFrom(d => d.IsForVote));
            dtoToVotingRecordMapping.ForMember(d => d.HasBeenRescinded,
                opts => opts.MapFrom(d => d.HasBeenRescinded));
        }

    }
}
