using AutoMapper;
using Mcms.Api.Business.Poco.Api.REST.Core;
using Mcms.Api.Data.Poco.Models.Core;

namespace Mcms.Api.Business.AutoMapper.Profiles
{
    internal class GameVersionMappingProfile
        : Profile
    {
        public GameVersionMappingProfile()
        {
            SetupGameVersionToDtoMapping();
            SetupDtoToGameVersionMapping();
        }

        private void SetupGameVersionToDtoMapping()
        {
            var gameVersionToDtoMapping = CreateMap<GameVersion, GameVersionDto>();
            gameVersionToDtoMapping.ForAllMembers(d => d.Ignore());
            gameVersionToDtoMapping.ForMember(d => d.Id,
                opts => opts.MapFrom(d => d.Id));
            gameVersionToDtoMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            gameVersionToDtoMapping.ForMember(d => d.CreatedOn,
                opts => opts.MapFrom(d => d.CreatedOn));
            gameVersionToDtoMapping.ForMember(d => d.CreatedBy,
                opts => opts.MapFrom(d => d.CreatedBy));
            gameVersionToDtoMapping.ForMember(d => d.IsPreRelease,
                opts => opts.MapFrom(d => d.IsPreRelease));
            gameVersionToDtoMapping.ForMember(d => d.IsSnapshot,
                opts => opts.MapFrom(d => d.IsSnapshot));
        }

        private void SetupDtoToGameVersionMapping()
        {
            var dtoToGameVersionMapping = CreateMap<GameVersionDto, GameVersion>();
            dtoToGameVersionMapping.ForAllMembers(d => d.Ignore());
            dtoToGameVersionMapping.ForMember(d => d.Name,
                opts => opts.MapFrom(d => d.Name));
            dtoToGameVersionMapping.ForMember(d => d.IsPreRelease,
                opts => opts.MapFrom(d => d.IsPreRelease));
            dtoToGameVersionMapping.ForMember(d => d.IsSnapshot,
                opts => opts.MapFrom(d => d.IsSnapshot));
        }
    }
}
