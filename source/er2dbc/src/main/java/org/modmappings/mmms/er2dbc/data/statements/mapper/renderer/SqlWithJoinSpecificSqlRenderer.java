package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;


import org.springframework.data.relational.core.sql.Select;
import org.springframework.data.relational.core.sql.render.NamingStrategies;
import org.springframework.data.relational.core.sql.render.RenderContext;

public class SqlWithJoinSpecificSqlRenderer {

    private final RenderContext renderContext;

    public SqlWithJoinSpecificSqlRenderer(final RenderContext renderContext) {
        this.renderContext = renderContext;
    }

    public String render(final Select select) {
        final SelectWithJoinStatementVisitor visitor = new SelectWithJoinStatementVisitor(this.renderContext);
        select.visit(visitor);

        return visitor.getRenderedPart().toString();
    }

}
