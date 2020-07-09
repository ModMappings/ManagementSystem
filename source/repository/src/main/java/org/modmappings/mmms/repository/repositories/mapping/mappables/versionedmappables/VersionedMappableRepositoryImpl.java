package org.modmappings.mmms.repository.repositories.mapping.mappables.versionedmappables;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.VersionedMappableDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.List;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.leftOuterJoin;

/**
 * Represents a repository which can provide and store {@link VersionedMappableDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
public class VersionedMappableRepositoryImpl extends AbstractModMappingRepository<VersionedMappableDMO> implements VersionedMappableRepository {

    public VersionedMappableRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, VersionedMappableDMO.class);
    }

    @Override
    public Mono<Page<VersionedMappableDMO>> findAllForMappable(final UUID mappableId, final Pageable pageable) {
        return createPagedStarSingleWhereRequest("mappable_id", mappableId, pageable);
    }

    /**
     * Finds all versioned mappables for a given game version.
     * <p>
     * The order of the returned versioned mappables is not guaranteed.
     *
     * @param gameVersionId The id of the game version to look up all versioned mappables for.
     * @param pageable      The paging and sorting information.
     * @return The versioned mappables which are part of the given game version.
     */
    @Override
    public Mono<Page<VersionedMappableDMO>> findAllForGameVersion(
            final UUID gameVersionId,
            final Pageable pageable
    ) {
        return createPagedStarSingleWhereRequest("game_version_id", gameVersionId, pageable);
    }

    /**
     * Finds all versioned mappables, which represent methods, fields and inner classes that are part of the class with the given id.
     *
     * @param classVersionedMappableId The id of the class of which the contents are being looked up.
     * @param pageable                 The paging and sorting information.
     * @return The versioned mappables which are part of the class of which the versioned mappable has the given id.
     */
    @Override
    public Mono<Page<VersionedMappableDMO>> findAllWhichArePartOfClass(
            final UUID classVersionedMappableId,
            final Pageable pageable
    ) {
        return createPagedStarSingleWhereRequest("parent_class_id", classVersionedMappableId, pageable);
    }

    /**
     * Finds all versioned mappables, which represent parameters that are part of the method with the given id.
     *
     * @param methodVersionedMappableId The id of the method of which the contents are being looked up.
     * @param pageable                  The paging and sorting information.
     * @return The versioned mappables which are part of the method of which the versioned mappable has the given id.
     */
    @Override
    public Mono<Page<VersionedMappableDMO>> findAllWhichArePartOfMethod(
            final UUID methodVersionedMappableId,
            final Pageable pageable
    ) {
        return createPagedStarSingleWhereRequest("parent_method_id", methodVersionedMappableId, pageable);
    }

    /**
     * Finds the versioned mappable who has the given mapping.
     *
     * @param mappingId The id of the mapping to look up.
     * @return The versioned mappable who has the given mapping.
     */
    @Override
    public Mono<VersionedMappableDMO> findAllForMapping(
            final UUID mappingId
    ) {
        final ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        final SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(this.getEntity().getTableName());
        final List<String> columns = this.getAccessStrategy().getAllColumns(this.getEntity().getJavaType());

        selectSpec
                .distinct()
                .withProjectionFromColumnName(columns)
                .withJoin(
                        join("mapping", "m")
                                .withOn(on(Expressions.reference("id")).is(Expressions.reference("m", "versioned_mappable_id")))
                )
                .withCriteria(where(Expressions.reference("m", "id")).is(Expressions.parameter(mappingId)))
                .withPage(PageRequest.of(0, 1));

        final PreparedOperation<?> operation = mapper.getMappedObject(selectSpec);

        return this.getDatabaseClient().execute(operation) //
                .as(this.getEntity().getJavaType()) //
                .fetch()
                .first();
    }

    /**
     * Finds all super types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable                 The paging and sorting information.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    @Override
    public Mono<Page<VersionedMappableDMO>> findAllSuperTypesOf(
            final UUID classVersionedMappableId,
            final Pageable pageable) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .distinct()
                        .withJoin(
                                join("inheritance_data", "mid")
                                        .withOn(on(Expressions.reference("id")).is(Expressions.reference("mid", "super_type_versioned_mappable_id")))
                        )
                        .withCriteria(where(Expressions.reference("mid", "sub_type_versioned_mappable_id")).is(Expressions.parameter(classVersionedMappableId))),
                pageable
        );
    }

    /**
     * Finds all sub types (in the form of classes) of the class with the given id and returns their versioned mappables.
     *
     * @param classVersionedMappableId The id of the class of which the super types are being looked up.
     * @param pageable                 The paging and sorting information.
     * @return The versioned mappables which represent the super types of the class of which the id of its versioned mappable was provided.
     */
    @Override
    public Mono<Page<VersionedMappableDMO>> findAllSubTypesOf(
            final UUID classVersionedMappableId,
            final Pageable pageable) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .distinct()
                        .withJoin(
                                join("inheritance_data", "mid")
                                        .withOn(on(Expressions.reference("id")).is(Expressions.reference("mid", "sub_type_versioned_mappable_id")))
                        )
                        .withCriteria(where(Expressions.reference("mid", "super_type_versioned_mappable_id")).is(Expressions.parameter(classVersionedMappableId))),
                pageable
        );
    }

    /**
     * Finds all versioned mappables who match the given search criteria.
     *
     * @param gameVersionId The id of the game version. Null to ignore.
     * @param mappableTypeDMO The type of the mappable to look up. Null to ignore.
     * @param classId           The id of the class to find versioned mappables in. Null to ignore.
     * @param methodId          The id of the method to find versioned mappables in. Null to ignore.
     * @param mappingId         The id of the mapping to find the versioned mappables for. Null to ignore. If parameter is passed, either a single result is returned or none. Since each mapping can only target a single versioned mappable.
     * @param mappingTypeId         The id of the mapping type to find the versioned mappables for. Null to ignore. Use full in combination with a input and output regex.
     * @param mappingInputRegex A regex that is mapped against the input of the mapping. Null to ignore
     * @param mappingOutputRegex A regex that is mapped against the output of the mapping. Null to ignore
     * @param superTypeTargetId The id of the class to find the super types for. Null to ignore.
     * @param subTypeTargetId   The id of the class to find the sub types for. Null to ignore.
     * @param externallyVisibleOnly Indicate if externally visible classes only
     * @param pageable          The pagination and sorting information for the request.
     *
     * @return The page that returns the requested versioned mappables.
     */
    @Override
    public Mono<Page<VersionedMappableDMO>> findAllFor(
            final UUID gameVersionId,
            final MappableTypeDMO mappableTypeDMO,
            final UUID classId,
            final UUID methodId,
            final UUID mappingId,
            final UUID mappingTypeId,
            final String mappingInputRegex,
            final String mappingOutputRegex,
            final UUID superTypeTargetId,
            final UUID subTypeTargetId,
            final boolean externallyVisibleOnly,
            final Pageable pageable
    ) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .join(() -> join("mappable", "mp").on(
                                () -> on(Expressions.reference("mappable_id")).is(Expressions.reference("mp", "id"))
                                )
                        )
                        .join(() -> leftOuterJoin("mapping", "m").on(
                                () -> on(Expressions.reference("id")).is(Expressions.reference("m", "versioned_mappable_id"))
                                )
                        )
                        .join(() -> leftOuterJoin("mapping_type", "mt").on(
                                () -> on(Expressions.reference("m", "mapping_type_id")).is(Expressions.reference("mt", "id"))
                        ))
                        .join(() -> leftOuterJoin("inheritance_data", "super_mid").on(
                                () -> on(Expressions.reference("id")).is(Expressions.reference("super_mid", "super_type_versioned_mappable_id"))
                                )
                        )
                        .join(() -> leftOuterJoin("inheritance_data", "sub_mid").on(
                                () -> on(Expressions.reference("id")).is(Expressions.reference("sub_mid", "sub_type_versioned_mappable_id"))
                                )
                        )
                        .where(() -> {
                            ColumnBasedCriteria criteria = nonNullAndEqualsCheckForWhere(null, gameVersionId, "", "game_version_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, mappableTypeDMO, "mp", "type");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, classId, "", "parent_class_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, methodId, "", "parent_method_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, mappingId, "m", "id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, superTypeTargetId, "super_mid", "sub_type_versioned_mappable_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, subTypeTargetId, "sub_mid", "super_type_versioned_mappable_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, mappingTypeId, "m", "mapping_type_id");
                            criteria = nonNullAndMatchesCheckForWhere(criteria, mappingInputRegex, "m", "input");
                            criteria = nonNullAndMatchesCheckForWhere(criteria, mappingOutputRegex, "m", "output");
                            if  (externallyVisibleOnly)
                            {
                                criteria = nonNullAndEqualsCheckForWhere(criteria, true, "mt", "visible");
                            }

                            return criteria;
                        }),
                pageable
        );
    }
}
