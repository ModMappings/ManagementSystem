namespace Auth.STS.Identity.Configuration.Interfaces
{
    public interface IRootConfiguration
    {
        IAdminConfiguration AdminConfiguration { get; }

        IRegisterConfiguration RegisterConfiguration { get; }
    }
}