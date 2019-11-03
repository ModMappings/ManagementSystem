using IdentityServer4.AccessTokenValidation;

namespace Mcms.Api.WebApi.Http.Configuration
{
    public class AuthenticationConfiguration : IdentityServerAuthenticationOptions
    {

        public string SwaggerUIClientId { get; set; }

        public string SwaggerUIClientSecret { get; set; }
    }
}
