package org.modmappings.mmms.er2dbc.data.statements.select;

import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.DistinctOnExpression;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expression;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.sort.SortSpec;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import java.util.*;
import java.util.function.Supplier;
import java.util.stream.Collectors;

public class SelectSpecWithJoin {

    private final boolean distinct;
    private final String table;
    private final Collection<JoinSpec> joinSpecs;
    private final DistinctOnExpression distinctOnExpression;
    private final List<Expression> projectedFields;
    @Nullable
    private final ColumnBasedCriteria criteria;
    private final SortSpec sort;
    private final Pageable page;

    public SelectSpecWithJoin(final boolean distinct, final String table, final Collection<JoinSpec> joinSpecs, final DistinctOnExpression distinctOnExpression, final List<Expression> projectedFields, @Nullable final ColumnBasedCriteria criteria, final SortSpec sort, final Pageable page) {
        this.distinct = distinct;
        this.table = table;
        this.joinSpecs = joinSpecs;
        this.distinctOnExpression = distinctOnExpression;
        this.projectedFields = projectedFields;
        this.criteria = criteria;
        this.sort = sort;
        this.page = page;
    }

    public static SelectSpecWithJoin create(final String table) {
        return new SelectSpecWithJoin(false, table, Collections.emptyList(), null, Collections.emptyList(), null, SortSpec.UNSORTED, Pageable.unpaged());
    }

    public static SelectSpecWithJoin createDistinct(final String table) {
        return new SelectSpecWithJoin(true, table, Collections.emptyList(), null, Collections.emptyList(), null, SortSpec.UNSORTED, Pageable.unpaged());
    }

