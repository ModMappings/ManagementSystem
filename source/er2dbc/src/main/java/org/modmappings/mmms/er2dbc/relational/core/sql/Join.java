package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Table;

public class Join extends AbstractSegment {

    private final org.springframework.data.relational.core.sql.Join.JoinType type;
    private final Table joinTable;
    private final Condition on;

    public Join(final org.springframework.data.relational.core.sql.Join.JoinType type, final Table joinTable, final Condition on) {

        super(joinTable, on);

        this.joinTable = joinTable;
        this.type = type;
        this.on = on;
    }

    /**
     * @return join type.
     */
    public org.springframework.data.relational.core.sql.Join.JoinType getType() {
        return type;
    }

    /**
     * @return the joined {@link Table}.
     */
    public Table getJoinTable() {
        return joinTable;
    }

    /**
     * @return join condition (the ON or USING part).
     */
    public Condition getOn() {
        return on;
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public String toString() {
        return type + " " + joinTable + " ON " + on;
    }
}