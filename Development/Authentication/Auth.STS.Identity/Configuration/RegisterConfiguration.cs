using Auth.STS.Identity.Configuration.Interfaces;

namespace Auth.STS.Identity.Configuration
{
    public class RegisterConfiguration : IRegisterConfiguration
    {
        public bool Enabled { get; set; } = true;
    }
}
