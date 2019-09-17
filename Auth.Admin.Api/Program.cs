using System.IO;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Auth.Admin.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name;

            return WebHost.CreateDefaultBuilder(args)
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
                .UseStartup<Startup>();
        }
    }
}
