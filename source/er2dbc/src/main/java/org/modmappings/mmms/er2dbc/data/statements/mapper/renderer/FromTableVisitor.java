package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Aliased;
import org.springframework.data.relational.core.sql.Table;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class FromTableVisitor extends TypedSubtreeVisitor<Table> {

    private final RenderContext context;
    private final RenderTarget parent;

    FromTableVisitor(RenderContext context, RenderTarget parent) {
        super();
        this.context = context;
        this.parent = parent;
    }

    /*
     * (non-Javadoc)
     * @see TypedSubtreeVisitor#enterMatched(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    Delegation enterMatched(Table segment) {

        StringBuilder builder = new StringBuilder();

        builder.append(context.getNamingStrategy().getName(segment));
        if (segment instanceof Aliased) {
            builder.append(" ").append(((Aliased) segment).getAlias());
        }

        parent.onRendered(builder);

        return super.enterMatched(segment);
    }
}
