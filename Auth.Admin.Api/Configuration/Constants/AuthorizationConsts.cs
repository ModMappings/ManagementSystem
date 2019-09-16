namespace Auth.Admin.Api.Configuration.Constants
{
    public class AuthorizationConsts
    {
        public const string IdentityServerBaseUrl = "http://localhost:5000";
        public const string OidcSwaggerUIClientId = "auth-admin_api_swaggerui";
        public const string OidcApiName = "auth-admin_api";

        public const string AdministrationPolicy = "RequireAdministratorRole";
        public const string AdministrationRole = "SysAdmin";
    }
}