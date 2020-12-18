package org.modmappings.mmms.repository.repositories.mapping.mappings.detailed;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.DetailedMappingDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.MappingDMO;
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
import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.where;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.leftOuterJoin;
import static org.modmappings.mmms.er2dbc.data.statements.sort.SortSpec.sort;

@Primary
@Priority(Integer.MAX_VALUE)
public class DetailedMappingRepositoryImpl implements DetailedMappingRepository, IModMappingQuerySupport {

    private final Logger logger = LoggerFactory.getLogger(this.getClass());
    private final DatabaseClient databaseClient;
    private final R2dbcConverter converter;
    private final ExtendedDataAccessStrategy accessStrategy;

    public DetailedMappingRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
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
    public Mono<Page<DetailedMappingDMO>> findAllBy(
            final Boolean latestOnly,
            final UUID versionedMappableId,
            final UUID releaseId,
            final MappableTypeDMO mappableType,
            final String inputRegex,
            final String outputRegex,
            final UUID mappingTypeId,
            final UUID gameVersionId,
            final UUID userId,
            final boolean externallyVisibleOnly,
            final Pageable pageable
    ) {
        return createPagedRequestWithCountType(
                selectSpecWithJoin -> selectSpecWithJoin
                        .select(createSelectStatementsForCompoundEntity(DetailedMappingDMO.class))
                        .join(() -> join("release_component", "rc")
                                .on(() -> on(Expressions.reference("id")).is(Expressions.reference("rc", "mapping_id"))))
                        .join(() -> join("versioned_mappable", "versioned_mappable")
                                .on(() -> on(Expressions.reference("versioned_mappable_id")).is(Expressions.reference("versioned_mappable", "id"))))
                        .join(() -> join("mappable", "mappable")
                                .on(() -> on(Expressions.reference("versioned_mappable", "mappable_id")).is(Expressions.reference("mappable", "id"))))
                        .join(() -> join("mapping_type", "mt")
                                .on(() -> on(Expressions.reference("mapping_type_id")).is(Expressions.reference("mt", "id"))))
                        .join(() -> leftOuterJoin("mapping", "m2")
                                .on(() -> on(Expressions.reference("versioned_mappable_id")).is(Expressions.reference("m2", "versioned_mappable_id"))
                                        .and(Expressions.reference("mapping_type_id")).is(Expressions.reference("m2", "mapping_type_id"))
                                        .and(Expressions.reference("created_on")).lessThan(Expressions.reference("m2", "created_on")))
                        )
                        .where(
                                () -> {
                                    ColumnBasedCriteria criteria = latestOnly ? where(Expressions.reference("m2", "id")).isNull() : null;
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, versionedMappableId, "", "versioned_mappable_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, releaseId, "rc", "release_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, mappableType, "mappable", "type");
                                    criteria = nonNullAndMatchesCheckForWhere(criteria, inputRegex, "", "input");
                                    criteria = nonNullAndMatchesCheckForWhere(criteria, outputRegex, "", "output");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, mappingTypeId, "", "mapping_type_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, gameVersionId, "versioned_mappable", "game_version_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, userId, "", "created_by");

                                    if (externallyVisibleOnly) {
                                        criteria = nonNullAndEqualsCheckForWhere(criteria, true, "mt", "visible");
                                    }

                                    return criteria;
                                }
                        )
                ,
                "mapping",
                DetailedMappingDMO.class,
                MappingDMO.class,
                pageable
        );
    }
}
