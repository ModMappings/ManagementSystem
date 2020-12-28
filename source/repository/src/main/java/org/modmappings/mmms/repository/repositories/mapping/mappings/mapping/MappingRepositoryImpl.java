package org.modmappings.mmms.repository.repositories.mapping.mappings.mapping;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.MappingDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.util.Assert;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.List;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.on;
import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.where;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.leftOuterJoin;

/**
 * Represents a repository which can provide and store {@link MappingDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class MappingRepositoryImpl extends AbstractModMappingRepository<MappingDMO> implements MappingRepository {

    public MappingRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, MappingDMO.class);
    }

    @Override
    public Mono<Page<MappingDMO>> findAllOrLatestFor(final Boolean latestOnly,
                                                     final UUID versionedMappableId,
                                                     final UUID releaseId,
                                                     final MappableTypeDMO mappableType,
                                                     final String inputRegex,
                                                     final String outputRegex,
                                                     final UUID mappingTypeId,
                                                     final UUID gameVersionId,
                                                     final UUID userId,
                                                     final UUID parentClassId,
                                                     final UUID parentMethodId,
                                                     final boolean externallyVisibleOnly,
                                                     final Pageable pageable) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .join(() -> join("release_component", "rc")
                                .on(() -> on(Expressions.reference("id")).is(Expressions.reference("rc", "mapping_id"))))
                        .join(() -> join("versioned_mappable", "vm")
                                .on(() -> on(Expressions.reference("versioned_mappable_id")).is(Expressions.reference("vm", "id"))))
                        .join(() -> join("mappable", "mp")
                                .on(() -> on(Expressions.reference("vm", "mappable_id")).is(Expressions.reference("mp", "id"))))
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
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, mappableType, "mp", "type");
                                    criteria = nonNullAndMatchesCheckForWhere(criteria, inputRegex, "", "input");
                                    criteria = nonNullAndMatchesCheckForWhere(criteria, outputRegex, "", "output");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, mappingTypeId, "", "mapping_type_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, gameVersionId, "vm", "game_version_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, parentClassId, "vm", "parent_class_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, parentMethodId, "vm", "parent_method_id");
                                    criteria = nonNullAndEqualsCheckForWhere(criteria, userId, "", "created_by");

                                    if (externallyVisibleOnly) {
                                        criteria = nonNullAndEqualsCheckForWhere(criteria, true, "mt", "visible");
                                    }

                                    return criteria;
                                }
                        )
                ,
                pageable
        );
    }

    @Override
    public Mono<MappingDMO> findById(final UUID id,
                                     final boolean externallyVisibleOnly) {
        Assert.notNull(id, "Id must not be null!");

        final List<String> columns = getAccessStrategy().getAllColumns(this.getEntity().getJavaType());
        final String idColumnName = getIdColumnName();

        final ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        final SelectSpecWithJoin specWithJoin = mapper.createSelectWithJoin(this.getEntity().getTableName())
                .withProjectionFromColumnName(columns)
                .join(() -> join("mapping_type", "mt").on(() -> on(Expressions.reference("mapping_type_id")).is(Expressions.reference("mt", "id"))))
                .where(() -> {
                    ColumnBasedCriteria criteria = nonNullAndEqualsCheckForWhere(
                            null,
                            id,
                            "",
                            idColumnName
                    );

                    if (externallyVisibleOnly) {
                        criteria = nonNullAndEqualsCheckForWhere(
                                criteria,
                                true,
                                "mt",
                                "visible"
                        );
                    }

                    return criteria;
                });


        final PreparedOperation<?> operation = mapper.getMappedObject(specWithJoin);

        return this.getDatabaseClient().execute(operation) //
                .as(this.getEntity().getJavaType()) //
                .fetch() //
                .one();
    }
}
