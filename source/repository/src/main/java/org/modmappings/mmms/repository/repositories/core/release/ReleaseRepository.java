package org.modmappings.mmms.repository.repositories.core.release;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.r2dbc.core.StatementMapper;
import org.springframework.data.r2dbc.query.Criteria;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.lang.NonNull;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;

/**
 * Represents a repository which can provide and store {@link ReleaseDMO} objects.
 */
@Repository
public class ReleaseRepository extends ModMappingR2DBCRepository<ReleaseDMO> {

    public ReleaseRepository(RelationalEntityInformation<ReleaseDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    public Mono<ReleaseDMO> findById(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        Assert.notNull(id, "Id must not be null!");

        List<String> columns = getAccessStrategy().getAllColumns(this.getEntity().getJavaType());
        String idColumnName = getIdColumnName();

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        SelectSpecWithJoin specWithJoin = mapper.createSelectWithJoin(this.getEntity().getTableName())
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

                    if (externallyVisibleOnly)
                    {
                        criteria = nonNullAndEqualsCheckForWhere(
                                criteria,
                                true,
                                "mt",
                                "visible"
                        );
                    }

                    return criteria;
                });


        PreparedOperation<?> operation = mapper.getMappedObject(specWithJoin);

        return this.getDatabaseClient().execute(operation) //
                .as(this.getEntity().getJavaType()) //
                .fetch() //
                .one();
    }

    /**
     * Finds all releases which match the search criteria if they are supplied.
     * Supply null to anyone of them to ignore the search.
     *
     * @param nameRegex     The regex to filter the name on-
     * @param gameVersionId The id of the game version to filter releases on.
     * @param mappingTypeId The id of the mapping type to filter releases on.
     * @param isSnapshot    Indicate if snapshots should be included or not.
     * @param mappingId     The id of the mapping to filter releases on.
     * @param userId        The id of the creating user to filter releases on.
     * @param externallyVisibleOnly Indicates if only externally visible releases (releases for mapping types which are externally visible) should be returned.
     * @param pageable      The paging information for the query.
     * @return All releases which match the given criteria.
     */
    public Mono<Page<ReleaseDMO>> findAllBy(
            final String nameRegex,
            final UUID gameVersionId,
            final UUID mappingTypeId,
            final Boolean isSnapshot,
            final UUID mappingId,
            final UUID userId,
            @NonNull final boolean externallyVisibleOnly,
            @NonNull final Pageable pageable) {
        return createPagedStarRequest(
                selectSpec -> selectSpec.withJoin(
                        join("release_component", "rc")
                                .on(() -> on(reference("id")).is(reference("rc", "release_id"))),
                        join("mapping_type", "mt")
                                .on(() -> on(reference("mapping_type_id")).is(reference("mt", "id")))
                )
                        .where(() -> {
                            ColumnBasedCriteria criteria = nonNullAndMatchesCheckForWhere(
                                    null,
                                    nameRegex,
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
