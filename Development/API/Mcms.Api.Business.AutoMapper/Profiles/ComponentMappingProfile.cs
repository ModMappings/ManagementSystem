using System.Linq;
using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Mapping.Component;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class ComponentMappingProfile
        : Profile
    {

        public ComponentMappingProfile()
        {
            SetupComponentToDtoMapping();
            SetupDtoToComponentMapping();
        }

        private void SetupComponentToDtoMapping()
        {
            var componentToDtoMap = CreateMap<Component, ComponentDto>();
            componentToDtoMap.ForAllMembers(d => d.Ignore());
            componentToDtoMap.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            componentToDtoMap.ForMember(d => d.Type,
                opts => opts.MapFrom(d => d.Type));
            componentToDtoMap.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            componentToDtoMap.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            componentToDtoMap.ForMember(c => c.VersionedComponents,
                opts => opts.MapFrom(
                    map => map.VersionedComponents.Select(vc => vc.Id).ToHashSet()));
        }

        private void SetupDtoToComponentMapping()
        {
            var dtoToComponentMap = CreateMap<ComponentDto, Component>();
            dtoToComponentMap.ForAllMembers(opt => opt.Ignore());
            dtoToComponentMap.ForMember(dto => dto.Type, opt => opt.MapFrom(src => src.Type));
            dtoToComponentMap.ForMember(d => d.VersionedComponents,
                opts => opts.MapFrom(d =>
                    d.VersionedComponents.Select(id => new VersionedComponent() {Id = id}).ToList()));
        }
    }
}
