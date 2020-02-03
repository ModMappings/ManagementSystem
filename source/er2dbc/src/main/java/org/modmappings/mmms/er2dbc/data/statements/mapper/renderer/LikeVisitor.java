package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.Like;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.lang.Nullable;

public class LikeVisitor extends FilteredSubtreeVisitor {

    private final RenderContext context;
    private final RenderTarget target;
    private final StringBuilder part = new StringBuilder();
    private @Nullable
    PartRenderer current;

    LikeVisitor(Like condition, RenderContext context, RenderTarget target) {
        super(it -> it == condition);
        this.context = context;
        this.target = target;
    }

    /*
     * (non-Javadoc)
     * @see FilteredSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(Visitable segment) {

        if (segment instanceof Expression) {
            ExpressionVisitor visitor = new ExpressionVisitor(context);
            current = visitor;
            return Delegation.delegateTo(visitor);
        }

        if (segment instanceof Condition) {
            ConditionVisitor visitor = new ConditionVisitor(context);
            current = visitor;
            return Delegation.delegateTo(visitor);
        }

        throw new IllegalStateException("Cannot provide visitor for " + segment);
    }

    /*
     * (non-Javadoc)
     * @see FilteredSubtreeVisitor#leaveNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveNested(Visitable segment) {

        if (current != null) {
            if (part.length() != 0) {
                part.append(" LIKE ");
            }

            part.append(current.getRenderedPart());
            current = null;
        }

        return super.leaveNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see FilteredSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(Visitable segment) {

        target.onRendered(part);

        return super.leaveMatched(segment);
    }
}
