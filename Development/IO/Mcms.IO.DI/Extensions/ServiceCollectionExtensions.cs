using System;
using Mcms.IO.Core.Protocol.Manager.Builder;
using Mcms.IO.DI.Protocol.Manager.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mcms.IO.DI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Sets up the IO system to be used inside the default DI container.
        /// </summary>
        /// <param name="services">The services to register the systems components and protocols in.</param>
        /// <param name="configuration">The configuration callback used to setup protocols and components.</param>
        /// <returns>The collection with the required services added.</returns>
        public static IServiceCollection AddMcmsIO(this IServiceCollection services, Action<IIOProtocolManagerConfiguration> configuration)
        {
            var builder = new IOProtocolManagerBuilder(services);
            configuration(builder);
            builder.Build();
            return services;
        }
    }
}
