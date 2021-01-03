package org.modmappings.mmms.api.configuration.redis.core;

import com.fasterxml.jackson.databind.type.TypeFactory;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;
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
public class MappingRedisCacheConfiguration {

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, Page<MappingDTO>> mappingPageReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<Page<MappingDTO>> valueSerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructParametricType(
                CachedPageImpl.class,
                MappingDTO.class
            )
        );
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, Page<MappingDTO>> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, Page<MappingDTO>> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, MappingDTO> mappingReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<MappingDTO> valueSerializer = new Jackson2JsonRedisSerializer<>(MappingDTO.class);
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, MappingDTO> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, MappingDTO> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, Page<MappingDTO>> mappingPageReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, Page<MappingDTO>> template
    ) {
        return template.opsForValue();
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, MappingDTO> mappingReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, MappingDTO> template
    ) {
        return template.opsForValue();
    }

}
