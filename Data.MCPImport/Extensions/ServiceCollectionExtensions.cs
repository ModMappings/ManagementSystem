using Data.MCPImport.TSRG;
using Microsoft.Extensions.DependencyInjection;

namespace Data.MCPImport.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMCPImportDataHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IDataImportHandler, TSRGImportHandler>();
        }

    }
}
