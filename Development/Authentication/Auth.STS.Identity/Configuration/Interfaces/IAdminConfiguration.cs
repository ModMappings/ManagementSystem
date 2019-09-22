namespace Auth.STS.Identity.Configuration.Interfaces
{
    public interface IAdminConfiguration
    {
        string IdentityAdminBaseUrl { get; }

        bool ForgeHttpsProtocol { get; }
    }
}
