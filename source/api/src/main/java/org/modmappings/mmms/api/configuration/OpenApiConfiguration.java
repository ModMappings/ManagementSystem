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
import org.modmappings.mmms.api.util.Constants;
import org.springdoc.core.*;
import org.springdoc.core.customizers.OperationCustomizer;
import org.springdoc.core.customizers.ParameterCustomizer;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.LocalVariableTableParameterNameDiscoverer;

import java.util.List;
import java.util.Optional;

@Configuration
public class OpenApiConfiguration {

    @Bean
    public OpenAPI buildModMappingsOpenAPISpecification(@Value("${springdoc.version:0.0.0-Dev}") String appVersion) {
        return new OpenAPI()
                .components(new Components()
                        .addSecuritySchemes(Constants.MOD_MAPPINGS_OFFICIAL_AUTH, new SecurityScheme()
                                .type(SecurityScheme.Type.OAUTH2)
                                .scheme(Constants.BEARER_AUTH_SCHEME)
                                .in(SecurityScheme.In.HEADER)
                                .bearerFormat(Constants.JWT_BEARER_FORMAT)
                                .description(Constants.OFFICIAL_AUTH_DESC)
                                .openIdConnectUrl(Constants.OFFICIAL_AUTH_OPENID_CONFIG_URL)
                                .flows(new OAuthFlows()
                                        .implicit(
                                                new OAuthFlow()
                                                        .authorizationUrl(Constants.OFFICIAL_AUTH_AUTHORIZATION_URL)
                                                        .tokenUrl(Constants.OFFICIAL_AUTH_TOKEN_URL)
                                                        .scopes(new Scopes()
                                                                .addString(Constants.SCOPE_ROLES_NAME, Constants.SCOPE_ROLE_DESC)
                                                        )
                                        )
                                )
                        )
                        .addSecuritySchemes(Constants.MOD_MAPPINGS_DEV_AUTH, new SecurityScheme()
                                .type(SecurityScheme.Type.OAUTH2)
                                .scheme(Constants.BEARER_AUTH_SCHEME)
                                .in(SecurityScheme.In.HEADER)
                                .bearerFormat(Constants.JWT_BEARER_FORMAT)
                                .description(Constants.DEV_AUTH_DESC)
                                .openIdConnectUrl(Constants.DEV_AUTH_OPENID_CONFIG_URL)
                                .flows(new OAuthFlows()
                                    .implicit(
                                            new OAuthFlow()
                                                .authorizationUrl(Constants.DEV_AUTH_AUTHORIZATION_URL)
                                                .tokenUrl(Constants.DEV_AUTH_TOKEN_URL)
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
                    .url("https://api.modmappings.org")
                    .description("The central official mappings api for ModMappings.")
                )
                .addServersItem(new Server()
                    .url("http://localhost:8080")
                    .description("The local development server url.")
                );
    }
}