    /**
     * Associate {@code projectedFields} with the select as distinct on call and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withDistinctOn(final Expression... projectedFields) {
        final List<Expression> expressions = Arrays.asList(projectedFields);
        Assert.noNullElements(expressions, "Expressions is not allowed to contain null elements!");
        Assert.isTrue(expressions.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All expressions need to be references!");

        return this.withDistinctOn(expressions);
    }

    /**
     * Associate {@code projectedFields} with the select as distinct on call and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withDistinctOnFromColumnNames(final Collection<String> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");

        final List<Expression> fields = new ArrayList<>();
        if (this.distinctOnExpression != null)
            fields.addAll(this.distinctOnExpression.getSource());
        fields.addAll(projectedFields.stream().map(Expressions::reference).collect(Collectors.toList()));

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, new DistinctOnExpression(fields), fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withDistinctOn(final Collection<Expression> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");
        Assert.isTrue(projectedFields.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All ProjectedFields need to be references!");

        final List<Expression> fields = new ArrayList<>();
        if (this.distinctOnExpression != null)
            fields.addAll(this.distinctOnExpression.getSource());
        fields.addAll(projectedFields);

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, new DistinctOnExpression(fields), fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withProjection(final Expression... projectedFields) {
        final List<Expression> expressions = Arrays.asList(projectedFields);
        Assert.noNullElements(expressions, "Expressions is not allowed to contain null elements!");
        Assert.isTrue(expressions.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All expressions need to be references!");

        return this.withProjection(expressions);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withProjectionFromColumnName(final Collection<String> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");

        final List<Expression> fields = new ArrayList<>(this.projectedFields);
        fields.addAll(projectedFields.stream().map(Expressions::reference).collect(Collectors.toList()));

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withProjection(final Collection<Expression> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");
        Assert.isTrue(projectedFields.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All ProjectedFields need to be references!");

        final List<Expression> fields = new ArrayList<>(this.projectedFields);
        fields.addAll(projectedFields);

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin select(final Expression... projectedFields) {
        final List<Expression> expressions = Arrays.asList(projectedFields);
        Assert.noNullElements(expressions, "Expressions is not allowed to contain null elements!");
        Assert.isTrue(expressions.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All expressions need to be references!");

        return this.withProjection(expressions);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin selectColumns(final Collection<String> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");

        final List<Expression> fields = new ArrayList<>(this.projectedFields);
        fields.addAll(projectedFields.stream().map(Expressions::reference).collect(Collectors.toList()));

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin select(final Collection<Expression> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");
        Assert.isTrue(projectedFields.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All ProjectedFields need to be references!");

        final List<Expression> fields = new ArrayList<>(this.projectedFields);
        fields.addAll(projectedFields);

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, fields, this.criteria, this.sort, this.page);
    }


    /**
     * Override {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin setProjection(final Expression... projectedFields) {
        final List<Expression> expressions = Arrays.asList(projectedFields);
        Assert.noNullElements(expressions, "Expressions is not allowed to contain null elements!");
        Assert.isTrue(expressions.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All expressions need to be references!");

        return this.setProjection(expressions);
    }

    /**
     * Override {@code projectedFields} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param projectedFields
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin setProjectionFromColumnName(final Collection<String> projectedFields) {
        Assert.notNull(projectedFields, "ProjectedFields can not be null!");
        Assert.noNullElements(projectedFields, "ProjectedFields is not allowed to contain null elements!");

        final List<Expression> fields = projectedFields.stream().map(Expressions::reference).collect(Collectors.toList());

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, fields, this.criteria, this.sort, this.page);
    }

    /**
     * Override {@code expressions} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param expressions Expressions that form the projections in the statement.
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin setProjection(final Collection<Expression> expressions) {
        Assert.notNull(expressions, "ProjectedFields can not be null!");
        Assert.noNullElements(expressions, "ProjectedFields is not allowed to contain null elements!");
        Assert.isTrue(expressions.stream().allMatch(ex -> ex.isReference() || ex.isNative() || ex.isFunction() || ex.isDistinct() || ex.isAliased()), "All expressions need to be references!");

        final List<Expression> fields = new ArrayList<>(expressions);

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, fields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate a {@link ColumnBasedCriteria} with the select and return a new {@link SelectSpecWithJoin}.
     *
     * @param criteria
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withCriteria(final ColumnBasedCriteria criteria) {
        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, this.projectedFields, criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin where(final Supplier<ColumnBasedCriteria> criteriaSupplier) {
        final ColumnBasedCriteria criteria = criteriaSupplier.get();
        if (criteria != null) {
            return withCriteria(criteria);
        }

        return this;
    }

    public SelectSpecWithJoin withJoin(final JoinSpec join) {
        if (join instanceof JoinSpec.NoopJoinSpec)
            return this;

        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.add(join);

        return new SelectSpecWithJoin(distinct, this.table, joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin withJoin(final JoinSpec... joins) {
        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.addAll(Arrays.stream(joins).filter(s -> !(s instanceof JoinSpec.NoopJoinSpec)).collect(Collectors.toSet()));

        return new SelectSpecWithJoin(distinct, this.table, joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin withJoin(final Collection<JoinSpec> joins) {
        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.addAll(joins.stream().filter(s -> !(s instanceof JoinSpec.NoopJoinSpec)).collect(Collectors.toSet()));

        return new SelectSpecWithJoin(distinct, this.table, joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin join(final Supplier<JoinSpec> join) {
        return this.withJoin(join.get());
    }

    public SelectSpecWithJoin join(final Supplier<JoinSpec>... joins) {
        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.addAll(Arrays.stream(joins).map(Supplier::get).filter(s -> !(s instanceof JoinSpec.NoopJoinSpec)).collect(Collectors.toList()));

        return new SelectSpecWithJoin(distinct, this.table, joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin join(final Collection<Supplier<JoinSpec>> joins) {
        final Collection<JoinSpec> joinSpecs = new ArrayList<>(this.joinSpecs);
        joinSpecs.addAll(joins.stream().map(Supplier::get).filter(s -> !(s instanceof JoinSpec.NoopJoinSpec)).collect(Collectors.toList()));

        return new SelectSpecWithJoin(distinct, this.table, joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, this.page);
    }

    public SelectSpecWithJoin distinct() {
        return withDistinct(true);
    }

    public SelectSpecWithJoin notDistinct() {
        return withDistinct(false);
    }

    public SelectSpecWithJoin withDistinct(final boolean distinct) {
        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, this.page);
    }


    /**
     * Associate {@link Sort} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param sort
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withSort(final SortSpec sort) {

        if (!sort.isUnsorted()) {
            return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, sort, this.page);
        }

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, this.page);
    }

    /**
     * Associate a {@link Pageable} with the select and create a new {@link SelectSpecWithJoin}.
     *
     * @param page
     * @return the {@link SelectSpecWithJoin}.
     */
    public SelectSpecWithJoin withPage(final Pageable page) {

        if (page.isPaged()) {

            final Sort sort = page.getSort();
            final SortSpec sortSpec = SortSpec.sort(sort);

            return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, !this.sort.isUnsorted() || sort.isUnsorted() ? this.sort : sortSpec,
                    page);
        }

        return new SelectSpecWithJoin(distinct, this.table, this.joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, this.sort, page);
    }

    public SelectSpecWithJoin clearSortAndPage() {
        return new SelectSpecWithJoin(this.distinct, this.table, this.joinSpecs, distinctOnExpression, this.projectedFields, this.criteria, SortSpec.UNSORTED, Pageable.unpaged());
    }

    public boolean isDistinct() {
        return distinct;
    }

    public String getTable() {
        return this.table;
    }

    public Collection<JoinSpec> getJoinSpecs() {
        return joinSpecs;
    }

    public List<Expression> getProjectedFields() {
        final List<Expression> projections = new ArrayList<>();
        if (this.distinctOnExpression != null)
            projections.add(this.distinctOnExpression);

        projections.addAll(this.projectedFields);

        return Collections.unmodifiableList(projections);
    }

    @Nullable
    public ColumnBasedCriteria getCriteria() {
        return this.criteria;
    }

    public SortSpec getSort() {
        return this.sort;
    }

    public Pageable getPage() {
        return this.page;
    }
}
