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
    /// <summary>
    /// Contains extension methods for the <see cref="IApplicationBuilder"/> class.
    /// </summary>
    public static class AppBuilderExtensions
    {

        /// <summary>
        /// Applies the database migrations to the to the database the application is attached to.
        /// </summary>
        /// <param name="builder">The application pipeline builder.</param>
        /// <returns>The application pipeline builder.</returns>
        public static IApplicationBuilder AddDatabaseMigrations(this IApplicationBuilder builder)
        {
            //Get a scope so we can properly access the database and logger.
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                //Get a logger and a database connection.
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<EFCore>>();
                var mcmsContext = scope.ServiceProvider.GetRequiredService<MCMSContext>();

                //Check for pending migrations.
                var pendingMigrations = mcmsContext.Database.GetPendingMigrations().ToList();
                if (pendingMigrations.Any())
                {
                    //Attempt to apply the pending migrations.
                    logger.LogWarning(
                        $"Pending migrations available. Applying: {pendingMigrations.Count()} new migrations.");

                    //Apply each of the pending migrations.
                    var appliedCount = 0;
                    foreach (var pendingMigration in pendingMigrations)
                    {
                        //Execute the migration.
                        logger.LogWarning("  > " + (appliedCount + 1) + "/" + pendingMigrations.Count + " (" + pendingMigration + ")");
                        mcmsContext.Database.GetService<IMigrator>().Migrate(pendingMigration);
                        appliedCount++;
                    }

                    logger.LogWarning("Database fully migrated.");
                }
                else
                {
                    //No pending migrations needs to be applied.
                    logger.LogInformation("No pending migrations.");
                }
            }

            //Finish the applying of the migrations, return the pipeline unchanged.
            return builder;
        }
    }
}
