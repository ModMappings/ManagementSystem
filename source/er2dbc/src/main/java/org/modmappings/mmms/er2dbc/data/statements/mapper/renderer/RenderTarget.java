package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

@FunctionalInterface
interface RenderTarget {

    /**
     * Callback method that is invoked once the rendering for a part or expression is finished. When called multiple
     * times, it's the responsibility of the implementor to ensure proper concatenation of render results.
     *
     * @param sequence the rendered part or expression.
     */
    void onRendered(CharSequence sequence);
}
