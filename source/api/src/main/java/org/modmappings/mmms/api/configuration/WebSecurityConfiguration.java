package org.modmappings.mmms.api.configuration;

import static org.springframework.security.config.Customizer.withDefaults;

import net.minidev.json.JSONArray;
import net.minidev.json.JSONObject;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.convert.converter.Converter;
import org.springframework.security.authentication.AbstractAuthenticationToken;
import org.springframework.security.config.annotation.method.configuration.EnableReactiveMethodSecurity;
import org.springframework.security.config.annotation.web.builders.WebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.config.annotation.web.reactive.EnableWebFluxSecurity;
import org.springframework.security.config.web.server.ServerHttpSecurity;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.oauth2.jwt.Jwt;
import org.springframework.security.oauth2.server.resource.authentication.JwtAuthenticationConverter;
import org.springframework.security.oauth2.server.resource.authentication.JwtGrantedAuthoritiesConverter;
import org.springframework.security.oauth2.server.resource.authentication.ReactiveJwtAuthenticationConverterAdapter;
import org.springframework.security.web.server.SecurityWebFilterChain;
import org.springframework.util.Assert;
import org.springframework.util.StringUtils;
import reactor.core.publisher.Mono;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.stream.Collectors;

@Configuration
@EnableReactiveMethodSecurity
@EnableWebFluxSecurity
public class WebSecurityConfiguration {

    @Bean
    SecurityWebFilterChain springSecurityFilterChain(ServerHttpSecurity http) {
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
        public Collection<GrantedAuthority> convert(Jwt jwt) {
            Collection<GrantedAuthority> grantedAuthorities = new ArrayList<>();
            for (String authority : getRealmRoles(jwt)) {
                grantedAuthorities.add(new SimpleGrantedAuthority(authority));
            }
            return grantedAuthorities;
        }

        private Collection<String> getRealmRoles(Jwt jwt) {
            JSONObject authData = jwt.getClaim("realm_access");
            JSONArray roleData = (JSONArray) authData.get("roles");
            return roleData.stream().map(roleObj -> (String) roleObj).map(roleStr -> "ROLE_" + roleStr).collect(Collectors.toSet());
        }
    }
}
