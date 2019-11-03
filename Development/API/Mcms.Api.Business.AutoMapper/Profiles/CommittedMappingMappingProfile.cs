using System;
using System.Linq;
using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Mappings;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class CommittedMappingMappingProfile
        : Profile
    {
        public CommittedMappingMappingProfile()
        {
            SetupCommittedMappingToDtoMapping();
            SetupDtoToCommittedMappingMapping();
        }

        private void SetupCommittedMappingToDtoMapping()
        {
            var committedMappingToDtoMapping = CreateMap<CommittedMapping, CommittedMappingDto>();
            committedMappingToDtoMapping.ForAllMembers(d => d.Ignore());
            committedMappingToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            committedMappingToDtoMapping.ForMember(d => d.VersionedComponent,
                opts => opts.MapFrom(d => d.VersionedComponent.Id));
            committedMappingToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            committedMappingToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            committedMappingToDtoMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            committedMappingToDtoMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            committedMappingToDtoMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            committedMappingToDtoMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            committedMappingToDtoMapping.ForMember(d => d.MappingType,
                opts => opts.MapFrom(d => d.MappingType.Id));
            committedMappingToDtoMapping.ForMember(d => d.Proposal,
                opts => opts.MapFrom(d => d.ProposedMapping != null ? (Guid?) d.ProposedMapping.Id : (Guid?) null));
            committedMappingToDtoMapping.ForMember(d => d.Releases,
                opts => opts.MapFrom(d => d.Releases.Select(r => r.Release.Id).ToHashSet()));
        }

        private void SetupDtoToCommittedMappingMapping()
        {
            var dtoToCommittedMappingMapping = CreateMap<CommittedMappingDto, CommittedMapping>();
            dtoToCommittedMappingMapping.ForAllMembers(d => d.Ignore());
            dtoToCommittedMappingMapping.ForMember(d => d.InputMapping,
                opts => opts.MapFrom(d => d.InputMapping));
            dtoToCommittedMappingMapping.ForMember(d => d.OutputMapping,
                opts => opts.MapFrom(d => d.OutputMapping));
            dtoToCommittedMappingMapping.ForMember(d => d.Documentation,
                opts => opts.MapFrom(d => d.Documentation));
            dtoToCommittedMappingMapping.ForMember(d => d.Distribution,
                opts => opts.MapFrom(d => d.Distribution));
            dtoToCommittedMappingMapping.ForMember(d => d.ProposedMapping,
                opts => opts.MapFrom(d => d.Proposal.HasValue ? new ProposedMapping {Id = d.Proposal.Value} : null));
        }
    }
}
