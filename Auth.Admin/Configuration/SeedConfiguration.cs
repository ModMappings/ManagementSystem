using System;
using Auth.Admin.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace Auth.Admin.Configuration
{
    public class SeedConfiguration : ISeedConfiguration
    {
        public IAdminUserSeedConfiguration AdminUserSeedConfiguration { get; }

        public SeedConfiguration(IOptions<AdminUserSeedConfiguration> adminUserSeedConfiguration)
        {
            AdminUserSeedConfiguration = adminUserSeedConfiguration.Value;
        }
    }
}
