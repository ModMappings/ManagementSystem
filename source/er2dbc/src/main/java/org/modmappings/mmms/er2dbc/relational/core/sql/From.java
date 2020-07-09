package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Table;
import org.springframework.util.StringUtils;

import java.util.Arrays;
import java.util.List;

public class From extends AbstractSegment {

    private final List<Table> tables;

    public From(final Table... tables) {
        this(Arrays.asList(tables));
    }

    public From(final List<Table> tables) {

        super(tables.toArray(new Table[]{}));

        this.tables = tables;
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public String toString() {
        return "FROM " + StringUtils.collectionToDelimitedString(tables, ", ");
    }
}