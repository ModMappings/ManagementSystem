namespace Auth.Admin.Configuration.Interfaces
{
    public interface IAdminUserSeedConfiguration
    {
        string AdminUserName { get; set; }

        string AdminPassword { get; set; }

        string AdminEmail { get; set; }
    }
}
