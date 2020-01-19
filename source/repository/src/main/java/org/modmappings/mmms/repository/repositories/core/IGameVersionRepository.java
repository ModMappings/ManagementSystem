package org.modmappings.mmms.repository.repositories.core;

import java.util.Optional;
import java.util.UUID;

import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.IPageableR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link GameVersionDMO} objects.
 */
@Repository
public interface IGameVersionRepository extends IPageableR2DBCRepository<GameVersionDMO> {

    /**
     * Finds all game versions which match the given name regex,
     * and which are a prerelease or snapshot if those parameters are supplied.
     *
     * The game versions are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup game versions for.
     * @param preRelease Indicates if prerelease are supposed to be filtered out or included, null indicates do not care.
     * @param snapshot Indicates if snapshots are supposed to be filtered out or included, null indicates do not care.
     * @return The game versions of which the name match the regex.
     */
    @Query("SELECT gv.* FROM game_version gv WHERE gv.name ~ :nameRegex AND (:preRelease is null OR gv.is_pre_release = :preRelease) AND (:snapshot is null OR gv.is_snapshot = :snapshot) Order by gv.created_on")
    Flux<GameVersionDMO> findAllFor(@Param("nameRegex") String nameRegex, @Param("preRelease") Boolean preRelease, @Param("snapshot") Boolean snapshot);

    @Override
    @Query("Select gv.* from game_version gv Order by gv.created_on")
    Flux<GameVersionDMO> findAll();
}
