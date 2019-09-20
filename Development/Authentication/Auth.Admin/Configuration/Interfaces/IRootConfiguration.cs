namespace Auth.Admin.Configuration.Interfaces
{
    public interface IRootConfiguration
    {
        IAdminConfiguration AdminConfiguration { get; }

        ISeedConfiguration SeedConfiguration { get; }
    }
}
