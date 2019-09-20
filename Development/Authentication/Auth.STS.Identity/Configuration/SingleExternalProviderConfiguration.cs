using IdentityServer4.EntityFramework.Entities;

namespace Auth.STS.Identity.Configuration
{
    public class SingleExternalProviderConfiguration
    {
        public bool Active { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
