package org.modmappings.mmms.api.configuration;

import net.minidev.json.JSONArray;
import net.minidev.json.JSONObject;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.security.oauth2.resource.OAuth2ResourceServerProperties;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.convert.converter.Converter;
import org.springframework.security.config.annotation.method.configuration.EnableReactiveMethodSecurity;
import org.springframework.security.config.annotation.web.reactive.EnableWebFluxSecurity;
import org.springframework.security.config.web.server.ServerHttpSecurity;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.oauth2.jwt.Jwt;
import org.springframework.security.oauth2.server.resource.authentication.JwtAuthenticationConverter;
import org.springframework.security.oauth2.server.resource.authentication.ReactiveJwtAuthenticationConverterAdapter;
import org.springframework.security.web.server.SecurityWebFilterChain;
import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.reactive.CorsConfigurationSource;
import org.springframework.web.cors.reactive.UrlBasedCorsConfigurationSource;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Locale;
import java.util.function.Supplier;
import java.util.stream.Collectors;

@Configuration
@EnableReactiveMethodSecurity
@EnableWebFluxSecurity
public class WebSecurityConfiguration {

    @Value("#{'${spring.security.oidc.claim-mode}'.toUpperCase()}")
    AuthMode authMode;

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
                        converter.setJwtGrantedAuthoritiesConverter(authMode.get());
                        jwtSpec.jwtAuthenticationConverter(new ReactiveJwtAuthenticationConverterAdapter(converter));
                    });
                })
                .cors(spec -> {
                    spec.configurationSource(corsConfigurationSource());
                });
        return http.build();
    }

    CorsConfigurationSource corsConfigurationSource() {
        UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
        source.registerCorsConfiguration("/**", new CorsConfiguration().applyPermitDefaultValues());
        return source;
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

    private static class WSO2RoleExtractingGrantedAuthoritiesConverter implements Converter<Jwt, Collection<GrantedAuthority>> {

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
            final JSONArray authData = jwt.getClaim("groups");
            //Filter out the weird additional groupings.
            //Only take the once for the application domain.
            return authData.stream()
                    .map(roleObj -> (String) roleObj)
                    .filter(roleStr -> roleStr.toLowerCase(Locale.ROOT).contains("application"))
                    .map(roleStr -> roleStr.substring(roleStr.indexOf("/") + 1))
                    .map(roleStr -> "ROLE_" + roleStr)
                    .collect(Collectors.toSet());
        }
    }

    public enum AuthMode {
        WSO2(WSO2RoleExtractingGrantedAuthoritiesConverter::new),
        KEY_CLOAK(KeycloakRoleExtractingGrantedAuthoritiesConverter::new);

        private final Supplier<Converter<Jwt, Collection<GrantedAuthority>>> modeConstructor;

        AuthMode(final Supplier<Converter<Jwt, Collection<GrantedAuthority>>> modeConstructor) {
            this.modeConstructor = modeConstructor;
        }

        public Converter<Jwt, Collection<GrantedAuthority>> get() {
            return this.modeConstructor.get();
        }
    }
}
