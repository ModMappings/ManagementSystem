using System.Collections.Generic;
using System.Linq;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mcms.Api.WebApi.Http.Services.Authorization
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly IdentityServerAuthenticationOptions _identityServerAuthenticationOptions;

        public AuthorizeCheckOperationFilter(IdentityServerAuthenticationOptions identityServerAuthenticationOptions)
        {
            _identityServerAuthenticationOptions = identityServerAuthenticationOptions;
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.MethodInfo.DeclaringType != null && context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                   .Union(context.MethodInfo.GetCustomAttributes(true))
                                   .OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.TryAdd("401", new Response { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>> {
                    new Dictionary<string, IEnumerable<string>> {{"oauth2", new[] { _identityServerAuthenticationOptions.ApiName } }}
                };
            }
        }
    }
}
