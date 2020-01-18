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
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class OpenApiConfiguration {

    @Bean
    public OpenAPI customOpenAPI(@Value("${springdoc.version:0.0.0-Dev}") String appVersion) {
        return new OpenAPI()
                .components(new Components()
                        .addSecuritySchemes("ModMappings Official auth", new SecurityScheme()
                                .type(SecurityScheme.Type.OPENIDCONNECT)
                                .scheme("bearer")
                                .in(SecurityScheme.In.HEADER)
                                .bearerFormat("JWT")
                                .description("The official OpenID connect authentication server for ModMappings.")
                                .openIdConnectUrl("https://testauth.minecraftforge.net/realms/ModMappings/.well-known/openid-configuration")
                                .flows(new OAuthFlows()
                                        .implicit(
                                                new OAuthFlow()
                                                        .authorizationUrl("https://testauth.minecraftforge.net/realms/ModMappings/protocol/openid-connect/auth")
                                                        .tokenUrl("https://testauth.minecraftforge.net/realms/ModMappings/protocol/openid-connect/token")
                                                        .scopes(new Scopes()
                                                                .addString("GAMEVERSIONS_DELETE", "Allows for game version deletion")
                                                                .addString("GAMEVERSIONS_UPDATE", "Allows for game version updating")
                                                                .addString("GAMEVERSIONS_CREATE", "Allows for game version creation")
                                                        )
                                        )
                                )
                        )
                        .addSecuritySchemes("ModMappings Local development auth", new SecurityScheme()
                                .type(SecurityScheme.Type.OPENIDCONNECT)
                                .scheme("bearer")
                                .in(SecurityScheme.In.HEADER)
                                .bearerFormat("JWT")
                                .description("The local development OpenID connect authentication server for ModMappings.")
                                .openIdConnectUrl("http://localhost:8081/realms/ModMappings/.well-known/openid-configuration")
                                .flows(new OAuthFlows()
                                    .implicit(
                                            new OAuthFlow()
                                                .authorizationUrl("http://localhost:8081/realms/ModMappings/protocol/openid-connect/auth")
                                                .tokenUrl("http://localhost:8081/realms/ModMappings/protocol/openid-connect/token")
                                                .scopes(new Scopes()
                                                        .addString("GAMEVERSIONS_DELETE", "Allows for game version deletion")
                                                        .addString("GAMEVERSIONS_UPDATE", "Allows for game version updating")
                                                        .addString("GAMEVERSIONS_CREATE", "Allows for game version creation")
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
