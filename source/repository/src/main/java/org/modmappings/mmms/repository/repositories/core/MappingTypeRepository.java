package org.modmappings.mmms.repository.repositories.core;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.ModMappingR2DBCRepository;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import org.springframework.util.Assert;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.on;
import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.reference;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;

/**
 * Represents a repository which provides and stores {@link MappingTypeDMO} objects.
 */
@Repository
public class MappingTypeRepository extends ModMappingR2DBCRepository<MappingTypeDMO> {

    public MappingTypeRepository(RelationalEntityInformation<MappingTypeDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    public Mono<MappingTypeDMO> findById(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        Assert.notNull(id, "Id must not be null!");

        List<String> columns = getAccessStrategy().getAllColumns(this.getEntity().getJavaType());
        String idColumnName = getIdColumnName();

        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        SelectSpecWithJoin specWithJoin = mapper.createSelectWithJoin(this.getEntity().getTableName())
                .withProjectionFromColumnName(columns)
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
                                "",
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
     * Finds all mapping types which match the given name regex.
     * and which are editable if that parameter is supplied.
     *
     * @param nameRegex The regular expression used to lookup mapping types for.
     * @param editable Indicates if filtering on editables is needed, and if editables should be included or not. Pass null as do not care indicator.
     * @param externallyVisibleOnly Indicator if only externally visible mapping types should be returned.
     * @param pageable The paging and sorting information.
     * @return The mapping types of which the name match the regex.
     */
    public Mono<Page<MappingTypeDMO>> findAllBy(
            final String nameRegex,
            final Boolean editable,
            final boolean externallyVisibleOnly,
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
                            if (externallyVisibleOnly) {
                                criteria = nonNullAndEqualsCheckForWhere(
                                        criteria,
                                        true,
                                        "",
                                        "visible"
                                );
                            }
                            return criteria;
                        }),
                pageable
        );
    }
}
