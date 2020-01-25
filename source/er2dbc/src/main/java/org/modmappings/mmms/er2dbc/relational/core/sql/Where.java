package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Condition;

public class Where extends AbstractSegment {

    private final Condition condition;

    Where(Condition condition) {

        super(condition);

        this.condition = condition;
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public String toString() {
        return "WHERE " + condition.toString();
    }
}
