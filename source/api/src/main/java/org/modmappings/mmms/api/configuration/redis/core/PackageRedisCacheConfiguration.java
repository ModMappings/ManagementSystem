package org.modmappings.mmms.api.configuration.redis.core;

import com.fasterxml.jackson.databind.type.TypeFactory;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;
import org.modmappings.mmms.api.model.objects.PackageDTO;
import org.modmappings.mmms.api.util.CachedPageImpl;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.domain.Page;
import org.springframework.data.redis.connection.ReactiveRedisConnectionFactory;
import org.springframework.data.redis.core.ReactiveRedisTemplate;
import org.springframework.data.redis.core.ReactiveValueOperations;
import org.springframework.data.redis.serializer.Jackson2JsonRedisSerializer;
import org.springframework.data.redis.serializer.RedisSerializationContext;

import java.util.HashMap;
import java.util.Map;

@Configuration
public class PackageRedisCacheConfiguration {

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, Page<PackageDTO>> packagePageReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<Page<PackageDTO>> valueSerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructParametricType(
                CachedPageImpl.class,
                PackageDTO.class
            )
        );
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, Page<PackageDTO>> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, Page<PackageDTO>> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, Page<PackageDTO>> packagePageReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, Page<PackageDTO>> template
    ) {
        return template.opsForValue();
    }

}
