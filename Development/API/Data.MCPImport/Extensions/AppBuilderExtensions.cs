using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Data.EFCore.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Data.MCPImport.Extensions
{
    public static class AppBuilderExtensions
    {

        public static IApplicationBuilder AddMCPImport(this IApplicationBuilder app)
        {
            Task.Run(async () =>
            {
                var mcpConfiguration =
                    app.ApplicationServices.GetRequiredService<IConfiguration>().GetSection("MCPImport");
                var logger = app.ApplicationServices.GetRequiredService<ILogger<IDataImportHandler>>();
                var database = app.ApplicationServices.GetRequiredService<MCMSContext>();
                var dataImportHandlers = app.ApplicationServices.GetServices<IDataImportHandler>().ToList();

                if (!mcpConfiguration.GetValue<bool>("Enabled"))
                {
                    logger.LogWarning("MCPImport disabled via configuration. Skipping");
                    return;
                }

                logger.LogWarning($"Attempting to import data using {dataImportHandlers.Count} handlers.");
                foreach (var dataImportHandler in dataImportHandlers)
                {
                    await dataImportHandler.Import(database);
                    await database.SaveChangesAsync();
                }
            });

            return app;
        }
    }
}
