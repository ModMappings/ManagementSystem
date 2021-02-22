package org.modmappings.mmms.api.configuration;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.data.redis.connection.ReactiveRedisConnectionFactory;
import org.springframework.data.redis.connection.RedisConnectionFactory;
import org.springframework.data.redis.connection.lettuce.LettuceConnectionFactory;

@Configuration
public class RedisConfiguration {

    private static final Logger LOGGER = LoggerFactory.getLogger(RedisConfiguration.class);

    @Value("${spring.caching.redis.host:localhost}")
    private String HOST;
    @Value("${spring.caching.redis.port:6379}")
    private int PORT;

    @Bean
    public RedisConnectionFactory redisConnectionFactory() {
        LOGGER.warn(String.format("Setting up REDIS cache to: %s:%d", HOST, PORT));
        return new LettuceConnectionFactory(HOST, PORT);
    }

    @Bean
    public ReactiveRedisConnectionFactory reactiveRedisConnectionFactory(final RedisConnectionFactory innerFactory) {
        if (innerFactory instanceof ReactiveRedisConnectionFactory)
            return (ReactiveRedisConnectionFactory) innerFactory;

        LOGGER.warn(String.format("Setting up REDIS cache to: %s:%d", HOST, PORT));
        return new LettuceConnectionFactory(HOST, PORT);
    }




}
