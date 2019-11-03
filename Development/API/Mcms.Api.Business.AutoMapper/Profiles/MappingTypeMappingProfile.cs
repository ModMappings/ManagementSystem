using System.Linq;
using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Core;
using Mcms.Api.Data.Poco.Models.Core;
using Mcms.Api.Data.Poco.Models.Core.Release;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class MappingTypeMappingProfile
        : Profile
    {
        public MappingTypeMappingProfile()
        {
            SetupMappingTypeToDtoMapping();
            SetupDtoToMappingTypeMapping();
        }

        private void SetupMappingTypeToDtoMapping()
        {
            var mappingTypeToDtoMapping = CreateMap<MappingType, MappingTypeDto>();
            mappingTypeToDtoMapping.ForAllMembers(d => d.Ignore());
            mappingTypeToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            mappingTypeToDtoMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            mappingTypeToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            mappingTypeToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            mappingTypeToDtoMapping.ForMember(d => d.Releases,
                opts => opts.MapFrom(d => d.Releases.Select(r => r.Id).ToHashSet()));
        }

        private void SetupDtoToMappingTypeMapping()
        {
            var dtoToMappingTypeMapping = CreateMap<MappingTypeDto, MappingType>();
            dtoToMappingTypeMapping.ForAllOtherMembers(d => d.Ignore());
            dtoToMappingTypeMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            dtoToMappingTypeMapping.ForMember(d => d.Releases,
                opts => opts.MapFrom(d => d.Releases.Select(id => new Release() {Id = id}).ToList()));
        }
    }
}
