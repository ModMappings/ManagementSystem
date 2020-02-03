package org.modmappings.mmms.repository.repositories.core.gameversions;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;

/**
 * Represents a repository which can provide and store {@link GameVersionDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class GameVersionRepositoryImpl extends AbstractModMappingRepository<GameVersionDMO> implements GameVersionRepository {

    public GameVersionRepositoryImpl(DatabaseClient databaseClient, ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, GameVersionDMO.class);
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
    @Override
    public Mono<Page<GameVersionDMO>> findAllBy(
            final String nameRegex,
            final Boolean preRelease,
            final Boolean snapshot,
            final Pageable pageable
    )
    {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .where(() -> {
                                    ColumnBasedCriteria criteria = nonNullAndMatchesCheckForWhere(
                                            null,
                                            nameRegex,
                                            "",
                                            "name"
                                    );
                                    criteria = nonNullAndEqualsCheckForWhere(
                                            criteria,
                                            preRelease,
                                            "",
                                            "is_pre_release"
                                    );
                                    criteria = nonNullAndEqualsCheckForWhere(
                                            criteria,
                                            snapshot,
                                            "",
                                            "is_snapshot"
                                    );
                                    return criteria;
                                }
                        ),
                pageable
        );
    }
}
