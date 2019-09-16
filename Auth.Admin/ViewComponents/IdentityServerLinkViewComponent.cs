using Microsoft.AspNetCore.Mvc;
using Auth.Admin.Configuration.Interfaces;

namespace Auth.Admin.ViewComponents
{
    public class IdentityServerLinkViewComponent : ViewComponent
    {
        private readonly IRootConfiguration _configuration;

        public IdentityServerLinkViewComponent(IRootConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IViewComponentResult Invoke()
        {
            var identityServerUrl = _configuration.AdminConfiguration.IdentityServerBaseUrl;
            
            return View(model: identityServerUrl);
        }
    }
}
