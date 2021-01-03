package org.modmappings.mmms.api.configuration.redis.core;

import com.fasterxml.jackson.databind.type.TypeFactory;
import org.modmappings.mmms.api.model.core.release.ReleaseDTO;
import org.modmappings.mmms.api.util.CachedPageImpl;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.redis.connection.ReactiveRedisConnectionFactory;
import org.springframework.data.redis.core.ReactiveRedisTemplate;
import org.springframework.data.redis.core.ReactiveValueOperations;
import org.springframework.data.redis.serializer.Jackson2JsonRedisSerializer;
import org.springframework.data.redis.serializer.RedisSerializationContext;

import java.util.HashMap;
import java.util.Map;

@Configuration
public class ReleaseRedisCacheConfiguration {

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, Page<ReleaseDTO>> releasePageReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<Page<ReleaseDTO>> valueSerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructParametricType(
                CachedPageImpl.class,
                ReleaseDTO.class
            )
        );
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, Page<ReleaseDTO>> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, Page<ReleaseDTO>> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveRedisTemplate<Map<String, String>, ReleaseDTO> releaseReactiveRedisTemplate(
            final ReactiveRedisConnectionFactory factory) {
        final Jackson2JsonRedisSerializer<Map<String, String>> keySerializer = new Jackson2JsonRedisSerializer<>(TypeFactory.defaultInstance().constructMapType(
                HashMap.class,
                String.class,
                String.class
        ));
        final Jackson2JsonRedisSerializer<ReleaseDTO> valueSerializer = new Jackson2JsonRedisSerializer<>(ReleaseDTO.class);
        final RedisSerializationContext.RedisSerializationContextBuilder<Map<String, String>, ReleaseDTO> builder =
                RedisSerializationContext.newSerializationContext(keySerializer);
        final RedisSerializationContext<Map<String, String>, ReleaseDTO> context =
                builder.value(valueSerializer).build();
        return new ReactiveRedisTemplate<>(factory, context);
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, Page<ReleaseDTO>> releasePageReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, Page<ReleaseDTO>> template
    ) {
        return template.opsForValue();
    }

    @Bean
    public ReactiveValueOperations<Map<String, String>, ReleaseDTO> releaseReactiveRedisOperations(
            final ReactiveRedisTemplate<Map<String, String>, ReleaseDTO> template
    ) {
        return template.opsForValue();
    }

}
