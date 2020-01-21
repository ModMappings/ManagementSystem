package org.modmappings.mmms.repository.repositories.core.release;

import java.util.UUID;

import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.IPageableR2DBCRepository;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Represents a repository which can provide and store {@link ReleaseDMO} objects.
 */
@Repository
public interface IReleaseRepository extends IPageableR2DBCRepository<ReleaseDMO> {

    /**
     * Finds all releases which match the search criteria if they are supplied.
     * Supply null to anyone of them to ignore the search.
     *
     * @param nameRegex The regex to filter the name on-
     * @param gameVersionId The id of the game version to filter releases on.
     * @param mappingTypeId The id of the mapping type to filter releases on.
     * @param isSnapshot Indicate if snapshots should be included or not.
     * @param mappingId The id of the mapping to filter releases on.
     * @param userId The id of the creating user to filter releases on.
     * @return All releases which match the given criteria.
     */
    @Query("Select r.* from release r" +
            "join release_component rc on r.id = rc.releaseId" +
            "where ((:nameRegex is null) or (r.name ~ :nameRegex)) " +
            "and ((:gameVersionId is null) or (r.game_version_id = :gameVersionId)) " +
            "and ((:mappingTypeId is null) or (r.mapping_type_id = :mappingTypeId)) " +
            "and ((:isSnapshot is null) or (r.is_snapshot = :isSnapshot)) " +
            "and ((:mappingId is null) or (rc.mapping_id = :mappingId)) " +
            "and ((:userId is null) or (r.created_by = :userId))")
    Flux<ReleaseDMO> findAllFor(
            @Param("nameRegex") String nameRegex,
            @Param("gameVersionId") UUID gameVersionId,
            @Param("mappingTypeId") UUID mappingTypeId,
            @Param("isSnapshot") Boolean isSnapshot,
            @Param("mappingId") UUID mappingId,
            @Param("userId") UUID userId);

    @Override
    @Query("Select r.* from release r")
    Flux<ReleaseDMO> findAll();
}
