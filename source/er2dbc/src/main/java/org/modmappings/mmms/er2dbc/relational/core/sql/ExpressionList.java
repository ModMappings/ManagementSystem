package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Expression;
import org.springframework.util.StringUtils;

import java.util.List;

public class ExpressionList extends AbstractSegment {

    private final Mode mode;
    private final List<Expression> expressionList;

    ExpressionList(final Mode mode, final List<Expression> expressionList) {
        super(expressionList.toArray(new Expression[0]));
        this.mode = mode;
        this.expressionList = expressionList;
    }

    public Mode getMode() {
        return mode;
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public String toString() {
        return StringUtils.collectionToDelimitedString(expressionList, ", ");
    }

    public enum Mode {
        SELECT,
        ORDER_BY
    }
}

