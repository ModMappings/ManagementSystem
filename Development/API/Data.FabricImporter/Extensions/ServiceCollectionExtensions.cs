using Data.FabricImporter.Intermediary;
using Microsoft.Extensions.DependencyInjection;

namespace Data.FabricImporter.Extensions
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
