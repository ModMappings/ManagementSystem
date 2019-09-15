// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.AspNetCore;
using Serilog.Events;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "IdentityServer4.EntityFramework";

            var seed = args.Contains("/seed");
            if (seed)
            {
                args = args.Except(new[] {"/seed"}).ToArray();
            }

            var host = CreateWebHostBuilder(args).Build();

            if (seed)
            {
                var config = host.Services.GetRequiredService<IConfiguration>();
                var persistenceConnectionString = config.GetConnectionString("Persistence");
                var configurationConnectionString = config.GetConnectionString("Configuration");
                SeedData.EnsureSeedData(persistenceConnectionString, configurationConnectionString);
                return;
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var applicationName = Assembly.GetEntryAssembly()?.GetName().Name;

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseIIS()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "..", "config", $"global.json"), true, true);
                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "..", "config", $"global{env.EnvironmentName}.json"), true, true);

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "..", "config", $"{applicationName}.json"), true, true);
                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "..", "config", $"{applicationName}.{env.EnvironmentName}.json"), true, true);

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "config", $"global.json"), true, true);
                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "config", $"global{env.EnvironmentName}.json"), true, true);

                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "config", $"{applicationName}.json"), true, true);
                    config.AddJsonFile(Path.Combine(env.ContentRootPath, "config", $"{applicationName}.{env.EnvironmentName}.json"), true, true);

                    config.AddEnvironmentVariables();
                })
                .UseSerilog((context, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration, "Logging");
                });
        }
    }
}
