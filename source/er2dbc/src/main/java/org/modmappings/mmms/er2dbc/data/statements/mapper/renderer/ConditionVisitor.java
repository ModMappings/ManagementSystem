package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.modmappings.mmms.er2dbc.relational.core.sql.Match;
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.lang.Nullable;

public class ConditionVisitor extends TypedSubtreeVisitor<Condition> implements PartRenderer {

    private final RenderContext context;
    private StringBuilder builder = new StringBuilder();

    ConditionVisitor(RenderContext context) {
        this.context = context;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterMatched(Condition segment) {

        DelegatingVisitor visitor = getDelegation(segment);

        return visitor != null ? Delegation.delegateTo(visitor) : Delegation.retain();
    }

    @Nullable
    private DelegatingVisitor getDelegation(Condition segment) {

        if (segment instanceof AndCondition) {
            return new MultiConcatConditionVisitor(context, (AndCondition) segment, builder::append);
        }

        if (segment instanceof OrCondition) {
            return new MultiConcatConditionVisitor(context, (OrCondition) segment, builder::append);
        }

        if (segment instanceof IsNull) {
            return new IsNullVisitor(context, builder::append);
        }

        if (segment instanceof Comparison) {
            return new ComparisonVisitor(context, (Comparison) segment, builder::append);
        }

        if (segment instanceof Like) {
            return new LikeVisitor((Like) segment, context, builder::append);
        }

        if (segment instanceof Match) {
            return new MatchVisitor((Match) segment, context, builder::append);
        }

        if (segment instanceof In) {
            return new InVisitor(context, builder::append);
        }

        return null;
    }

    /*
     * (non-Javadoc)
     * @see PartRenderer#getRenderedPart()
     */
    @Override
    public CharSequence getRenderedPart() {
        return builder;
    }
}
