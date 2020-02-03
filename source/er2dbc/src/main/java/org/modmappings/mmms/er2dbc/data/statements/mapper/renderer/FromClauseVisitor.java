package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.modmappings.mmms.er2dbc.relational.core.sql.From;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class FromClauseVisitor extends TypedSubtreeVisitor<From> {

    private final FromTableVisitor visitor;
    private final RenderTarget parent;
    private final StringBuilder builder = new StringBuilder();
    private boolean first = true;

    FromClauseVisitor(RenderContext context, RenderTarget parent) {

        this.visitor = new FromTableVisitor(context, it -> {

            if (first) {
                first = false;
            } else {
                builder.append(", ");
            }

            builder.append(it);
        });

        this.parent = parent;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(Visitable segment) {
        return Delegation.delegateTo(visitor);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(From segment) {
        parent.onRendered(builder);
        return super.leaveMatched(segment);
    }
}
