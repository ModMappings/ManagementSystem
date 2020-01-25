package org.modmappings.mmms.er2dbc.data.statements.select;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.relational.core.sql.Expressions;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import java.util.*;
import java.util.stream.Collectors;

public class SelectSpecWithJoin {

    private final String table;
    private final Collection<JoinSpec> joinSpecs;
    private final List<ColumnBasedCriteria.Expression> projectedFields;
    @Nullable
    private final ColumnBasedCriteria criteria;
    private final Sort sort;
    private final Pageable page;

    public SelectSpecWithJoin(String table, Collection<JoinSpec> joinSpecs, List<ColumnBasedCriteria.Expression> projectedFields, @Nullable ColumnBasedCriteria criteria, Sort sort, Pageable page) {
        this.table = table;
        this.joinSpecs = joinSpecs;
        this.projectedFields = projectedFields;
        this.criteria = criteria;
        this.sort = sort;
        this.page = page;
    }

    public static SelectSpecWithJoin create(String table) {
        return new SelectSpecWithJoin(table, Collections.emptyList(), Collections.emptyList(), null, Sort.unsorted(), Pageable.unpaged());
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withProjection(ColumnBasedCriteria.Expression... projectedFields) {
        final List<ColumnBasedCriteria.Expression> expressions = Arrays.asList(projectedFields);
        Assert.noNullElements(expressions, "Expressions is not allowed to contain null elements!");
        Assert.isTrue(expressions.stream().allMatch(ColumnBasedCriteria.Expression::isReference), "All expressions need to be references!");

        return this.withProjection(expressions);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withProjectionFromColumnName(Collection<String> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");

        List<ColumnBasedCriteria.Expression> fields = new ArrayList<>(this.projectedFields);
        fields.addAll(projectedFields.stream().map(ColumnBasedCriteria::reference).collect(Collectors.toList()));

        return new SelectSpecWithJoin(this.table, this.joinSpecs, fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withProjection(Collection<ColumnBasedCriteria.Expression> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");
        Assert.isTrue(projectedFields.stream().allMatch(ColumnBasedCriteria.Expression::isReference), "All ProjectedFields need to be references!");

        List<ColumnBasedCriteria.Expression> fields = new ArrayList<>(this.projectedFields);
        fields.addAll(projectedFields);

        return new SelectSpecWithJoin(this.table, this.joinSpecs, fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate a {@link ColumnBasedCriteria} with the select and return a new {@link SelectSpecWithJoin}.
     *
     * @param criteria
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withCriteria(ColumnBasedCriteria criteria) {
        return new SelectSpecWithJoin(this.table, this.joinSpecs, this.projectedFields, criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin withJoin(JoinSpec join)
    {
        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.add(join);

        return new SelectSpecWithJoin(this.table, joinSpecs, this.projectedFields, this.criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin withJoin(JoinSpec... joins)
    {
        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.addAll(Arrays.asList(joins));

        return new SelectSpecWithJoin(this.table, joinSpecs, this.projectedFields, this.criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin withJoin(Collection<JoinSpec> joins)
    {
        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.addAll(joins);

        return new SelectSpecWithJoin(this.table, joinSpecs, this.projectedFields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate {@link Sort} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param sort
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withSort(Sort sort) {

        if (sort.isSorted()) {
            return new SelectSpecWithJoin(this.table, this.joinSpecs, this.projectedFields, this.criteria, sort, this.page);
        }

        return new SelectSpecWithJoin(this.table, this.joinSpecs, this.projectedFields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate a {@link Pageable} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param page
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withPage(Pageable page) {

        if (page.isPaged()) {

            Sort sort = page.getSort();

            return new SelectSpecWithJoin(this.table, this.joinSpecs, this.projectedFields, this.criteria, sort.isSorted() ? sort : this.sort,
                    page);
        }

        return new SelectSpecWithJoin(this.table, this.joinSpecs, this.projectedFields, this.criteria, this.sort, page);
    }

    public String getTable() {
        return this.table;
    }

    public Collection<JoinSpec> getJoinSpecs() {
        return joinSpecs;
    }

    public List<ColumnBasedCriteria.Expression> getProjectedFields() {
        return Collections.unmodifiableList(this.projectedFields);
    }

    @Nullable
    public ColumnBasedCriteria getCriteria() {
        return this.criteria;
    }

    public Sort getSort() {
        return this.sort;
    }

    public Pageable getPage() {
        return this.page;
    }
}
