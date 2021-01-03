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

    @Value("${build.version:0.0.0-Dev}")
    private String VERSION;

    @Bean
    public OpenAPI buildModMappingsOpenAPISpecification() {
        return new OpenAPI()
                .components(new Components()
                        .addSecuritySchemes(Constants.MOD_MAPPINGS_OFFICIAL_AUTH, new SecurityScheme()
                                .type(SecurityScheme.Type.OAUTH2)
                                .scheme(Constants.BEARER_AUTH_SCHEME)
                                .in(SecurityScheme.In.HEADER)
                                .bearerFormat(Constants.JWT_BEARER_FORMAT)
                                .description(Constants.OFFICIAL_AUTH_DESC)
                                .openIdConnectUrl(buildOpenIdConfigUrl(URL_SEC))
                                .flows(new OAuthFlows()
                                        .implicit(
                                                new OAuthFlow()
                                                        .authorizationUrl(buildOpenIdAuthUrl(URL_SEC))
                                                        .tokenUrl(buildOpenIdTokenUrl(URL_SEC))
                                                        .scopes(new Scopes()
                                                                .addString(Constants.SCOPE_ROLES_NAME, Constants.SCOPE_ROLE_DESC)
                                                        )
                                        )
                                )
                        )
                )
                .info(new Info()
                        .title("ModMappings API")
                        .version(VERSION)
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

    private String buildOpenIdConfigUrl(final String url) {
        return String.format("%s/oauth2/token/.well-known/openid-configuration", url);
    }

    private String buildOpenIdAuthUrl(final String url) {
        return String.format("%s/oauth2/authorize", url);
    }

    private String buildOpenIdTokenUrl(final String url) {
        return String.format("%s/oauth2/token", url);
    }
}
