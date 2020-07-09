package org.modmappings.mmms.api.configuration;

import io.swagger.v3.oas.models.Components;
import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;
import io.swagger.v3.oas.models.info.License;
import io.swagger.v3.oas.models.security.OAuthFlow;
import io.swagger.v3.oas.models.security.OAuthFlows;
import io.swagger.v3.oas.models.security.Scopes;
import io.swagger.v3.oas.models.security.SecurityScheme;
import io.swagger.v3.oas.models.servers.Server;
import org.modmappings.mmms.api.springdoc.PageableSupportConverter;
import org.modmappings.mmms.api.util.Constants;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class OpenApiConfiguration {

    @Value("${spring.url}")
    private String URL;
    @Value("${spring.security.target.host}")
    private String URL_SEC;
    @Value("${spring.security.target.realm}")
    private String REALM_SEC;

    @Bean
    public OpenAPI buildModMappingsOpenAPISpecification(@Value("${springdoc.version:0.0.0-Dev}") final String appVersion) {
        return new OpenAPI()
                .components(new Components()
                        .addSecuritySchemes(Constants.MOD_MAPPINGS_OFFICIAL_AUTH, new SecurityScheme()
                                .type(SecurityScheme.Type.OAUTH2)
                                .scheme(Constants.BEARER_AUTH_SCHEME)
                                .in(SecurityScheme.In.HEADER)
                                .bearerFormat(Constants.JWT_BEARER_FORMAT)
                                .description(Constants.OFFICIAL_AUTH_DESC)
                                .openIdConnectUrl(buildOpenIdConfigUrl(URL_SEC, REALM_SEC))
                                .flows(new OAuthFlows()
                                        .implicit(
                                                new OAuthFlow()
                                                        .authorizationUrl(buildOpenIdAuthUrl(URL_SEC, REALM_SEC))
                                                        .tokenUrl(buildOpenIdTokenUrl(URL_SEC, REALM_SEC))
                                                        .scopes(new Scopes()
                                                                .addString(Constants.SCOPE_ROLES_NAME, Constants.SCOPE_ROLE_DESC)
                                                        )
                                        )
                                )
                        )
                )
                .info(new Info()
                        .title("ModMappings API")
                        .version(appVersion)
                        .description("This is the api for ModMappings. It is currently in development and in an alpha stage.")
                        .license(new License()
                                .name("GNU LESSER GENERAL PUBLIC LICENSE v3")
                                .url("https://www.gnu.org/licenses/lgpl-3.0.en.html")
                        )
                )
                .addServersItem(new Server()
                        .url(URL)
                        .description("The current server.")
                );
    }

    @Bean
    public PageableSupportConverter pageableSupportConverter() {
        return new PageableSupportConverter();
    }

    private String buildOpenIdConfigUrl(final String url, final String realm) {
        return String.format("%s/auth/realms/%s/.well-known/openid-configuration", url, realm);
    }

    private String buildOpenIdAuthUrl(final String url, final String realm) {
        return String.format("%s/auth/realms/%s/protocol/openid-connect/auth", url, realm);
    }

    private String buildOpenIdTokenUrl(final String url, final String realm) {
        return String.format("%s/auth/realms/%s/protocol/openid-connect/token", url, realm);
    }
}
