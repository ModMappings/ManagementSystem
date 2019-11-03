using AutoMapper;
using Mcms.Api.Business.AutoMapper.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Mcms.Api.Business.AutoMapper.Extensions
{
    /// <summary>
    /// Extensions class to handle injecting AutoMapper based remapping of DTO to MCMS data layer.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds DTO to DataLayer, and vice versa, Mapping using AutoMapper.
        /// </summary>
        /// <param name="services">The service collection (DI Container) to add the AutoMapper based implementation mapper for DTO to DataLayer mapping to.</param>
        /// <returns>The DI Container with the mapper implementation added.</returns>
        public static IServiceCollection AddDtoToDataLayerMapping(this IServiceCollection services)
        {
            return services.AddAutoMapper(
                typeof(CommentMappingProfile),
                typeof(CommentReactionMappingProfile),
                typeof(CommittedMappingMappingProfile),
                typeof(ComponentMappingProfile),
                typeof(GameVersionMappingProfile),
                typeof(MappingTypeMappingProfile),
                typeof(ProposedMappingMappingProfile),
                typeof(ReleaseMappingProfile),
                typeof(VersionedComponentMappingProfile),
                typeof(VotingRecordMappingProfile)
            );
        }
    }
}
