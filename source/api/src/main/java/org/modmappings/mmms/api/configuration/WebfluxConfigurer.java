package org.modmappings.mmms.api.configuration;

import org.springframework.context.annotation.Configuration;
import org.springframework.data.web.ReactivePageableHandlerMethodArgumentResolver;
import org.springframework.data.web.ReactiveSortHandlerMethodArgumentResolver;
import org.springframework.web.reactive.config.WebFluxConfigurer;
import org.springframework.web.reactive.result.method.annotation.ArgumentResolverConfigurer;

@Configuration
public class WebfluxConfigurer implements WebFluxConfigurer {

    @Override
    public void configureArgumentResolvers(final ArgumentResolverConfigurer configurer) {
        configurer.addCustomResolver(new ReactivePageableHandlerMethodArgumentResolver());
        configurer.addCustomResolver(new ReactiveSortHandlerMethodArgumentResolver());

    }
}
