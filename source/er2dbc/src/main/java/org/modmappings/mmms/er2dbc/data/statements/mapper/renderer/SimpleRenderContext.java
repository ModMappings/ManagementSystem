package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.data.relational.core.sql.render.RenderNamingStrategy;
import org.springframework.data.relational.core.sql.render.SelectRenderContext;

public class SimpleRenderContext implements RenderContext {

    private final RenderNamingStrategy namingStrategy;

    public SimpleRenderContext(RenderNamingStrategy namingStrategy) {
        this.namingStrategy = namingStrategy;
    }

    @Override
    public RenderNamingStrategy getNamingStrategy() {
        return namingStrategy;
    }

    @Override
    public SelectRenderContext getSelect() {
        return DefaultSelectRenderContext.INSTANCE;
    }

    enum DefaultSelectRenderContext implements SelectRenderContext {
        INSTANCE;
    }

}