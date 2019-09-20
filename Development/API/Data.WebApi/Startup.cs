using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Data.WebApi.Initialization;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Context;
using Data.EFCore.Writer.Core;
using Data.EFCore.Writer.Mapping;
using Data.MCPImport.Extensions;
using Data.WebApi.Services.Core;
using Data.WebApi.Services.UserResolving;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Data.WebApi
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
                    Description = "OpenAPI documentation for the MCP.Service Data.WebApi.",
                    License = new OpenApiLicense
                    {
                        Name = "GPL v3",
                    },
                    Title = "MCP.Data.WebApi - OpenAPI Documentation",
                    Version = "0.1"
                });
            });

            services.AddTransient<IUserResolvingService, AuthorizationBasedUserResolvingService>();
            services.AddTransient<IGameVersionReader, GameVersionWriter>();
            services.AddTransient<IGameVersionWriter, GameVersionWriter>();
            services.AddTransient<IReleaseReader, ReleaseWriter>();
            services.AddTransient<IReleaseWriter, ReleaseWriter>();
            services.AddTransient<IMappingTypeReader, MappingTypeWriter>();
            services.AddTransient<IMappingTypeWriter, MappingTypeWriter>();
            services.AddTransient<ComponentWriterFactory>();

            services.AddMCPImportDataHandlers();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MCP.Data.WebApi - OpenAPI Documentation");
            });

            app.AddMCPImport();
        }
    }
}
