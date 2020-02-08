package org.modmappings.mmms.er2dbc.data.statements.mapper;

import org.modmappings.mmms.er2dbc.data.query.mapper.ExtendedMapper;
import org.modmappings.mmms.er2dbc.data.statements.builder.ExtendedSelectBuilder;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.mapper.renderer.SqlWithJoinSpecificSqlRenderer;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.er2dbc.relational.core.sql.Join;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.mapping.context.MappingContext;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.r2dbc.core.StatementMapper;
import org.springframework.data.r2dbc.dialect.BindMarkers;
import org.springframework.data.r2dbc.dialect.BindTarget;
import org.springframework.data.r2dbc.dialect.Bindings;
import org.springframework.data.r2dbc.dialect.R2dbcDialect;
import org.springframework.data.r2dbc.query.BoundAssignments;
import org.springframework.data.r2dbc.query.BoundCondition;
import org.springframework.data.relational.core.mapping.RelationalPersistentEntity;
import org.springframework.data.relational.core.mapping.RelationalPersistentProperty;
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.relational.core.sql.render.RenderContext;
import org.springframework.data.relational.core.sql.render.SqlRenderer;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;

/**
 * This custom implementation of the {@link StatementMapper} adds support for
 * join based select statements.
 */
public class ExtendedStatementMapper implements StatementMapper {

    private final R2dbcDialect dialect;
    private final RenderContext renderContext;
    private final ExtendedMapper extendedMapper;
    private final MappingContext<RelationalPersistentEntity<?>, ? extends RelationalPersistentProperty> mappingContext;

    public ExtendedStatementMapper(final R2dbcDialect dialect, final RenderContext renderContext, final ExtendedMapper extendedMapper, final MappingContext<RelationalPersistentEntity<?>, ? extends RelationalPersistentProperty> mappingContext) {
        this.dialect = dialect;
        this.renderContext = renderContext;
        this.extendedMapper = extendedMapper;
        this.mappingContext = mappingContext;
    }

