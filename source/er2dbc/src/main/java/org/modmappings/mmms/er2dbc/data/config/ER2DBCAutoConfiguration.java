package org.modmappings.mmms.er2dbc.data.config;

import io.r2dbc.spi.ConnectionFactory;
import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.relational.core.sql.IMatchFormatter;
import org.modmappings.mmms.er2dbc.relational.postgres.sql.PostgresMatchFormatter;
import org.springframework.boot.autoconfigure.AutoConfigureAfter;
import org.springframework.boot.autoconfigure.condition.ConditionalOnBean;
import org.springframework.boot.autoconfigure.condition.ConditionalOnClass;
import org.springframework.boot.autoconfigure.condition.ConditionalOnMissingBean;
import org.springframework.boot.autoconfigure.r2dbc.ConnectionFactoryAutoConfiguration;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.data.r2dbc.convert.MappingR2dbcConverter;
import org.springframework.data.r2dbc.convert.R2dbcCustomConversions;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.ReactiveDataAccessStrategy;
import org.springframework.data.r2dbc.dialect.DialectResolver;
import org.springframework.data.r2dbc.mapping.R2dbcMappingContext;
import org.springframework.data.relational.core.mapping.RelationalMappingContext;

@Configuration(proxyBeanMethods = false)
public class ER2DBCAutoConfiguration {

    private final ConnectionFactory connectionFactory;

    public ER2DBCAutoConfiguration(ConnectionFactory connectionFactory) {
        this.connectionFactory = connectionFactory;
    }

    @Bean
    @Primary
    public ExtendedDataAccessStrategy extendedDataAccessStrategy(RelationalMappingContext mappingContext,
                                                                 R2dbcCustomConversions r2dbcCustomConversions,
                                                                 IMatchFormatter matchFormatter) {
        MappingR2dbcConverter converter = new MappingR2dbcConverter(mappingContext, r2dbcCustomConversions);
        return new ExtendedDataAccessStrategy(DialectResolver.getDialect(this.connectionFactory), converter, matchFormatter);
    }

    @Bean
    @ConditionalOnMissingBean
    public IMatchFormatter matchFormatter() {
        return new PostgresMatchFormatter();
    }

}
