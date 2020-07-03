package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Comparison;
import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.lang.Nullable;

public class ComparisonVisitor extends FilteredSubtreeVisitor {

    private final RenderContext context;
    private final Comparison condition;
    private final RenderTarget target;
    private final StringBuilder part = new StringBuilder();
    private @Nullable
    PartRenderer current;

    ComparisonVisitor(final RenderContext context, final Comparison condition, final RenderTarget target) {
        super(it -> it == condition);
        this.condition = condition;
        this.target = target;
        this.context = context;
    }

    /*
     * (non-Javadoc)
     * @see FilteredSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(final Visitable segment) {

        if (segment instanceof Expression) {
            final ExpressionVisitor visitor = new ExpressionVisitor(context);
            current = visitor;
            return Delegation.delegateTo(visitor);
        }

        if (segment instanceof Condition) {
            final ConditionVisitor visitor = new ConditionVisitor(context);
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
    Delegation leaveNested(final Visitable segment) {

        if (current != null) {
            if (part.length() != 0) {
                part.append(' ').append(condition.getComparator()).append(' ');
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
    Delegation leaveMatched(final Visitable segment) {

        target.onRendered(part);

        return super.leaveMatched(segment);
    }
}
