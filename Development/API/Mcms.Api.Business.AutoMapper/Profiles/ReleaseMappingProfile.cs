using System.Linq;
using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Core;
using Mcms.Api.Data.Poco.Models.Comments;
using Mcms.Api.Data.Poco.Models.Core.Release;
using Mcms.Api.Data.Poco.Models.Mapping.Component;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class ReleaseMappingProfile
        : Profile
    {
        public ReleaseMappingProfile()
        {
            SetupReleaseToDtoMapping();
            SetupDtoToReleaseMapping();
        }

        private void SetupReleaseToDtoMapping()
        {
            var releaseToDtoMapping = CreateMap<Release, ReleaseDto>();
            releaseToDtoMapping.ForAllMembers(d => d.Ignore());
            releaseToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            releaseToDtoMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            releaseToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            releaseToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            releaseToDtoMapping.ForMember(d => d.GameVersion,
                opts => opts.MapFrom(d => d.GameVersion.Id));
            releaseToDtoMapping.ForMember(d => d.MappingType,
                opts => opts.MapFrom(d => d.MappingType.Id));
            releaseToDtoMapping.ForMember(d => d.IsSnapshot,
                opts => opts.MapFrom(d => d.IsSnapshot));
            releaseToDtoMapping.ForMember(d => d.PackageMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.PACKAGE)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.ClassMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.CLASS)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.MethodMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.METHOD)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.FieldMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.FIELD)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.ParameterMappings,
                opts => opts.MapFrom(d =>
                    d.Components.Where(c => c.Mapping.VersionedComponent.Component.Type == ComponentType.PARAMETER)
                        .Select(c => c.Id).ToHashSet()));
            releaseToDtoMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d =>
                    d.Comments.Select(c => c.Id).ToHashSet()));
        }

        private void SetupDtoToReleaseMapping()
        {
            var dtoToReleaseMapping = CreateMap<ReleaseDto, Release>();
            dtoToReleaseMapping.ForAllMembers(d => d.Ignore());
            dtoToReleaseMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            dtoToReleaseMapping.ForMember(d => d.Comments,
                opts => opts.MapFrom(d => d.Comments.Select(id => new Comment {Id = id})));
        }
    }
}
