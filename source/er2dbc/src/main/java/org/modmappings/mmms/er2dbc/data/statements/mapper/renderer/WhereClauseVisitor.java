package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.modmappings.mmms.er2dbc.relational.core.sql.Where;
import org.springframework.data.relational.core.sql.Condition;
import org.springframework.data.relational.core.sql.Visitable;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class WhereClauseVisitor extends TypedSubtreeVisitor<Where> {

    private final RenderTarget parent;
    private final ConditionVisitor conditionVisitor;

    WhereClauseVisitor(final RenderContext context, final RenderTarget parent) {
        this.conditionVisitor = new ConditionVisitor(context);
        this.parent = parent;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterNested(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterNested(final Visitable segment) {

        if (segment instanceof Condition) {
            return Delegation.delegateTo(conditionVisitor);
        }

        return super.enterNested(segment);
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#leaveMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation leaveMatched(final Where segment) {

        parent.onRendered(conditionVisitor.getRenderedPart());
        return super.leaveMatched(segment);
    }
}