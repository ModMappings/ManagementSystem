package org.modmappings.mmms.er2dbc.relational.postgres.sql;

import org.modmappings.mmms.er2dbc.relational.core.sql.IMatchFormatter;

public class PostgresMatchFormatter implements IMatchFormatter {

    @Override
    public String format(final String target, final String pattern) {
        return String.format("'%s' ~ %s", target, pattern);
    }
}
