package org.modmappings.mmms.repository.repositories.core;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;
import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.parameter;

/**
 * Represents a repository which can provide and store {@link GameVersionDMO} objects.
 */
@Repository
public class GameVersionRepository extends ModMappingR2DBCRepository<GameVersionDMO> {

    public GameVersionRepository(RelationalEntityInformation<GameVersionDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all game versions which match the given search criteria.
     *
     * @param nameRegex The regular expression used to lookup game versions for.
     * @param preRelease Indicates if prerelease are supposed to be filtered out or included, null indicates do not care.
     * @param snapshot Indicates if snapshots are supposed to be filtered out or included, null indicates do not care.
     * @param pageable The paging information for the request.s
     * @return The game versions which match the given search criteria.
     */
    Mono<Page<GameVersionDMO>> findAllBy(
            final String nameRegex,
            final Boolean preRelease,
            final Boolean snapshot,
            final Pageable pageable
    )
    {
        return createPagedAllByRequest(
                nameRegex,
                preRelease,
                snapshot,
                pageable
        );
    }

    private Mono<Page<GameVersionDMO>> createPagedAllByRequest(
            final String nameRegex,
            final Boolean preRelease,
            final Boolean snapshot,
            final Pageable pageable
            ) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .withCriteria(
                        where(
                                parameter(nameRegex)).isNull().or(reference( "name")).matches(parameter(nameRegex))
                                .and(parameter(preRelease)).isNull().or(reference( "is_pre_release")).is(parameter(preRelease))
                                .and(parameter(snapshot)).isNull().or(reference( "is_snapshot")).is(parameter(snapshot))
                ),
                pageable
        );
    }

}
