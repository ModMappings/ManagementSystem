using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Auth.Admin.EntityFramework.Shared.DbContexts;
using Auth.Admin.EntityFramework.Shared.Entities.Identity;
using Auth.Admin.Helpers;
using Microsoft.Extensions.Configuration;

namespace Auth.Admin
{
    public class Program
    {
        private const string SeedArgs = "/seed";

        public static async Task Main(string[] args)
        {
            var seed = args.Any(x => x == SeedArgs);
            if (seed) args = args.Except(new[] { SeedArgs }).ToArray();

            var host = BuildWebHost(args);

            // Uncomment this to seed upon startup, alternatively pass in `dotnet run /seed` to seed using CLI
            // await DbMigrationHelpers.EnsureSeedData<IdentityServerConfigurationDbContext, AdminIdentityDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, UserIdentity, UserIdentityRole>(host);
            if (seed)
            {
                await DbMigrationHelpers.EnsureSeedData<IdentityServerConfigurationDbContext, AdminIdentityDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext, UserIdentity, UserIdentityRole>(host);
                return;
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name;

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(c => c.AddServerHeader = false)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "..", "config", $"global.json"), true, true);
                    config.AddJsonFile(
                        Path.Combine(env.ContentRootPath, "..", "config", $"global{env.EnvironmentName}.json"), true,
                        true);

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "..", "config", $"{applicationName}.json"),
                        true, true);
                    config.AddJsonFile(
                        Path.Combine(env.ContentRootPath, "..", "config",
                            $"{applicationName}.{env.EnvironmentName}.json"), true, true);

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "config", $"global.json"), true, true);
                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "config", $"global{env.EnvironmentName}.json"),
                        true, true);

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "config", $"{applicationName}.json"), true,
                        true);
                    config.AddJsonFile(
                        Path.Combine(env.ContentRootPath, "config", $"{applicationName}.{env.EnvironmentName}.json"),
                        true, true);

                    config.AddEnvironmentVariables();
                })
                .UseSerilog((context, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration, "Logging");
                })
                .UseStartup<Startup>()
                .Build();
        }
    }
}
