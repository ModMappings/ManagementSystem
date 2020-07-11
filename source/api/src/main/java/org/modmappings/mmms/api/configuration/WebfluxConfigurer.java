package org.modmappings.mmms.api.configuration;

import org.modmappings.mmms.api.spring.RoutePrefixBasedMethodArgumentResolver;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.domain.Pageable;
import org.springframework.data.web.ReactivePageableHandlerMethodArgumentResolver;
import org.springframework.data.web.ReactiveSortHandlerMethodArgumentResolver;
import org.springframework.web.reactive.config.WebFluxConfigurer;
import org.springframework.web.reactive.result.method.annotation.ArgumentResolverConfigurer;

import java.util.Map;

@Configuration
public class WebfluxConfigurer implements WebFluxConfigurer {

    @Value("${spring.arguments.resolvers.pageable.maxsize:50}")
    private int MAX_PAGE_SIZE;

    @Override
    public void configureArgumentResolvers(final ArgumentResolverConfigurer configurer) {
        final ReactivePageableHandlerMethodArgumentResolver defaultPageableParser = new ReactivePageableHandlerMethodArgumentResolver();
        defaultPageableParser.setMaxPageSize(MAX_PAGE_SIZE);
        final ReactivePageableHandlerMethodArgumentResolver systemUtilsPageableParser = new ReactivePageableHandlerMethodArgumentResolver();

        final RoutePrefixBasedMethodArgumentResolver routePrefixBasedMethodArgumentResolver = new RoutePrefixBasedMethodArgumentResolver(
          Pageable.class,
          Map.of(
            "/system", systemUtilsPageableParser
          ),
          defaultPageableParser
        );

        configurer.addCustomResolver(routePrefixBasedMethodArgumentResolver);
        configurer.addCustomResolver(new ReactiveSortHandlerMethodArgumentResolver());

    }
}
