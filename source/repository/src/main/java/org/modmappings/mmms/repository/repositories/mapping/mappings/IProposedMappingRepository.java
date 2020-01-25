package org.modmappings.mmms.repository.repositories.mapping.mappings;

import java.util.UUID;

import org.modmappings.mmms.repository.model.mapping.mappings.ProposedMappingDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;

/**
 * Represents a repository that can store and provide {@link ProposedMappingDMO} objects.
 */
public interface IProposedMappingRepository extends ModMappingR2DBCRepository<ProposedMappingDMO> {

    /**
     * Finds all proposed mappings for a given versioned mappable id.
     *
     * The proposed mappings will be returned from newest to oldest.
     *
     * @param versionedMappableId The id of the versioned mappable to get the proposed mappings for.
     * @return The proposed mappings for the given versioned mappable.
     */
    @Query("SELECT * FROM proposed_mapping pm WHERE pm.versionedMappableId = $1 And ($2 Is null OR (($2 is false AND pm.closedBy is null AND pm.closedOn is null) OR ($2 is true AND pm.closedBy is not null and pm.closedOn is not null))) And (($3 is null) or (($3 is false and pm.mappingId is null) or ($3 is true and pm.mappingId is not null))) order by m.createdOn")
    Flux<ProposedMappingDMO> findAllForVersionedMappableAndStateAndMerged(UUID versionedMappableId, Boolean state, Boolean merged);

    @Override
    @Query("Select * from proposed_mapping pm")
    Flux<ProposedMappingDMO> findAll();
}