    @Override
    public <T> Typed<T> forType(final Class<T> type) {

        Assert.notNull(type, "Type must not be null!");

        return new ExtendedStatementMapper.Typed<T>(
                dialect,
                renderContext,
                extendedMapper,
                mappingContext,
                (RelationalPersistentEntity<T>) this.mappingContext.getRequiredPersistentEntity(type));
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.StatementMapper#getMappedObject(org.springframework.data.r2dbc.function.StatementMapper.SelectSpec)
     */
    @Override
    public PreparedOperation<Select> getMappedObject(final SelectSpec selectSpec) {
        return getMappedObject(selectSpec, null);
    }

    private PreparedOperation<Select> getMappedObject(final SelectSpec selectSpec,
                                                      @Nullable final RelationalPersistentEntity<?> entity) {

        final Table table = Table.create(selectSpec.getTable());
        final List<Column> columns = table.columns(selectSpec.getProjectedFields());
        final SelectBuilder.SelectFromAndJoin selectBuilder = StatementBuilder.select(columns).from(table);

        final BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        Bindings bindings = Bindings.empty();

        if (selectSpec.getCriteria() != null) {

            final BoundCondition mappedObject = this.extendedMapper.getMappedObject(bindMarkers, selectSpec.getCriteria(), table,
                    entity);

            bindings = mappedObject.getBindings();
            selectBuilder.where(mappedObject.getCondition());
        }

        if (selectSpec.getSort().isSorted()) {

            final Sort mappedSort = this.extendedMapper.getMappedObject(selectSpec.getSort(), entity);
            selectBuilder.orderBy(createOrderByFields(table, mappedSort));
        }

        if (selectSpec.getPage().isPaged()) {

            final Pageable page = selectSpec.getPage();

            selectBuilder.limitOffset(page.getPageSize(), page.getOffset());
        }

        final Select select = selectBuilder.build();
        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(select, this.renderContext, bindings);
    }

    public PreparedOperation<Select> getMappedObject(final SelectSpecWithJoin selectSpecWithJoin) {
        return getMappedObject(selectSpecWithJoin, null);
    }

    private PreparedOperation<Select> getMappedObject(final SelectSpecWithJoin selectSpecWithJoin,
                                                      @Nullable final RelationalPersistentEntity<?> entity) {

        final Table table = Table.create(selectSpecWithJoin.getTable());
        final List<Expression> projectExpressions = selectSpecWithJoin.getProjectedFields().stream().map(e -> extendedMapper.getMappedObject(e, table)).collect(Collectors.toList());
        final ExtendedSelectBuilder selectBuilder = new ExtendedSelectBuilder().select(projectExpressions).from(table);

        final BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        Bindings bindings = Bindings.empty();

        if (selectSpecWithJoin.getJoinSpecs()!= null && !selectSpecWithJoin.getJoinSpecs().isEmpty()) {
            for (final JoinSpec joinSpec : selectSpecWithJoin.getJoinSpecs()) {
                final Table tableToJoin = joinSpec.isAliased() ? Table.aliased(joinSpec.getTableName(), joinSpec.getTableAlias()) :
                        Table.create(joinSpec.getTableName());

                final org.springframework.data.relational.core.sql.Join.JoinType remappedJoinType = extendedMapper.getMappedObject(joinSpec.getType());
                final BoundCondition boundCondition = extendedMapper.getMappedObject(bindMarkers, joinSpec.getOn(), table, entity);

                selectBuilder.join(new Join(remappedJoinType, tableToJoin, boundCondition.getCondition()));
                bindings = bindings.and(boundCondition.getBindings());
            }
        }

        if (selectSpecWithJoin.getCriteria() != null) {

            final BoundCondition mappedObject = this.extendedMapper.getMappedObject(bindMarkers, selectSpecWithJoin.getCriteria(), table,
                    entity);

            bindings = bindings.and(mappedObject.getBindings());
            selectBuilder.where(mappedObject.getCondition());
        }

        if (selectSpecWithJoin.getSort().isSorted()) {

            final Sort mappedSort = this.extendedMapper.getMappedObject(selectSpecWithJoin.getSort(), entity);
            selectBuilder.orderBy(createOrderByFields(table, mappedSort));
        }

        if (selectSpecWithJoin.getPage().isPaged()) {

            final Pageable page = selectSpecWithJoin.getPage();

            selectBuilder.limitOffset(page.getPageSize(), page.getOffset());
        }

        final Select select = selectBuilder.build();
        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(select, this.renderContext, bindings);
    }


    private Collection<? extends OrderByField> createOrderByFields(final Table table, final Sort sortToUse) {

        final List<OrderByField> fields = new ArrayList<>();

        for (final Sort.Order order : sortToUse) {

            final OrderByField orderByField = OrderByField.from(table.column(order.getProperty()));

            if (order.getDirection() != null) {
                fields.add(order.isAscending() ? orderByField.asc() : orderByField.desc());
            } else {
                fields.add(orderByField);
            }
        }

        return fields;
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.StatementMapper#getMappedObject(org.springframework.data.r2dbc.function.StatementMapper.InsertSpec)
     */
    @Override
    public PreparedOperation<Insert> getMappedObject(final InsertSpec insertSpec) {
        return getMappedObject(insertSpec, null);
    }

    private PreparedOperation<Insert> getMappedObject(final InsertSpec insertSpec,
                                                      @Nullable final RelationalPersistentEntity<?> entity) {

        final BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        final Table table = Table.create(insertSpec.getTable());

        final BoundAssignments boundAssignments = this.extendedMapper.getMappedObject(bindMarkers, insertSpec.getAssignments(),
                table, entity);

        final Bindings bindings;

        if (boundAssignments.getAssignments().isEmpty()) {
            throw new IllegalStateException("INSERT contains no values");
        }

        bindings = boundAssignments.getBindings();

        final InsertBuilder.InsertIntoColumnsAndValues insertBuilder = StatementBuilder.insert(table);
        InsertBuilder.InsertValuesWithBuild withBuild = (InsertBuilder.InsertValuesWithBuild) insertBuilder;

        for (final Assignment assignment : boundAssignments.getAssignments()) {

            if (assignment instanceof AssignValue) {
                final AssignValue assignValue = (AssignValue) assignment;

                insertBuilder.column(assignValue.getColumn());
                withBuild = insertBuilder.value(assignValue.getValue());
            }
        }

        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(withBuild.build(), this.renderContext, bindings);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.StatementMapper#getMappedObject(org.springframework.data.r2dbc.function.StatementMapper.UpdateSpec)
     */
    @Override
    public PreparedOperation<org.springframework.data.relational.core.sql.Update> getMappedObject(final UpdateSpec updateSpec) {
        return getMappedObject(updateSpec, null);
    }

    private PreparedOperation<org.springframework.data.relational.core.sql.Update> getMappedObject(final UpdateSpec updateSpec,
                                                                                                   @Nullable final RelationalPersistentEntity<?> entity) {

        final BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        final Table table = Table.create(updateSpec.getTable());

        final BoundAssignments boundAssignments = this.extendedMapper.getMappedObject(bindMarkers,
                updateSpec.getUpdate().getAssignments(), table, entity);

        Bindings bindings;

        if (boundAssignments.getAssignments().isEmpty()) {
            throw new IllegalStateException("UPDATE contains no assignments");
        }

        bindings = boundAssignments.getBindings();

        final UpdateBuilder.UpdateWhere updateBuilder = StatementBuilder.update(table).set(boundAssignments.getAssignments());

        final Update update;

        if (updateSpec.getCriteria() != null) {

            final BoundCondition boundCondition = this.extendedMapper.getMappedObject(bindMarkers, updateSpec.getCriteria(), table,
                    entity);

            bindings = bindings.and(boundCondition.getBindings());
            update = updateBuilder.where(boundCondition.getCondition()).build();
        } else {
            update = updateBuilder.build();
        }

        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(update, this.renderContext, bindings);
    }

    /*
     * (non-Javadoc)
     * @see org.springframework.data.r2dbc.function.StatementMapper#getMappedObject(org.springframework.data.r2dbc.function.StatementMapper.DeleteSpec)
     */
    @Override
    public PreparedOperation<Delete> getMappedObject(final DeleteSpec deleteSpec) {
        return getMappedObject(deleteSpec, null);
    }

    private PreparedOperation<Delete> getMappedObject(final DeleteSpec deleteSpec,
                                                      @Nullable final RelationalPersistentEntity<?> entity) {

        final BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        final Table table = Table.create(deleteSpec.getTable());

        final DeleteBuilder.DeleteWhere deleteBuilder = StatementBuilder.delete(table);

        Bindings bindings = Bindings.empty();

        final Delete delete;
        if (deleteSpec.getCriteria() != null) {

            final BoundCondition boundCondition = this.extendedMapper.getMappedObject(bindMarkers, deleteSpec.getCriteria(), table,
                    entity);

            bindings = boundCondition.getBindings();
            delete = deleteBuilder.where(boundCondition.getCondition()).build();
        } else {
            delete = deleteBuilder.build();
        }

        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(delete, this.renderContext, bindings);
    }

    public SelectSpecWithJoin createSelectWithJoin(final String table) {
        return SelectSpecWithJoin.create(table);
    }

    /**
     * Extended implementation of {@link PreparedOperation}.
     *
     * @param <T>
     */
    private class ExtendedPreparedOperation<T> implements PreparedOperation<T> {

        private final T source;
        private final RenderContext renderContext;
        private final Bindings bindings;

        public ExtendedPreparedOperation(final T source, final RenderContext renderContext, final Bindings bindings) {
            this.source = source;
            this.renderContext = renderContext;
            this.bindings = bindings;
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.PreparedOperation#getSource()
         */
        @Override
        public T getSource() {
            return this.source;
        }

        /*
         * (non-Javadoc)
         * @see org.springframework.data.r2dbc.function.QueryOperation#toQuery()
         */
        @Override
        public String toQuery() {

            final SqlRenderer sqlRenderer = SqlRenderer.create(this.renderContext);

            if (this.source instanceof Select) {
                if (this.source instanceof org.modmappings.mmms.er2dbc.relational.core.sql.Select)
                {
                    final SqlWithJoinSpecificSqlRenderer specificSqlRenderer = new SqlWithJoinSpecificSqlRenderer();
                    return specificSqlRenderer.render((Select) this.source);
                }

                return sqlRenderer.render((Select) this.source);
            }

            if (this.source instanceof Insert) {
                return sqlRenderer.render((Insert) this.source);
            }

            if (this.source instanceof Update) {
                return sqlRenderer.render((Update) this.source);
            }

            if (this.source instanceof Delete) {
                return sqlRenderer.render((Delete) this.source);
            }

            throw new IllegalStateException("Cannot render " + this.getSource());
        }

        @Override
        public void bindTo(final BindTarget to) {
            this.bindings.apply(to);
        }
    }

    private class Typed<T> extends ExtendedStatementMapper implements TypedStatementMapper<T> {

        final RelationalPersistentEntity<T> entity;

        public Typed(final R2dbcDialect dialect, final RenderContext renderContext, final ExtendedMapper extendedMapper, final MappingContext<RelationalPersistentEntity<?>, ? extends RelationalPersistentProperty> mappingContext, final RelationalPersistentEntity<T> entity) {
            super(dialect, renderContext, extendedMapper, mappingContext);
            this.entity = entity;
        }

        @Override
        public <TC> Typed<TC> forType(final Class<TC> type) {
            return ExtendedStatementMapper.this.forType(type);
        }

        @Override
        public PreparedOperation<Select> getMappedObject(final SelectSpec selectSpec) {
            return ExtendedStatementMapper.this.getMappedObject(selectSpec, entity);
        }

        public PreparedOperation<Select> getMappedObject(final SelectSpecWithJoin selectSpecWithJoin) {
            return ExtendedStatementMapper.this.getMappedObject(selectSpecWithJoin, entity);
        }

        @Override
        public PreparedOperation<Insert> getMappedObject(final InsertSpec insertSpec) {
            return ExtendedStatementMapper.this.getMappedObject(insertSpec, entity);
        }

        @Override
        public PreparedOperation<Update> getMappedObject(final UpdateSpec updateSpec) {
            return ExtendedStatementMapper.this.getMappedObject(updateSpec, entity);
        }

        @Override
        public PreparedOperation<Delete> getMappedObject(final DeleteSpec deleteSpec) {
            return ExtendedStatementMapper.this.getMappedObject(deleteSpec, entity);
        }
    }

}
