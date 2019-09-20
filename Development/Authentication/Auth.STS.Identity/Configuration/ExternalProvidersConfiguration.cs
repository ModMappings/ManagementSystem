namespace Auth.STS.Identity.Configuration
{
    public class ExternalProvidersConfiguration
    {
        public SingleExternalProviderConfiguration GitHub { get; set; } = null;

        public SingleExternalProviderConfiguration Discord { get; set; } = null;
    }
}
