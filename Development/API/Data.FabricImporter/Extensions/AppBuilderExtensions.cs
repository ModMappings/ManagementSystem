using System;
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
                try
                {
                    using (var scope = app.ApplicationServices.CreateScope())
                    {
                        var mcpConfiguration =
                            scope.ServiceProvider.GetRequiredService<IConfiguration>().GetSection("MCPImport");
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<MCPImport>>();
                        var database = scope.ServiceProvider.GetRequiredService<MCMSContext>();
                        var dataImportHandlers = scope.ServiceProvider.GetServices<IDataImportHandler>().ToList();

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
                    }
                }
                catch (Exception e)
                {
                    var exceptionLogger = app.ApplicationServices.GetRequiredService<ILogger<MCPImport>>();
                    exceptionLogger.LogCritical(e, "Failed to import MCP data.");
                }
            });

            return app;
        }
    }
}
