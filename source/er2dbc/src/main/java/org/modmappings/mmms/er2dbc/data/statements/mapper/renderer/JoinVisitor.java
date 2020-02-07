package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.modmappings.mmms.er2dbc.relational.core.sql.Join;
import org.springframework.data.relational.core.sql.Aliased;
import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Table;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class JoinVisitor extends TypedSubtreeVisitor<Join> {

    private final RenderContext context;
    private final RenderTarget parent;
    private final StringBuilder joinClause = new StringBuilder();
    private boolean inCondition = false;
    private boolean hasSeenCondition = false;

    JoinVisitor(final RenderContext context, final RenderTarget parent) {
        this.context = context;
        this.parent = parent;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterMatched(final Join segment) {

        joinClause.append(segment.getType().getSql()).append(' ');

        return super.enterMatched(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(final Visitable segment) {

        if (segment instanceof Table && !inCondition) {
            joinClause.append(context.getNamingStrategy().getName(((Table) segment)));
            if (segment instanceof Aliased) {
                joinClause.append(" AS ").append(((Aliased) segment).getAlias());
            }
        } else if (segment instanceof Condition) {

            // TODO: Use proper delegation for condition rendering.
            inCondition = true;
            if (!hasSeenCondition) {
                hasSeenCondition = true;
                joinClause.append(" ON ");
                joinClause.append(segment);
            }
        }

        return super.enterNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveNested(final Visitable segment) {

        if (segment instanceof Condition) {
            inCondition = false;
        }
        return super.leaveNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(final Join segment) {
        parent.onRendered(joinClause);
        return super.leaveMatched(segment);
    }
}
