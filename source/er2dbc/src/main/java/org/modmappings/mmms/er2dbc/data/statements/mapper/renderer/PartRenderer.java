package org.modmappings.mmms.er2dbc.data.statements.mapper.renderer;

import org.springframework.data.relational.core.sql.Visitor;

interface PartRenderer extends Visitor {

    /**
     * Returns the rendered part.
     *
     * @return the rendered part.
     */
    CharSequence getRenderedPart();
}