package org.modmappings.mmms.api.configuration;

import net.minidev.json.JSONArray;
import net.minidev.json.JSONObject;
import org.springframework.boot.autoconfigure.security.oauth2.resource.IssuerUriCondition;
import org.springframework.boot.autoconfigure.security.oauth2.resource.OAuth2ResourceServerProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Conditional;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.convert.converter.Converter;
import org.springframework.security.config.annotation.method.configuration.EnableReactiveMethodSecurity;
import org.springframework.security.config.annotation.web.reactive.EnableWebFluxSecurity;
import org.springframework.security.config.web.server.ServerHttpSecurity;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.oauth2.jwt.Jwt;
import org.springframework.security.oauth2.jwt.ReactiveJwtDecoder;
import org.springframework.security.oauth2.jwt.ReactiveJwtDecoders;
import org.springframework.security.oauth2.server.resource.authentication.JwtAuthenticationConverter;
import org.springframework.security.oauth2.server.resource.authentication.ReactiveJwtAuthenticationConverterAdapter;
import org.springframework.security.web.server.SecurityWebFilterChain;

import java.util.ArrayList;
import java.util.Collection;
import java.util.stream.Collectors;

@Configuration
@EnableReactiveMethodSecurity
@EnableWebFluxSecurity
public class WebSecurityConfiguration {

    private final OAuth2ResourceServerProperties.Jwt properties;

    public WebSecurityConfiguration(final OAuth2ResourceServerProperties properties) {
        this.properties = properties.getJwt();
    }


    @Bean
    public SecurityWebFilterChain springSecurityFilterChain(final ServerHttpSecurity http) {
        http
            .authorizeExchange(exchanges ->
                   exchanges.anyExchange().permitAll()
            )
            .oauth2ResourceServer(oAuth2ResourceServerSpec -> {
                oAuth2ResourceServerSpec.jwt(jwtSpec -> {
                    final JwtAuthenticationConverter converter = new JwtAuthenticationConverter();
                    converter.setJwtGrantedAuthoritiesConverter(new KeycloakRoleExtractingGrantedAuthoritiesConverter());

                    jwtSpec.jwtAuthenticationConverter(new ReactiveJwtAuthenticationConverterAdapter(converter));
                });
            });
        return http.build();
    }

    private static class KeycloakRoleExtractingGrantedAuthoritiesConverter implements Converter<Jwt, Collection<GrantedAuthority>> {

        /**
         * Extract {@link GrantedAuthority}s from the given {@link Jwt}.
         *
         * @param jwt The {@link Jwt} token
         * @return The {@link GrantedAuthority authorities} read from the token scopes
         */
        @Override
        public Collection<GrantedAuthority> convert(final Jwt jwt) {
            final Collection<GrantedAuthority> grantedAuthorities = new ArrayList<>();
            for (final String authority : getRealmRoles(jwt)) {
                grantedAuthorities.add(new SimpleGrantedAuthority(authority));
            }
            return grantedAuthorities;
        }

        private Collection<String> getRealmRoles(final Jwt jwt) {
            final JSONObject authData = jwt.getClaim("realm_access");
            final JSONArray roleData = (JSONArray) authData.get("roles");
            return roleData.stream().map(roleObj -> (String) roleObj).map(roleStr -> "ROLE_" + roleStr).collect(Collectors.toSet());
        }
    }
}
