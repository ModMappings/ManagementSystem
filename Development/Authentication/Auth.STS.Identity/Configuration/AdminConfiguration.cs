using Auth.STS.Identity.Configuration.Interfaces;

namespace Auth.STS.Identity.Configuration
{
    public class AdminConfiguration : IAdminConfiguration
    {
        public string IdentityAdminBaseUrl { get; set; } = "http://localhost:9000";

        public bool ForgeHttpsProtocol { get; set; } = false;
    }
}
