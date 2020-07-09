package org.modmappings.mmms.repository.repositories.mapping.mappables.inheritancedata;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.repository.model.mapping.mappable.InheritanceDataDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.core.DatabaseClient;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;

/**
 * Represents a repository which can provide and store {@link InheritanceDataDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class InheritanceDataRepositoryImpl extends AbstractModMappingRepository<InheritanceDataDMO> implements InheritanceDataRepository {

    public InheritanceDataRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, InheritanceDataDMO.class);
    }

    /**
     * Finds all the inheritance data in which the given versioned mappable class is
     * the super type tole.
     *
     * @param superTypeVersionedMappableId The id of the versioned mappable class for which the inheritance data in super type role will be looked up.
     * @param pageable The pageable information in the request.
     * @return All inheritance data which indicates that the given mappable in a game version is a super type.
     */
    @Override
    public Mono<Page<InheritanceDataDMO>> findAllForSuperType(
            final UUID superTypeVersionedMappableId,
            final Pageable pageable)
    {
       return createPagedStarRequest(
               selectSpecWithJoin -> selectSpecWithJoin.withCriteria(where(Expressions.reference("super_type_versioned_mappable_id")).is(Expressions.parameter(superTypeVersionedMappableId))),
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
    @Override
    public Mono<Page<InheritanceDataDMO>> findAllForSubType(
            final UUID subTypeVersionedMappableId,
            final Pageable pageable
    ) {
        return createPagedStarRequest(
                selectSpecWithJoin -> selectSpecWithJoin.withCriteria(where(Expressions.reference("sub_type_versioned_mappable_id")).is(Expressions.parameter(subTypeVersionedMappableId))),
                pageable
        );
    }
}
