package org.modmappings.mmms.api.configuration.redis.core;

import com.fasterxml.jackson.databind.type.TypeFactory;
import org.modmappings.mmms.api.model.mapping.mappable.VersionedMappableDTO;
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
public class VersionedMappableRedisCacheConfiguration {

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, Page<VersionedMappableDTO>> versionedMappablePageReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<Page<VersionedMappableDTO>> valueSerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructParametricType(
                CachedPageImpl.class,
                VersionedMappableDTO.class
            )
        );
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, Page<VersionedMappableDTO>> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, Page<VersionedMappableDTO>> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, VersionedMappableDTO> versionedMappableReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<VersionedMappableDTO> valueSerializer = new Jackson2JsonRedisSerializer<>(VersionedMappableDTO.class);
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, VersionedMappableDTO> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, VersionedMappableDTO> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, Page<VersionedMappableDTO>> versionedMappablePageReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, Page<VersionedMappableDTO>> template
    ) {
        return template.opsForValue();
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, VersionedMappableDTO> versionedMappableReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, VersionedMappableDTO> template
    ) {
        return template.opsForValue();
    }

}
