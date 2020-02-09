package org.modmappings.mmms.api.configuration;

import io.netty.buffer.ByteBufAllocator;
import io.r2dbc.postgresql.PostgresqlConnectionConfiguration;
import io.r2dbc.postgresql.PostgresqlConnectionFactory;
import io.r2dbc.postgresql.api.PostgresqlConnection;
import io.r2dbc.postgresql.codec.CodecRegistry;
import io.r2dbc.postgresql.extension.CodecRegistrar;
import io.r2dbc.spi.ConnectionFactory;
import org.modmappings.mmms.api.configuration.codec.EnumCodec;
import org.modmappings.mmms.er2dbc.data.config.ER2DBCAutoConfiguration;
import org.modmappings.mmms.repository.repositories.Repositories;
import org.reactivestreams.Publisher;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.condition.ConditionalOnMissingBean;
import org.springframework.boot.jdbc.DataSourceBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Import;
import org.springframework.context.annotation.Primary;
import org.springframework.data.r2dbc.config.AbstractR2dbcConfiguration;
import org.springframework.data.r2dbc.connectionfactory.R2dbcTransactionManager;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.repository.config.EnableR2dbcRepositories;
import org.springframework.transaction.ReactiveTransactionManager;
import reactor.core.publisher.Mono;

import javax.sql.DataSource;

@Configuration
@EnableR2dbcRepositories(basePackageClasses = Repositories.class)
class R2DBCConfiguration extends AbstractR2dbcConfiguration {

    @Value("${spring.data.postgres.host}")
    private String host;
    @Value("${spring.data.postgres.port}")
    private int port;
    @Value("${spring.data.postgres.database}")
    private String database;
    @Value("${spring.data.postgres.username}")
    private String username;
    @Value("${spring.data.postgres.password}")
    private String password;

    @Bean
    @Override
    public PostgresqlConnectionFactory connectionFactory() {
        final PostgresqlConnectionConfiguration config = PostgresqlConnectionConfiguration.builder()
                                                                   .host(host)
                                                                   .port(port)
                                                                   .database(database)
                                                                   .username(username)
                                                                   .password(password)
                .codecRegistrar((connection, allocator, registry) -> {
                    registry.addLast(new EnumCodec(allocator));
                    return Mono.empty();
                })
                                                                   .build();
        return new PostgresqlConnectionFactory(config);
    }

    @Bean
    public DataSource dataSource() {
        final DataSourceBuilder dataSourceBuilder = DataSourceBuilder.create();
        dataSourceBuilder.driverClassName("org.postgresql.Driver");
        dataSourceBuilder.url("jdbc:postgresql://" + host + ":" + port + "/" + database);
        dataSourceBuilder.username(username);
        dataSourceBuilder.password(password);
        return dataSourceBuilder.build();
    }

    @Bean
    @Primary
    R2dbcTransactionManager connectionFactoryTransactionManager(final ConnectionFactory connectionFactory) {
        return new R2dbcTransactionManager(connectionFactory);
    }
}
