package org.modmappings.mmms.repository.repositories.core;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Represents a repository which provides and stores {@link MappingTypeDMO} objects.
 */
@Repository
public class MappingTypeRepository extends ModMappingR2DBCRepository<MappingTypeDMO> {

    public MappingTypeRepository(RelationalEntityInformation<MappingTypeDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all mapping types which match the given name regex.
     * and which are editable if that parameter is supplied.
     *
     * @param nameRegex The regular expression used to lookup mapping types for.
     * @param editable Indicates if filtering on editables is needed, and if editables should be included or not. Pass null as do not care indicator.
     * @param pageable The paging and sorting information.
     * @return The mapping types of which the name match the regex.
     */
    public Mono<Page<MappingTypeDMO>> findAllBy(
            final String nameRegex,
            final Boolean editable,
            final Pageable pageable) {
        return createPagedStarRequest(
                selectSpec -> selectSpec
                        .where(() -> {
                            ColumnBasedCriteria criteria = nonNullAndMatchesCheckForWhere(
                                    null,
                                    nameRegex,
                                    "",
                                    "name"
                            );
                            criteria = nonNullAndEqualsCheckForWhere(
                                    criteria,
                                    editable,
                                    "",
                                    "editable"
                            );
                            return criteria;
                        }),
                pageable
        );
    }
}
