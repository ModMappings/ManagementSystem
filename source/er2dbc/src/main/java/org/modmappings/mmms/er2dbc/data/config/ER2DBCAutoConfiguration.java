package org.modmappings.mmms.er2dbc.data.config;

import io.r2dbc.spi.ConnectionFactory;
import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.relational.core.sql.IMatchFormatter;
import org.modmappings.mmms.er2dbc.relational.postgres.sql.PostgresMatchFormatter;
import org.springframework.boot.autoconfigure.condition.ConditionalOnMissingBean;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Primary;
import org.springframework.data.r2dbc.convert.MappingR2dbcConverter;
import org.springframework.data.r2dbc.convert.R2dbcCustomConversions;
import org.springframework.data.r2dbc.dialect.DialectResolver;
import org.springframework.data.relational.core.mapping.RelationalMappingContext;

@Configuration(proxyBeanMethods = false)
public class ER2DBCAutoConfiguration {

    private final ConnectionFactory connectionFactory;

    public ER2DBCAutoConfiguration(final ConnectionFactory connectionFactory) {
        this.connectionFactory = connectionFactory;
    }

    @Bean
    @Primary
    public ExtendedDataAccessStrategy extendedDataAccessStrategy(final RelationalMappingContext mappingContext,
                                                                 final R2dbcCustomConversions r2dbcCustomConversions,
                                                                 final IMatchFormatter matchFormatter) {
        final MappingR2dbcConverter converter = new MappingR2dbcConverter(mappingContext, r2dbcCustomConversions);
        return new ExtendedDataAccessStrategy(DialectResolver.getDialect(this.connectionFactory), converter, matchFormatter);
    }

    @Bean
    @ConditionalOnMissingBean
    public IMatchFormatter matchFormatter() {
        return new PostgresMatchFormatter();
    }

}
