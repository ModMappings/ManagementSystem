using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using API.Initialization;
using API.Services.Core;
using API.Services.UserResolving;
using Data.Core.Models.Core;
using Data.Core.Readers.Class;
using Data.Core.Readers.Core;
using Data.Core.Readers.Field;
using Data.Core.Readers.Method;
using Data.Core.Readers.Parameter;
using Data.Core.Writers.Class;
using Data.Core.Writers.Core;
using Data.Core.Writers.Field;
using Data.Core.Writers.Method;
using Data.Core.Writers.Parameters;
using Data.EFCore.Context;
using Data.EFCore.Writer.Class;
using Data.EFCore.Writer.Core;
using Data.EFCore.Writer.Field;
using Data.EFCore.Writer.Method;
using Data.EFCore.Writer.Parameter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks()
                .AddDbContextCheck<MCPContext>();

            services.AddEntityFrameworkProxies();

            services.AddDbContext<MCPContext>(opt =>
                opt.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"])
                    .UseLazyLoadingProxies());

            services.AddSwaggerGen(config =>
            {
                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Contact = new OpenApiContact
                    {
                        Email = "mcp.service@test.com",
                        Url = new Uri("https://mcptest.ldtteam.com"),
                        Name = "mcp.service - Development team"
                    },
                    Description = "OpenAPI documentation for the MCP.Service API.",
                    License = new OpenApiLicense
                    {
                        Name = "GPL v3",
                    },
                    Title = "MCP.API - OpenAPI Documentation",
                    Version = "0.1"
                });
            });

            services.AddTransient<IUserResolvingService, DummyUserResolvingService>();
            services.AddTransient<IClassMappingWriter, ClassMappingWriter>();
            services.AddTransient<IClassMappingReader, ClassMappingWriter>();
            services.AddTransient<IMethodMappingWriter, MethodMappingWriter>();
            services.AddTransient<IMethodMappingReader, MethodMappingWriter>();
            services.AddTransient<IFieldMappingWriter, FieldMappingWriter>();
            services.AddTransient<IFieldMappingReader, FieldMappingWriter>();
            services.AddTransient<IParameterMappingWriter, ParameterMappingWriter>();
            services.AddTransient<IParameterMappingReader, ParameterMappingWriter>();
            services.AddTransient<IGameVersionReader, GameVersionWriter>();
            services.AddTransient<IGameVersionWriter, GameVersionWriter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MCP.API - OpenAPI Documentation");
            });

            UpdateDatabase(app);
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            MCPDataInitializer.InitializeData(app);
        }
    }
}
