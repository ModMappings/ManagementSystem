using Mcms.IO.Fabric.Intermediary;
using Microsoft.Extensions.DependencyInjection;

namespace Mcms.IO.Fabric.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddFabricImportDataHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IDataImportHandler, IntermediaryImportHandler>()
                ;
        }

    }
}
