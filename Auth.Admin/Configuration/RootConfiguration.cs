using Microsoft.Extensions.Options;
using Auth.Admin.Configuration.Interfaces;

namespace Auth.Admin.Configuration
{
    public class RootConfiguration : IRootConfiguration
    {
        public IAdminConfiguration AdminConfiguration { get; set; }
        public ISeedConfiguration SeedConfiguration { get; }

        public RootConfiguration(IOptions<AdminConfiguration> adminConfiguration, ISeedConfiguration seedConfiguration)
        {
            SeedConfiguration = seedConfiguration;
            AdminConfiguration = adminConfiguration.Value;
        }
    }
}
