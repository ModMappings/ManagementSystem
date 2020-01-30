package org.modmappings.mmms.repository.repositories.mapping.mappable;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.repository.model.mapping.mappable.InheritanceDataDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * Represents a repository which can provide and store {@link InheritanceDataDMO} objects.
 */
@Repository
public class InheritanceDataRepository extends ModMappingR2DBCRepository<InheritanceDataDMO> {

    public InheritanceDataRepository(RelationalEntityInformation<InheritanceDataDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the super type tole.
     *
     * @param superTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in super type role will be looked up.
     * @param pageable The pageable information in the request.
     * @return All inheritance data which indicates that the given mappable in a game version is a super type.
     */
    public Mono<Page<InheritanceDataDMO>> findAllForSuperType(
            final UUID superTypeVersionedMappableId,
            final Pageable pageable)
    {
       return createPagedStarRequest(
               selectSpecWithJoin -> selectSpecWithJoin.withCriteria(where(reference("super_type_versioned_mappable_id")).is(parameter(superTypeVersionedMappableId))),
               pageable
       );
    }

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the sub type tole.
     *
     * @param subTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in sub type role will be looked up.
     * @param pageable The pageable information in the request.
     * @return All inheritance data which indicates that the given mappable in a game version is a sub type.
     */
    public Mono<Page<InheritanceDataDMO>> findAllForSubType(
            final UUID subTypeVersionedMappableId,
            final Pageable pageable
    ) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin.withCriteria(where(reference("sub_type_versioned_mappable_id")).is(parameter(subTypeVersionedMappableId))),
                pageable
        );
    }
}
