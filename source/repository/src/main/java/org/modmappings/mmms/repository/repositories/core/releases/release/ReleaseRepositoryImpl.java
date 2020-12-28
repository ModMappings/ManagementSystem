package org.modmappings.mmms.repository.repositories.core.releases.release;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.lang.NonNull;
import org.springframework.util.Assert;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.List;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.on;
import static org.modmappings.mmms.er2dbc.data.statements.expression.Expressions.reference;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;

/**
 * Represents a repository which can provide and store {@link ReleaseDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class ReleaseRepositoryImpl extends AbstractModMappingRepository<ReleaseDMO> implements ReleaseRepository {

    public ReleaseRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, ReleaseDMO.class);
    }

    @Override
    public Mono<ReleaseDMO> findBy(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        Assert.notNull(id, "Id must not be null!");

        final List<String> columns = getAccessStrategy().getAllColumns(this.getEntity().getJavaType());
        final String idColumnName = getIdColumnName();

        final ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        final SelectSpecWithJoin specWithJoin = mapper.createSelectWithJoin(this.getEntity().getTableName())
                .withProjectionFromColumnName(columns)
                .join(() -> join("mapping_type", "mt")
                        .on(() -> on(reference("mapping_type_id")).is(reference("mt", "id"))))
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

    /**
     * Finds all releases which match the search criteria if they are supplied.
     * Supply null to anyone of them to ignore the search.
     *
     * @param nameExpression             The expression to filter the name on-
     * @param gameVersionId         The id of the game version to filter releases on.
     * @param mappingTypeId         The id of the mapping type to filter releases on.
     * @param isSnapshot            Indicate if snapshots should be included or not.
     * @param mappingId             The id of the mapping to filter releases on.
     * @param userId                The id of the creating user to filter releases on.
     * @param externallyVisibleOnly Indicates if only externally visible releases (releases for mapping types which are externally visible) should be returned.
     * @param pageable              The paging information for the query.
     * @return All releases which match the given criteria.
     */
    @Override
    public Mono<Page<ReleaseDMO>> findAllBy(
            final String nameExpression,
            final UUID gameVersionId,
            final UUID mappingTypeId,
            final Boolean isSnapshot,
            final UUID mappingId,
            final UUID userId,
            @NonNull final boolean externallyVisibleOnly,
            @NonNull final Pageable pageable) {
        return createPagedStarRequest(
                selectSpec -> selectSpec
                        .distinct()
                        .withJoin(
                                join("release_component", "rc")
                                        .on(() -> on(reference("id")).is(reference("rc", "release_id"))),
                                join("mapping_type", "mt")
                                        .on(() -> on(reference("mapping_type_id")).is(reference("mt", "id")))
                        )
                        .where(() -> {
                            ColumnBasedCriteria criteria = nonNullAndLikesCheckForWhere(
                                    null,
                                    nameExpression,
                                    "",
                                    "name"
                            );
                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    gameVersionId,
                                    "",
                                    "game_version_id"
                            );
                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    mappingTypeId,
                                    "",
                                    "mapping_type_id"
                            );
                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    isSnapshot,
                                    "",
                                    "is_snapshot"
                            );
                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    mappingId,
                                    "rc",
                                    "mapping_id"
                            );
                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    userId,
                                    "",
                                    "created_by"
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
                        }),
                pageable
        );
    }
}
