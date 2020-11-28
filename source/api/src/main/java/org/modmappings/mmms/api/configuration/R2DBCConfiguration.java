package org.modmappings.mmms.api.configuration;

import io.r2dbc.postgresql.PostgresqlConnectionFactoryProvider;
import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.repositories.Repositories;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.condition.ConditionalOnMissingBean;
import org.springframework.boot.autoconfigure.flyway.FlywayDataSource;
import org.springframework.boot.autoconfigure.r2dbc.ConnectionFactoryOptionsBuilderCustomizer;
import org.springframework.boot.jdbc.DataSourceBuilder;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.core.annotation.Order;
import org.springframework.data.r2dbc.connectionfactory.R2dbcTransactionManager;
import org.springframework.data.r2dbc.repository.config.EnableR2dbcRepositories;
import org.springframework.transaction.TransactionManager;

import javax.sql.DataSource;

@Configuration
@EnableR2dbcRepositories(basePackageClasses = Repositories.class)
class R2DBCConfiguration {

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
    @FlywayDataSource
    public DataSource dataSource() {
        final DataSourceBuilder dataSourceBuilder = DataSourceBuilder.create();
        dataSourceBuilder.driverClassName("org.postgresql.Driver");
        dataSourceBuilder.url("jdbc:postgresql://" + host + ":" + port + "/" + database);
        dataSourceBuilder.username(username);
        dataSourceBuilder.password(password);
        return dataSourceBuilder.build();
    }

    @Bean
    @ConditionalOnMissingBean(name = "reactiveDataAccessStrategy")
    public ExtendedDataAccessStrategy reactiveDataAccessStrategy(final ExtendedDataAccessStrategy extendedDataAccessStrategy) {
        return extendedDataAccessStrategy;
    }

    @Bean()
    @Primary
    @Order(Integer.MIN_VALUE)
    public TransactionManager preferredTransactionManager(R2dbcTransactionManager transactionManager) {
        return transactionManager;
    }

    @Bean
    public ConnectionFactoryOptionsBuilderCustomizer customEncoderCustomizer() {
        return builder -> builder.option(PostgresqlConnectionFactoryProvider.AUTODETECT_EXTENSIONS, true);
    }
}
