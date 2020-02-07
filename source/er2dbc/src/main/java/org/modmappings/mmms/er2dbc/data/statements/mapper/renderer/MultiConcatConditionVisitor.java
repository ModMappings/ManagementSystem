package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.AndCondition;
import org.springframework.data.relational.core.sql.OrCondition;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class MultiConcatConditionVisitor extends FilteredSingleConditionRenderSupport {

    private final RenderTarget target;
    private final String concat;
    private final StringBuilder part = new StringBuilder();

    MultiConcatConditionVisitor(final RenderContext context, final AndCondition condition, final RenderTarget target) {
        super(context, it -> it == condition);
        this.target = target;
        this.concat = " AND ";
    }

    MultiConcatConditionVisitor(final RenderContext context, final OrCondition condition, final RenderTarget target) {
        super(context, it -> it == condition);
        this.target = target;
        this.concat = " OR ";
    }

    /*
     * (non-Javadoc)
     * @see FilteredSubtreeVisitor#leaveNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveNested(final Visitable segment) {

        if (hasDelegatedRendering()) {
            if (part.length() != 0) {
                part.append(concat);
            }

            part.append(consumeRenderedPart());
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