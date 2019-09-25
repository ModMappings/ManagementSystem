using System.Linq;
using System.Threading.Tasks;
using Data.EFCore.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Data.EFCore.Extensions
{
    public static class AppBuilderExtensions
    {

        public static IApplicationBuilder AddDatabaseMigrations(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<EFCore>>();
                var mcmsContext = scope.ServiceProvider.GetRequiredService<MCMSContext>();

                var pendingMigrations = mcmsContext.Database.GetPendingMigrations().ToList();
                if (pendingMigrations.Any())
                {
                    logger.LogWarning(
                        $"Pending migrations available. Applying: {pendingMigrations.Count()} new migrations.");
                    var appliedCount = 0;
                    foreach (var pendingMigration in pendingMigrations)
                    {
                        logger.LogWarning("  > " + (appliedCount + 1) + "/" + pendingMigrations.Count + " (" + pendingMigration + ")");
                        mcmsContext.Database.GetService<IMigrator>().Migrate(pendingMigration);
                        appliedCount++;
                    }
                    logger.LogWarning("Database fully migrated.");
                }
                else
                {
                    logger.LogInformation("No pending migrations.");
                }
            }

            return builder;
        }
    }
}
