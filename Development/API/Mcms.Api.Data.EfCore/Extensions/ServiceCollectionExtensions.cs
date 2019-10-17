using System;
using Mcms.Api.Data.Core.Manager.Comments;
using Mcms.Api.Data.Core.Manager.Core;
using Mcms.Api.Data.Core.Manager.Mapping.Component;
using Mcms.Api.Data.Core.Manager.Mapping.Mappings;
using Mcms.Api.Data.Core.Manager.Mapping.Mappings.Voting;
using Mcms.Api.Data.Core.Stores;
using Mcms.Api.Data.EfCore.Context;
using Mcms.Api.Data.EfCore.Manager.Comment;
using Mcms.Api.Data.EfCore.Manager.Core;
using Mcms.Api.Data.EfCore.Manager.Mapping.Component;
using Mcms.Api.Data.EfCore.Manager.Mapping.Mappings;
using Mcms.Api.Data.EfCore.Manager.Mapping.Mappings.Voting;
using Mcms.Api.Data.EfCore.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Mcms.Api.Data.EfCore.Extensions
{
    /// <summary>
    /// A set of extension methods that inject new services into the dependency container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds the required services for an EFCore based data layer of an MCMS Api to the given <see cref="IServiceCollection"/>,
        /// enabling it to provide the required services from the 'Mcmc.Api.Data.Core' library which use an EFCore ORM based database as backing store.
        /// </summary>
        /// <param name="services">The dependency container to inject the services into.</param>
        /// <param name="optionsAction">A callback used to configure the database connection, optional.</param>
        /// <returns>The dependency container with the services registered.</returns>
        public static IServiceCollection AddEfCoreDataLayer(this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction = null)
        {
            return services.AddDbContext<MCMSContext>(optionsAction)
                .AddEfCoreBasedDataStore()
                .AddEfCoreBasedManagers();
        }

        /// <summary>
        /// Injects the <see cref="EfCoreBackedStore{TEntity}"/> as a scoped implementation of the <see cref="IStore{TEntity}"/>.
        /// </summary>
        /// <param name="services">The dependency container to register the services to.</param>
        /// <returns>The dependency container with the services registered.</returns>
        public static IServiceCollection AddEfCoreBasedDataStore(this IServiceCollection services)
        {
            services.TryAddScoped(typeof(IStore<>), typeof(EfCoreBackedStore<>));
            services.TryAddScoped(typeof(EfCoreBackedStore<>));
            return services;
        }

        /// <summary>
        /// Injects the required data managers into the given <see cref="IServiceCollection"/>.
        /// Enabling the given <see cref="IServiceCollection"/> to provide all data managers as defined in 'Mcms.Api.Data.Core'
        /// backed up by an <see cref="EfCoreBackedStore{TEntity}"/>.
        /// </summary>
        /// <param name="services">The dependency container to register thee services to.</param>
        /// <returns>The dependency container with the services registered.</returns>
        public static IServiceCollection AddEfCoreBasedManagers(this IServiceCollection services)
        {
            return services.AddScoped<IComponentDataManager, ComponentDataManager>()
                .AddScoped<IVersionedComponentDataManager, VersionedComponentDataManager>()
                .AddScoped<ICommittedMappingDataManager, CommittedMappingDataManager>()
                .AddScoped<IProposedMappingDataManager, ProposedMappingDataManager>()
                .AddScoped<IVotingRecordDataManager, VotingRecordDataManager>()
                .AddScoped<IGameVersionDataManager, GameVersionDataManager>()
                .AddScoped<IMappingTypeDataManager, MappingTypeDataManager>()
                .AddScoped<IReleaseDataManager, ReleaseDataManager>()
                .AddScoped<ICommentDataManager, CommentDataManager>()
                .AddScoped<ICommentReactionDataManager, CommentReactionDataManager>();
        }
    }
}
