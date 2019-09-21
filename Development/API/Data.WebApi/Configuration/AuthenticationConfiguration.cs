using IdentityServer4.AccessTokenValidation;

namespace Data.WebApi.Configuration
{
    public class AuthenticationConfiguration : IdentityServerAuthenticationOptions
    {

        public string SwaggerUIClientId { get; set; }

        public string SwaggerUIClientSecret { get; set; }
    }
}
