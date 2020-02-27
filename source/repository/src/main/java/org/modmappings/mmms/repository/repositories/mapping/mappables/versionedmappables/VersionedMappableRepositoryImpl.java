package org.modmappings.mmms.repository.repositories.mapping.mappables.versionedmappables;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
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
     * Finds all versioned mappables, which represent classes or other packages that are part of the package with the given id.
     *
     * @param packageVersionedMappableId The id of the package of which the contents are being looked up.
     * @param pageable                   The paging and sorting information.
     * @return The versioned mappables which are part of the package of which the versioned mappable has the given id.
     */
    @Override
    public Mono<Page<VersionedMappableDMO>> findAllWhichArePartOfPackage(
            final UUID packageVersionedMappableId,
            final Pageable pageable
    ) {
        return createPagedStarSingleWhereRequest("parent_package_id", packageVersionedMappableId, pageable);
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
                .withProjectionFromColumnName(columns)
                .withJoin(
                        join("mapping", "m")
                                .withOn(on(reference("id")).is(reference("m", "versioned_mappable_id")))
                )
                .withCriteria(where(reference("m", "id")).is(parameter(mappingId)))
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
                        .withJoin(
                                join("inheritance_data", "mid")
                                        .withOn(on(reference("id")).is(reference("mid", "super_type_versioned_mappable_id")))
                        )
                        .withCriteria(where(reference("mid", "sub_type_versioned_mappable_id")).is(parameter(classVersionedMappableId))),
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
                        .withJoin(
                                join("inheritance_data", "mid")
                                        .withOn(on(reference("id")).is(reference("mid", "sub_type_versioned_mappable_id")))
                        )
                        .withCriteria(where(reference("mid", "super_type_versioned_mappable_id")).is(parameter(classVersionedMappableId))),
                pageable
        );
    }

    /**
     * Finds all versioned mappables who match the given search criteria.
     *
     * @param gameVersionId The id of the game version. Null to ignore.
     * @param mappableTypeDMO The type of the mappable to look up. Null to ignore.
     * @param classId The id of the class to find versioned mappables in. Null to ignore.
     * @param methodId The id of the method to find versioned mappables in. Null to ignore.
     * @param mappingId The id of the mapping to find the versioned mappables for. Null to ignore. If parameter is passed, either a single result is returned or none. Since each mapping can only target a single versioned mappable.
     * @param superTypeTargetId The id of the class to find the super types for. Null to ignore.
     * @param subTypeTargetId The id of the class to find the sub types for. Null to ignore.
     * @param pageable The pagination and sorting information for the request.
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
            final UUID superTypeTargetId,
            final UUID subTypeTargetId,
            final Pageable pageable
    ) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .join(() -> join("mappable", "mp").on(
                                () -> on(reference("mappable_id")).is(reference("mp", "id"))
                                )
                        )
                        .join(() -> join("mapping", "m").on(
                                () -> on(reference("id")).is(reference("m", "versioned_mappable_id"))
                                )
                        )
                        .join(() -> join("inheritance_data", "super_mid").on(
                                () -> on(reference("id")).is(reference("super_mid", "super_type_versioned_mappable_id"))
                                )
                        )
                        .join(() -> join("inheritance_data", "sub_mid").on(
                                () -> on(reference("id")).is(reference("sub_mid", "sub_type_versioned_mappable_id"))
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

                            return criteria;
                        }),
                pageable
        );
    }
}
