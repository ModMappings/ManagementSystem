using System.Linq;
using AutoMapper;
using Data.WebApi.Model.Api.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Data.WebApi.Mapping
{
    public class ApiModelMappingProfile
        : Profile
    {
        public ApiModelMappingProfile()
        {
            CreateMap<Component, ComponentDto>()
                .ForMember(c => c.VersionedComponents,
                    opts => opts.MapFrom(
                        map => map.VersionedComponents.ToDictionary(vc => vc.GameVersion.Name, vc => vc.Id)));
        }
    }
}
