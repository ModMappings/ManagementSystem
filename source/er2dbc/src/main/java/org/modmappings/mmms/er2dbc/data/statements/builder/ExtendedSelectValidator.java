package org.modmappings.mmms.er2dbc.data.statements.builder;

import org.modmappings.mmms.er2dbc.relational.core.sql.From;
import org.modmappings.mmms.er2dbc.relational.core.sql.Join;
import org.modmappings.mmms.er2dbc.relational.core.sql.Select;
import org.modmappings.mmms.er2dbc.relational.core.sql.Where;
import org.springframework.data.relational.core.sql.*;
import org.springframework.lang.Nullable;

import java.util.HashSet;
import java.util.Set;
import java.util.Stack;

/**
 * This class mirrors the logic from {@code SelectValidator} in R2DBC
 * It's implementation merges the functionality from it and its parent class into one
 * And allows this library to access it from anywhere.
 */
public class ExtendedSelectValidator implements Visitor {

    private Set<Table> requiredByWhere = new HashSet<>();
    private Set<Table> from = new HashSet<>();
    @Nullable
    private Visitable parent;

    private final Stack<Select> selects = new Stack<>();

    private int selectFieldCount;
    private Set<Table> requiredBySelect = new HashSet<>();
    private Set<Table> requiredByOrderBy = new HashSet<>();

    private Set<Table> join = new HashSet<>();

    /**
     * Validates a {@link Select} statement.
     *
     * @param select the {@link Select} statement.
     * @throws IllegalStateException if the statement is not valid.
     */
    public static void validate(Select select) {
        new ExtendedSelectValidator().doValidate(select);
    }

    private void doValidate(Select select) {

        select.visit(this);

        if (selectFieldCount == 0) {
            throw new IllegalStateException("SELECT does not declare a select list");
        }

        for (Table table : requiredBySelect) {
            if (!join.contains(table) && !from.contains(table)) {
                throw new IllegalStateException(String
                        .format("Required table [%s] by a SELECT column not imported by FROM %s or JOIN %s", table, from, join));
            }
        }

        for (Table table : requiredByWhere) {
            if (!join.contains(table) && !from.contains(table)) {
                throw new IllegalStateException(String
                        .format("Required table [%s] by a WHERE predicate not imported by FROM %s or JOIN %s", table, from, join));
            }
        }

        for (Table table : requiredByOrderBy) {
            if (!join.contains(table) && !from.contains(table)) {
                throw new IllegalStateException(String
                        .format("Required table [%s] by a ORDER BY column not imported by FROM %s or JOIN %s", table, from, join));
            }
        }
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.Visitor#enter(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    public void enter(Visitable segment) {

        if (segment instanceof Select) {
            selects.push((Select) segment);
        }

        if (selects.size() > 1) {
            return;
        }

        if (segment instanceof Expression && parent instanceof Select) {
            selectFieldCount++;
        }

        if (segment instanceof AsteriskFromTable && parent instanceof Select) {

            Table table = ((AsteriskFromTable) segment).getTable();
            requiredBySelect.add(table);
        }

        if (segment instanceof Column && (parent instanceof Select || parent instanceof SimpleFunction)) {

            Table table = ((Column) segment).getTable();

            if (table != null) {
                requiredBySelect.add(table);
            }
        }

        if (segment instanceof Column && parent instanceof OrderByField) {

            Table table = ((Column) segment).getTable();

            if (table != null) {
                requiredByOrderBy.add(table);
            }
        }

        if (segment instanceof Table && parent instanceof Join) {
            join.add((Table) segment);
        }

        if (segment instanceof Table && parent instanceof From) {
            from.add((Table) segment);
        }

        if (segment instanceof Where) {
            segment.visit(new ExtendedSelectValidator.SubselectFilteringWhereVisitor());
        }

        if (segment instanceof Join || segment instanceof OrderByField || segment instanceof From
                || segment instanceof Select || segment instanceof Where || segment instanceof SimpleFunction) {
            parent = segment;
        }
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.relational.core.sql.AbstractImportValidator#leave(org.springframework.data.relational.core.sql.Visitable)
     */
    @Override
    public void leave(Visitable segment) {



        if (segment instanceof Select) {
            selects.remove(segment);
        }

        if (selects.size() > 1) {
            return;
        }

    }

    /**
     * {@link Visitor} that skips sub-{@link Select} and collects columns within a {@link Where} clause.
     */
    class SubselectFilteringWhereVisitor implements Visitor {

        private @Nullable Select selectFilter;

        /*
         * (non-Javadoc)
         * @see org.springframework.data.relational.core.sql.Visitor#enter(org.springframework.data.relational.core.sql.Visitable)
         */
        @Override
        public void enter(Visitable segment) {

            if (selectFilter != null) {
                return;
            }

            if (segment instanceof Select) {
                this.selectFilter = (Select) segment;
                return;
            }

            if (segment instanceof Table) {
                requiredByWhere.add((Table) segment);
            }
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.relational.core.sql.Visitor#leave(org.springframework.data.relational.core.sql.Visitable)
         */
        @Override
        public void leave(Visitable segment) {

            if (this.selectFilter == segment) {
                this.selectFilter = null;
            }
        }
    }
}
