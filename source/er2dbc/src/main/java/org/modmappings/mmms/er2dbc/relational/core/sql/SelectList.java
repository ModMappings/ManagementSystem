package org.modmappings.mmms.er2dbc.relational.core.sql;

import org.springframework.data.relational.core.sql.Expression;
import org.springframework.util.StringUtils;

import java.util.List;

public class SelectList extends AbstractSegment {

    private final List<Expression> selectList;

    SelectList(List<Expression> selectList) {
        super(selectList.toArray(new Expression[0]));
        this.selectList = selectList;
    }

    /*
     * (non-Javadoc)
     * @see java.lang.Object#toString()
     */
    @Override
    public String toString() {
        return StringUtils.collectionToDelimitedString(selectList, ", ");
    }
}

