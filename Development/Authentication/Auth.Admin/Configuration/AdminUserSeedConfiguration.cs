using Auth.Admin.Configuration.Interfaces;

namespace Auth.Admin.Configuration
{
    public class AdminUserSeedConfiguration : IAdminUserSeedConfiguration
    {
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }
        public string AdminEmail { get; set; }
    }
}
