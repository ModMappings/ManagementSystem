package org.modmappings.mmms.er2dbc.data.statements.mapper;

import org.modmappings.mmms.er2dbc.data.query.mapper.ExtendedMapper;
import org.modmappings.mmms.er2dbc.data.statements.builder.ExtendedSelectBuilder;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.relational.core.sql.Join;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.mapping.context.MappingContext;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.r2dbc.core.StatementMapper;
import org.springframework.data.r2dbc.dialect.BindMarkers;
import org.springframework.data.r2dbc.dialect.BindTarget;
import org.springframework.data.r2dbc.dialect.Bindings;
import org.springframework.data.r2dbc.dialect.R2dbcDialect;
import org.springframework.data.r2dbc.query.*;
import org.springframework.data.relational.core.mapping.RelationalPersistentEntity;
import org.springframework.data.relational.core.mapping.RelationalPersistentProperty;
import org.springframework.data.relational.core.sql.*;
import org.springframework.data.relational.core.sql.Update;
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

    public ExtendedStatementMapper(R2dbcDialect dialect, RenderContext renderContext, ExtendedMapper extendedMapper, MappingContext<RelationalPersistentEntity<?>, ? extends RelationalPersistentProperty> mappingContext) {
        this.dialect = dialect;
        this.renderContext = renderContext;
        this.extendedMapper = extendedMapper;
        this.mappingContext = mappingContext;
    }

    @Override
    public <T> Typed<T> forType(Class<T> type) {

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
    public PreparedOperation<Select> getMappedObject(SelectSpec selectSpec) {
        return getMappedObject(selectSpec, null);
    }

    private PreparedOperation<Select> getMappedObject(SelectSpec selectSpec,
                                                      @Nullable RelationalPersistentEntity<?> entity) {

        Table table = Table.create(selectSpec.getTable());
        List<Column> columns = table.columns(selectSpec.getProjectedFields());
        SelectBuilder.SelectFromAndJoin selectBuilder = StatementBuilder.select(columns).from(table);

        BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        Bindings bindings = Bindings.empty();

        if (selectSpec.getCriteria() != null) {

            BoundCondition mappedObject = this.extendedMapper.getMappedObject(bindMarkers, selectSpec.getCriteria(), table,
                    entity);

            bindings = mappedObject.getBindings();
            selectBuilder.where(mappedObject.getCondition());
        }

        if (selectSpec.getSort().isSorted()) {

            Sort mappedSort = this.extendedMapper.getMappedObject(selectSpec.getSort(), entity);
            selectBuilder.orderBy(createOrderByFields(table, mappedSort));
        }

        if (selectSpec.getPage().isPaged()) {

            Pageable page = selectSpec.getPage();

            selectBuilder.limitOffset(page.getPageSize(), page.getOffset());
        }

        Select select = selectBuilder.build();
        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(select, this.renderContext, bindings);
    }

    public PreparedOperation<Select> getMappedObject(SelectSpecWithJoin selectSpecWithJoin) {
        return getMappedObject(selectSpecWithJoin, null);
    }

    private PreparedOperation<Select> getMappedObject(SelectSpecWithJoin selectSpecWithJoin,
                                                      @Nullable RelationalPersistentEntity<?> entity) {

        Table table = Table.create(selectSpecWithJoin.getTable());
        List<Expression> projectExpressions = selectSpecWithJoin.getProjectedFields().stream().map(e -> extendedMapper.getMappedObject(e, table)).collect(Collectors.toList());
        ExtendedSelectBuilder selectBuilder = new ExtendedSelectBuilder().select(projectExpressions).from(table);

        BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        Bindings bindings = Bindings.empty();

        if (selectSpecWithJoin.getJoinSpecs()!= null && !selectSpecWithJoin.getJoinSpecs().isEmpty()) {
            for (JoinSpec joinSpec : selectSpecWithJoin.getJoinSpecs()) {
                Table tableToJoin = joinSpec.isAliased() ? Table.aliased(joinSpec.getTableName(), joinSpec.getTableAlias()) :
                        Table.create(joinSpec.getTableName());

                org.springframework.data.relational.core.sql.Join.JoinType remappedJoinType = extendedMapper.getMappedObject(joinSpec.getType());
                BoundCondition boundCondition = extendedMapper.getMappedObject(bindMarkers, joinSpec.getOn(), table, entity);

                selectBuilder.join(new Join(remappedJoinType, tableToJoin, boundCondition.getCondition()));
                bindings = bindings.and(boundCondition.getBindings());
            }
        }

        if (selectSpecWithJoin.getCriteria() != null) {

            BoundCondition mappedObject = this.extendedMapper.getMappedObject(bindMarkers, selectSpecWithJoin.getCriteria(), table,
                    entity);

            bindings = bindings.and(mappedObject.getBindings());
            selectBuilder.where(mappedObject.getCondition());
        }

        if (selectSpecWithJoin.getSort().isSorted()) {

            Sort mappedSort = this.extendedMapper.getMappedObject(selectSpecWithJoin.getSort(), entity);
            selectBuilder.orderBy(createOrderByFields(table, mappedSort));
        }

        if (selectSpecWithJoin.getPage().isPaged()) {

            Pageable page = selectSpecWithJoin.getPage();

            selectBuilder.limitOffset(page.getPageSize(), page.getOffset());
        }

        Select select = selectBuilder.build();
        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(select, this.renderContext, bindings);
    }


    private Collection<? extends OrderByField> createOrderByFields(Table table, Sort sortToUse) {

        List<OrderByField> fields = new ArrayList<>();

        for (Sort.Order order : sortToUse) {

            OrderByField orderByField = OrderByField.from(table.column(order.getProperty()));

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
    public PreparedOperation<Insert> getMappedObject(InsertSpec insertSpec) {
        return getMappedObject(insertSpec, null);
    }

    private PreparedOperation<Insert> getMappedObject(InsertSpec insertSpec,
                                                      @Nullable RelationalPersistentEntity<?> entity) {

        BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        Table table = Table.create(insertSpec.getTable());

        BoundAssignments boundAssignments = this.extendedMapper.getMappedObject(bindMarkers, insertSpec.getAssignments(),
                table, entity);

        Bindings bindings;

        if (boundAssignments.getAssignments().isEmpty()) {
            throw new IllegalStateException("INSERT contains no values");
        }

        bindings = boundAssignments.getBindings();

        InsertBuilder.InsertIntoColumnsAndValues insertBuilder = StatementBuilder.insert(table);
        InsertBuilder.InsertValuesWithBuild withBuild = (InsertBuilder.InsertValuesWithBuild) insertBuilder;

        for (Assignment assignment : boundAssignments.getAssignments()) {

            if (assignment instanceof AssignValue) {
                AssignValue assignValue = (AssignValue) assignment;

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
    public PreparedOperation<org.springframework.data.relational.core.sql.Update> getMappedObject(UpdateSpec updateSpec) {
        return getMappedObject(updateSpec, null);
    }

    private PreparedOperation<org.springframework.data.relational.core.sql.Update> getMappedObject(UpdateSpec updateSpec,
                                                                                                   @Nullable RelationalPersistentEntity<?> entity) {

        BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        Table table = Table.create(updateSpec.getTable());

        BoundAssignments boundAssignments = this.extendedMapper.getMappedObject(bindMarkers,
                updateSpec.getUpdate().getAssignments(), table, entity);

        Bindings bindings;

        if (boundAssignments.getAssignments().isEmpty()) {
            throw new IllegalStateException("UPDATE contains no assignments");
        }

        bindings = boundAssignments.getBindings();

        UpdateBuilder.UpdateWhere updateBuilder = StatementBuilder.update(table).set(boundAssignments.getAssignments());

        Update update;

        if (updateSpec.getCriteria() != null) {

            BoundCondition boundCondition = this.extendedMapper.getMappedObject(bindMarkers, updateSpec.getCriteria(), table,
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
    public PreparedOperation<Delete> getMappedObject(DeleteSpec deleteSpec) {
        return getMappedObject(deleteSpec, null);
    }

    private PreparedOperation<Delete> getMappedObject(DeleteSpec deleteSpec,
                                                      @Nullable RelationalPersistentEntity<?> entity) {

        BindMarkers bindMarkers = this.dialect.getBindMarkersFactory().create();
        Table table = Table.create(deleteSpec.getTable());

        DeleteBuilder.DeleteWhere deleteBuilder = StatementBuilder.delete(table);

        Bindings bindings = Bindings.empty();

        Delete delete;
        if (deleteSpec.getCriteria() != null) {

            BoundCondition boundCondition = this.extendedMapper.getMappedObject(bindMarkers, deleteSpec.getCriteria(), table,
                    entity);

            bindings = boundCondition.getBindings();
            delete = deleteBuilder.where(boundCondition.getCondition()).build();
        } else {
            delete = deleteBuilder.build();
        }

        return new ExtendedStatementMapper.ExtendedPreparedOperation<>(delete, this.renderContext, bindings);
    }

    public SelectSpecWithJoin createSelectWithJoin(String table) {
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

        public ExtendedPreparedOperation(T source, RenderContext renderContext, Bindings bindings) {
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

            SqlRenderer sqlRenderer = SqlRenderer.create(this.renderContext);

            if (this.source instanceof Select) {
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
        public void bindTo(BindTarget to) {
            this.bindings.apply(to);
        }
    }

    private class Typed<T> extends ExtendedStatementMapper implements TypedStatementMapper<T> {

        final RelationalPersistentEntity<T> entity;

        public Typed(R2dbcDialect dialect, RenderContext renderContext, ExtendedMapper extendedMapper, MappingContext<RelationalPersistentEntity<?>, ? extends RelationalPersistentProperty> mappingContext, RelationalPersistentEntity<T> entity) {
            super(dialect, renderContext, extendedMapper, mappingContext);
            this.entity = entity;
        }

        @Override
        public <TC> Typed<TC> forType(Class<TC> type) {
            return ExtendedStatementMapper.this.forType(type);
        }

        @Override
        public PreparedOperation<Select> getMappedObject(SelectSpec selectSpec) {
            return ExtendedStatementMapper.this.getMappedObject(selectSpec, entity);
        }

        public PreparedOperation<Select> getMappedObject(SelectSpecWithJoin selectSpecWithJoin) {
            return ExtendedStatementMapper.this.getMappedObject(selectSpecWithJoin, entity);
        }

        @Override
        public PreparedOperation<Insert> getMappedObject(InsertSpec insertSpec) {
            return ExtendedStatementMapper.this.getMappedObject(insertSpec, entity);
        }

        @Override
        public PreparedOperation<Update> getMappedObject(UpdateSpec updateSpec) {
            return ExtendedStatementMapper.this.getMappedObject(updateSpec, entity);
        }

        @Override
        public PreparedOperation<Delete> getMappedObject(DeleteSpec deleteSpec) {
            return ExtendedStatementMapper.this.getMappedObject(deleteSpec, entity);
        }
    }

}
