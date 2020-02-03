package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Expression;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

public class TypedSingleConditionRenderSupport<T extends Visitable & Condition> extends TypedSubtreeVisitor<T> {

    private final RenderContext context;
    private @Nullable
    PartRenderer current;

    TypedSingleConditionRenderSupport(RenderContext context) {
        this.context = context;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
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

        PartRenderer current = this.current;
        this.current = null;
        return current.getRenderedPart();
    }
}

