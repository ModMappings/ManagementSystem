package org.modmappings.mmms.repository.repositories.objects;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expression;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.objects.PackageDMO;
import org.modmappings.mmms.repository.repositories.IModMappingQuerySupport;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.on;
import static org.modmappings.mmms.er2dbc.data.statements.expression.Expressions.*;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.*;
import static org.modmappings.mmms.er2dbc.data.statements.sort.SortSpec.Order.asc;
import static org.modmappings.mmms.er2dbc.data.statements.sort.SortSpec.Order.desc;
import static org.modmappings.mmms.er2dbc.data.statements.sort.SortSpec.sort;

@Primary
@Priority(Integer.MAX_VALUE)
public class PackageRepositoryImpl implements PackageRepository, IModMappingQuerySupport {

    private final Logger logger = LoggerFactory.getLogger(this.getClass());
    private final DatabaseClient databaseClient;
    private final R2dbcConverter converter;
    private final ExtendedDataAccessStrategy accessStrategy;

    public PackageRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        this.databaseClient = databaseClient;
        this.converter = accessStrategy.getConverter();
        this.accessStrategy = accessStrategy;
    }

    @Override
    public Logger getLogger() {
        return logger;
    }

    @Override
    public DatabaseClient getDatabaseClient() {
        return databaseClient;
    }

    @Override
    public R2dbcConverter getConverter() {
        return converter;
    }

    @Override
    public ExtendedDataAccessStrategy getAccessStrategy() {
        return accessStrategy;
    }

    @Override
    public Mono<Page<PackageDMO>> findAllBy(
            final Boolean latestOnly,
            final UUID gameVersion,
            final UUID releaseId,
            final UUID mappingTypeId,
            final String matchingRegex,
            final String parentPackagePath, final boolean externallyVisibleOnly,
            final Pageable pageable
    ) {

        return this.createPagedStarRequest(
                selectSpecWithJoin -> {
                    if (latestOnly != null && latestOnly) {
                        selectSpecWithJoin = selectSpecWithJoin
                            .withDistinctOn(reference("path"));
                    } else {
                        selectSpecWithJoin = selectSpecWithJoin.withDistinct(true);
                    }

                    return selectSpecWithJoin.withJoin()
                            //LIKE concat(packages.path,'%') and m.package_parent_path like concat(packages.parent_path,'%')
                            .withJoin(leftOuterJoin("mapping", "m").withOn(on(reference("m", "package_path")).like(invoke("concat", reference("path"), just("'%'"))).and(reference("m", "package_parent_path")).like(invoke("concat", reference("parent_path"), just("'%'")))))
                            .withJoin(optionalLeftOuterJoin( externallyVisibleOnly, "mapping_type", "mt").withOn(on(reference("m", "mapping_type_id")).is(reference("mt", "id"))))
                            .withJoin(nonNullLeftOuterJoin( releaseId,"release_component", "rc").withOn(on(reference("m", "id")).is(reference("rc", "mapping_id"))))
                            .where(() -> {
                                ColumnBasedCriteria criteria = nonNullAndEqualsCheckForWhere(null, gameVersion, "m", "game_version_id");
                                criteria = nonNullAndEqualsCheckForWhere(criteria, releaseId, "rc", "release_id");
                                criteria = nonNullAndEqualsCheckForWhere(criteria, mappingTypeId, "m", "mapping_type_id");
                                criteria = nonNullAndEqualsCheckForWhere(criteria, parentPackagePath, "packages", "parent_path");

                                if (externallyVisibleOnly) {
                                    criteria = nonNullAndEqualsCheckForWhere(
                                            criteria,
                                            true,
                                            "mt",
                                            "visible"
                                    );
                                }

                                return nonNullAndMatchesCheckForWhere(criteria, matchingRegex, "packages", "path");
                            })
                            .withSort(sort(asc(reference("path"))).and(desc(reference("created_on"))));
                }
                ,
                "packages",
                PackageDMO.class,
                pageable
        );
    }

    @Override
    public Mono<Long> createCountRequest(final SelectSpecWithJoin selectSpec, final String tableName, final Class<?> resultType) {
        if (resultType != PackageDMO.class) {
            final Expression countingExpression = selectSpec.getProjectedFields().size() == 1 ? selectSpec.getProjectedFields().get(0).dealias() : Expressions.reference(tableName, getIdColumnName(resultType));
            final Expression countingDistinctExpression = selectSpec.isDistinct() ? Expressions.distinct(countingExpression) : countingExpression;
            final Expression projectionExpression = Expressions.invoke("count", countingDistinctExpression);

            return this.createCountRequestWith(selectSpec, resultType, projectionExpression);
        }

        return this.createCountRequestWith(selectSpec, resultType,Expressions.invoke("count", Expressions.distinct(Expressions.reference("path"))));
    }
}
