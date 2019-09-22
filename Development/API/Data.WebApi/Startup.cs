﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Data.Core.Readers.Core;
using Data.Core.Writers.Core;
using Data.EFCore.Context;
using Data.EFCore.Writer.Core;
using Data.EFCore.Writer.Mapping;
using Data.MCPImport.Extensions;
using Data.WebApi.Configuration;
using Data.WebApi.Services.Authorization;
using Data.WebApi.Services.Core;
using Data.WebApi.Services.UserResolving;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    Configuration.GetSection("AuthenticationServerConfig").Bind(options);
                });


            services.AddSwaggerGen(config =>
            {
                config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                config.SwaggerDoc("v1", new Info()
                {
                    Contact = new Contact()
                    {
                        Email = "mcp.service@test.com",
                        Url = "https://mcptest.ldtteam.com",
                        Name = "mcp.service - Development team"
                    },
                    Description = "OpenAPI documentation for the MCMS Data.WebApi.",
                    License = new License()
                    {
                        Name = "GPL v3",
                    },
                    Title = "MCP.Data.WebApi - OpenAPI Documentation",
                    Version = "0.1"
                });

                var customBoundJwtOptions = new AuthenticationConfiguration();
                Configuration.GetSection("AuthenticationServerConfig").Bind(customBoundJwtOptions);

                config.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Flow = "implicit",
                    AuthorizationUrl = $"{customBoundJwtOptions.Authority}/connect/authorize",
                    Scopes = new Dictionary<string, string> {
                        { customBoundJwtOptions.ApiName, "MCMS.Api" }
                    }
                });

                config.OperationFilter<AuthorizeCheckOperationFilter>(customBoundJwtOptions);
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

            var customBoundJwtOptions = new AuthenticationConfiguration();
            Configuration.GetSection("AuthenticationServerConfig").Bind(customBoundJwtOptions);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MCP.Data.WebApi - OpenAPI Documentation");

                c.OAuthClientId(customBoundJwtOptions.SwaggerUIClientId);
                c.OAuthClientSecret(customBoundJwtOptions.SwaggerUIClientSecret);
                c.OAuthAppName(customBoundJwtOptions.ApiName);
            });

            app.AddMCPImport();
        }
    }
}