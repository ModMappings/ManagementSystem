package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.IsNull;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class IsNullVisitor extends TypedSingleConditionRenderSupport<IsNull> {

    private final RenderTarget target;
    private final StringBuilder part = new StringBuilder();

    IsNullVisitor(RenderContext context, RenderTarget target) {
        super(context);
        this.target = target;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveNested(Visitable segment) {

        if (hasDelegatedRendering()) {
            part.append(consumeRenderedPart());
        }

        return super.leaveNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(IsNull segment) {

        if (segment.isNegated()) {
            part.append(" IS NOT NULL");
        } else {
            part.append(" IS NULL");
        }

        target.onRendered(part);

        return super.leaveMatched(segment);
    }
}

