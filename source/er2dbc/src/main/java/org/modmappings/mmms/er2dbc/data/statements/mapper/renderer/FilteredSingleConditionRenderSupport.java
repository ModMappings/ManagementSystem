package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import java.util.function.Predicate;

public class FilteredSingleConditionRenderSupport extends FilteredSubtreeVisitor {

    private final RenderContext context;
    private @Nullable
    PartRenderer current;

    /**
     * Creates a new {@link FilteredSingleConditionRenderSupport} given the filter {@link Predicate}.
     *
     * @param context
     * @param filter filter predicate to identify when to {@link #enterMatched(Visitable)
     *          enter}/{@link #leaveMatched(Visitable) leave} the {@link Visitable segment} that this visitor is
     *          responsible for.
     */
    FilteredSingleConditionRenderSupport(final RenderContext context, final Predicate<Visitable> filter) {
        super(filter);
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

    /**
     * Returns whether rendering was delegated to a {@link ExpressionVisitor} or {@link ConditionVisitor}.
     *
     * @return {@literal true} when rendering was delegated to a {@link ExpressionVisitor} or {@link ConditionVisitor}.
     */
    protected boolean hasDelegatedRendering() {
        return current != null;
    }

    /**
     * Consumes the delegated rendering part. Call {@link #hasDelegatedRendering()} to check whether rendering was
     * actually delegated. Consumption releases the delegated rendered.
     *
     * @return the delegated rendered part.
     * @throws IllegalStateException if rendering was not delegate.
     */
    protected CharSequence consumeRenderedPart() {

        Assert.state(hasDelegatedRendering(), "Rendering not delegated. Cannot consume delegated rendering part.");

        final PartRenderer current = this.current;
        this.current = null;
        return current.getRenderedPart();
    }
}
