package org.modmappings.mmms.er2dbc.data.access.strategy;

import org.modmappings.mmms.er2dbc.data.query.mapper.ExtendedMapper;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.relational.core.sql.IMatchFormatter;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.convert.R2dbcCustomConversions;
import org.springframework.data.r2dbc.core.DefaultReactiveDataAccessStrategy;
import org.springframework.data.r2dbc.core.NamedParameterExpander;
import org.springframework.data.r2dbc.dialect.R2dbcDialect;
import org.springframework.data.relational.core.dialect.RenderContextFactory;

import java.util.Collection;
import java.util.Collections;

public class ExtendedDataAccessStrategy extends DefaultReactiveDataAccessStrategy {
    private final ExtendedStatementMapper statementMapper;
    private final IMatchFormatter matchFormatter;

    /**
     * Creates a new {@link DefaultReactiveDataAccessStrategy} given {@link R2dbcDialect} and optional
     * {@link org.springframework.core.convert.converter.Converter}s.
     *
     * @param dialect the {@link R2dbcDialect} to use.
     * @param matchFormatter
     */
    public ExtendedDataAccessStrategy(final R2dbcDialect dialect, final IMatchFormatter matchFormatter) {
        this(dialect, Collections.emptyList(), matchFormatter);
    }

    /**
     * Creates a new {@link DefaultReactiveDataAccessStrategy} given {@link R2dbcDialect} and optional
     * {@link org.springframework.core.convert.converter.Converter}s.
     *
     * @param dialect the {@link R2dbcDialect} to use.
     * @param converters custom converters to register, must not be {@literal null}.
     * @param matchFormatter
     * @see R2dbcCustomConversions
     * @see org.springframework.core.convert.converter.Converter
     */
    public ExtendedDataAccessStrategy(final R2dbcDialect dialect, final Collection<?> converters, final IMatchFormatter matchFormatter) {
        this(dialect, createConverter(dialect, converters), matchFormatter);
    }


    /**
     * Creates a new {@link DefaultReactiveDataAccessStrategy} given {@link R2dbcDialect} and {@link R2dbcConverter}.
     *  @param dialect the {@link R2dbcDialect} to use.
     * @param converter must not be {@literal null}.
     * @param matchFormatter
     */
    public ExtendedDataAccessStrategy(final R2dbcDialect dialect, final R2dbcConverter converter, final IMatchFormatter matchFormatter) {
        this(dialect, converter, new NamedParameterExpander(), matchFormatter);
    }

    /**
     * Creates a new {@link DefaultReactiveDataAccessStrategy} given {@link R2dbcDialect} and {@link R2dbcConverter}.
     *  @param dialect the {@link R2dbcDialect} to use.
     * @param converter must not be {@literal null}.
     * @param expander must not be {@literal null}.
     * @param matchFormatter
     */
    @SuppressWarnings("unchecked")
    public ExtendedDataAccessStrategy(final R2dbcDialect dialect, final R2dbcConverter converter,
                                      final NamedParameterExpander expander, final IMatchFormatter matchFormatter) {
        super(dialect, converter, expander);
        this.matchFormatter = matchFormatter;

        final RenderContextFactory factory = new RenderContextFactory(dialect);
        this.statementMapper = new ExtendedStatementMapper(dialect, factory.createRenderContext(), new ExtendedMapper(converter, this.matchFormatter),
                this.getMappingContext());
    }

    @Override
    public ExtendedStatementMapper getStatementMapper() {
        return statementMapper;
    }
}
