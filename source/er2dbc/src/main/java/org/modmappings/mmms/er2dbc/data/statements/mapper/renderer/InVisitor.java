package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.In;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class InVisitor extends TypedSingleConditionRenderSupport<In> {

    private final RenderTarget target;
    private final StringBuilder part = new StringBuilder();
    private boolean needsComma = false;
    private boolean notIn = false;

    InVisitor(RenderContext context, RenderTarget target) {
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
            CharSequence renderedPart = consumeRenderedPart();

            if (needsComma) {
                part.append(", ");
            }

            if (part.length() == 0) {
                part.append(renderedPart);
                if (notIn) {
                    part.append(" NOT");
                }
                part.append(" IN (");
            } else {
                part.append(renderedPart);
                needsComma = true;
            }
        }

        return super.leaveNested(segment);
    }

    @Override
    Delegation enterMatched(In segment) {

        notIn = segment.isNotIn();

        return super.enterMatched(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(In segment) {

        part.append(")");
        target.onRendered(part);

        return super.leaveMatched(segment);
    }
}
