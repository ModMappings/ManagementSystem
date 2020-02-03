package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;


import org.springframework.data.relational.core.sql.Select;
import org.springframework.data.relational.core.sql.render.NamingStrategies;

public class SqlWithJoinSpecificSqlRenderer {
    public String render(Select select) {
        SelectWithJoinStatementVisitor visitor = new SelectWithJoinStatementVisitor(new SimpleRenderContext(NamingStrategies.asIs()));
        select.visit(visitor);

        return visitor.getRenderedPart().toString();
    }

}
