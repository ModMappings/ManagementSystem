package org.modmappings.mmms.er2dbc.data.statements.join;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;
import org.springframework.util.StringUtils;

import java.util.function.Supplier;

public class JoinSpec {

    private final JoinType type;
    private final String tableName;
    private final String tableAlias;
    @Nullable
    private ColumnBasedCriteria onCriteria = null;

    private JoinSpec(final JoinType type, final String tableName, final String tableAlias) {
        this.type = type;
        this.tableName = tableName;
        this.tableAlias = tableAlias;
    }

    public static JoinSpec join(final String tableName, final String tableAlias) {
        return new JoinSpec(JoinType.JOIN, tableName, tableAlias);
    }

    public static JoinSpec crossJoin(final String tableName, final String tableAlias) {
        return new JoinSpec(JoinType.CROSS_JOIN, tableName, tableAlias);
    }

    public static JoinSpec leftOuterJoin(final String tableName, final String tableAlias) {
        return new JoinSpec(JoinType.LEFT_OUTER_JOIN, tableName, tableAlias);
    }

    public static JoinSpec rightOuterJoin(final String tableName, final String tableAlias) {
        return new JoinSpec(JoinType.RIGHT_OUTER_JOIN, tableName, tableAlias);
    }

    public static JoinSpec fullOuterJoin(final String tableName, final String tableAlias) {
        return new JoinSpec(JoinType.FULL_OUTER_JOIN, tableName, tableAlias);
    }

    public JoinSpec withOn(final ColumnBasedCriteria columnBasedCriteria)
    {
        Assert.isTrue(columnBasedCriteria.getType() == ColumnBasedCriteria.Type.ON, "On statement criteria needs to be of on type! use columnBasedCriteria.on() to create one!");
        this.onCriteria = columnBasedCriteria;

        return this;
    }

    public JoinSpec on(final Supplier<ColumnBasedCriteria> columnBasedCriteriaSupplier) {
        final ColumnBasedCriteria criteria = columnBasedCriteriaSupplier.get();
        if (criteria != null) {
            return withOn(criteria);
        }
        return this;
    }

    public JoinType getType() {
        return type;
    }

    public String getTableName() {
        return tableName;
    }

    public String getTableAlias() {
        return tableAlias;
    }

    public boolean isAliased() {
        return StringUtils.isEmpty(tableAlias);
    }

    @Nullable
    public ColumnBasedCriteria getOn() {
        return onCriteria;
    }

    public enum JoinType {

        /**
         * {@code INNER JOIN} for two tables.
         */

        JOIN,

        /**
         * {@code CROSS JOIN} for two tables.
         */

        CROSS_JOIN,

        /**
         * {@code LEFT OUTER JOIN} two tables.
         */

        LEFT_OUTER_JOIN,

        /**
         * {@code RIGHT OUTER JOIN} two tables.
         */

        RIGHT_OUTER_JOIN,

        /**
         * {@code FULL OUTER JOIN} two tables.
         */

        FULL_OUTER_JOIN;
    }
}
