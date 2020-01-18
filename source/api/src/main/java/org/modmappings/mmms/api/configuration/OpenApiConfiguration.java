package org.modmappings.mmms.api.configuration;

import io.swagger.v3.oas.models.Components;
import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;
import io.swagger.v3.oas.models.info.License;
import io.swagger.v3.oas.models.security.OAuthFlow;
import io.swagger.v3.oas.models.security.OAuthFlows;
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
                                .type(SecurityScheme.Type.HTTP)
                                .scheme("bearer")
                                .bearerFormat("JWT")
                                .flows(new OAuthFlows()
                                        .authorizationCode(new OAuthFlow())
                                )
                        )
                        .addSecuritySchemes("ModMappings Local development auth", new SecurityScheme()
                                .type(SecurityScheme.Type.HTTP)
                                .scheme("bearer")
                                .bearerFormat("JWT")
                                .flows(new OAuthFlows()
                                        .authorizationCode(new OAuthFlow())
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
