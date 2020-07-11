package org.modmappings.mmms.api.spring;

import org.springframework.core.MethodParameter;
import org.springframework.web.method.support.HandlerMethodArgumentResolver;
import org.springframework.web.reactive.BindingContext;
import org.springframework.web.reactive.result.method.SyncHandlerMethodArgumentResolver;
import org.springframework.web.server.ServerWebExchange;

import java.util.Map;

public class RoutePrefixBasedMethodArgumentResolver implements SyncHandlerMethodArgumentResolver
{

    final Class<?>                                   parameterType;
    final Map<String, SyncHandlerMethodArgumentResolver> prefixBasedResolvers;
    final SyncHandlerMethodArgumentResolver defaultResolver;

    public RoutePrefixBasedMethodArgumentResolver(
      final Class<?> parameterType,
      final Map<String, SyncHandlerMethodArgumentResolver> prefixBasedResolvers, final SyncHandlerMethodArgumentResolver defaultResolver) {
        this.parameterType = parameterType;
        this.prefixBasedResolvers = prefixBasedResolvers;
        this.defaultResolver = defaultResolver;
    }

    @Override
    public Object resolveArgumentValue(
      final MethodParameter parameter, final BindingContext bindingContext, final ServerWebExchange exchange)
    {
        return this.prefixBasedResolvers.keySet().stream()
          .filter(prefix -> exchange.getRequest().getPath().pathWithinApplication().value().startsWith(prefix))
          .map(this.prefixBasedResolvers::get)
          .map(resolver -> resolver.resolveArgumentValue(parameter, bindingContext, exchange))
          .findFirst()
          .orElseGet(() -> this.defaultResolver.resolveArgumentValue(parameter, bindingContext, exchange));
    }

    @Override
    public boolean supportsParameter(final MethodParameter parameter)
    {
        return this.parameterType.equals(parameter.getParameterType());
    }
}
