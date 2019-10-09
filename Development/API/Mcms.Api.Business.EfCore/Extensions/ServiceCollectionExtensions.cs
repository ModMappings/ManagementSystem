using Data.EFCore.Store;
using Mcms.Api.Business.Core.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Data.EFCore.Extensions
{
    /// <summary>
    /// A set of extension methods that inject new services into the dependency container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {

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
    }
}
