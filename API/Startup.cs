using System.Collections.Generic;
using API.Services.Core;
using API.Services.UserResolving;
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
using NSwag;
using NSwag.AspNetCore;

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

            services.AddDbContext<MCPContext>(opt =>
                opt.UseNpgsql(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddSwaggerDocument(config =>
            {
                var securityData = new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flow = OpenApiOAuth2Flow.Implicit,
                    AuthorizationUrl = "auth.mcp.service.test/connect/authorize",
                    Scopes = new Dictionary<string, string>
                    {
                        {"mcp.api", "MCP API - Access"}
                    }
                };

                config.PostProcess = document =>
                {
                    document.Info.Version = "v0.1";
                    document.Info.Title = "MCP.Service API";
                    document.Info.Description = "API for MCP data.";
                    document.Info.TermsOfService = "MCP";
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "MCP Team",
                        Email = string.Empty,
                        Url = "https://github.com/mcp_service_example"
                    };
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "Use under MCP Licence",
                        Url = "https://example.com/license"
                    };
                };
                config.AddSecurity("Authentication", securityData);
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

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.OAuth2Client = new OAuth2ClientSettings()
                {
                    AppName = "MCP.API - Swagger",
                    ClientId = "MCP.API_Swagger"
                };
            });

            app.UseMvc();
        }
    }
}
