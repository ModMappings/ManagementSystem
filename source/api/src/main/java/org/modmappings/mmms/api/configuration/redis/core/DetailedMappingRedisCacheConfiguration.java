package org.modmappings.mmms.api.configuration.redis.core;

import com.fasterxml.jackson.databind.type.TypeFactory;
import org.modmappings.mmms.api.model.mapping.mappable.DetailedMappingDTO;
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
public class DetailedMappingRedisCacheConfiguration {

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, Page<DetailedMappingDTO>> detailedMappingPageReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<Page<DetailedMappingDTO>> valueSerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructParametricType(
                CachedPageImpl.class,
                DetailedMappingDTO.class
            )
        );
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, Page<DetailedMappingDTO>> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, Page<DetailedMappingDTO>> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, DetailedMappingDTO> detailedMappingReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<DetailedMappingDTO> valueSerializer = new Jackson2JsonRedisSerializer<>(DetailedMappingDTO.class);
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, DetailedMappingDTO> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, DetailedMappingDTO> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, Page<DetailedMappingDTO>> detailedMappingPageReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, Page<DetailedMappingDTO>> template
    ) {
        return template.opsForValue();
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, DetailedMappingDTO> detailedMappingReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, DetailedMappingDTO> template
    ) {
        return template.opsForValue();
    }

}
