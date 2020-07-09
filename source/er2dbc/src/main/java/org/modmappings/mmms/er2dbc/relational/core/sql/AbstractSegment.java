package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Segment;
import org.springframework.data.relational.core.sql.Visitor;
import org.springframework.util.Assert;

public class AbstractSegment implements Segment {

    private final Segment[] children;

    protected AbstractSegment(final Segment... children) {
        this.children = children;
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Visitable#visit(org.springframework.data.relational.core.sql.Visitor)
     */
    @Override
    public void visit(final Visitor visitor) {

        Assert.notNull(visitor, "Visitor must not be null!");

        visitor.enter(this);
        for (final Segment child : children) {
            child.visit(visitor);
        }
        visitor.leave(this);
    }

    public Segment[] getChildren() {
        return children;
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#hashCode()
     */
    @Override
    public int hashCode() {
        return toString().hashCode();
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#equals(java.lang.Object)
     */
    @Override
    public boolean equals(final Object obj) {
        return obj instanceof Segment && toString().equals(obj.toString());
    }
}
