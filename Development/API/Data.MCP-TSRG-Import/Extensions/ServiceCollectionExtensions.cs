using Data.MCP.TSRG.Importer.TSRG;
using Microsoft.Extensions.DependencyInjection;

namespace Data.MCP.TSRG.Importer.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMCPImportDataHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IDataImportHandler, TSRGImportHandler>()
/*                .AddSingleton<IDataImportHandler>(services => new MCPImportHandler(
                    services.GetRequiredService<ILogger<MCPImportHandler>>(),
                    Constants.MCP_STABLE_NAME
                ))
                .AddSingleton<IDataImportHandler>(services => new MCPImportHandler(
                    services.GetRequiredService<ILogger<MCPImportHandler>>(),
                    Constants.MCP_SNAPSHOT_NAME
                ))*/
                ;
        }

    }
}
