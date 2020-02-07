package org.modmappings.mmms.repository.repositories.core.mappingtypes;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.util.Assert;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.List;
import java.util.UUID;

/**
 * Represents a repository which provides and stores {@link MappingTypeDMO} objects.
 */
@Primary
@Priority(Integer.MAX_VALUE)
class MappingTypeRepositoryImpl extends AbstractModMappingRepository<MappingTypeDMO> implements MappingTypeRepository {

    public MappingTypeRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        super(databaseClient, accessStrategy, MappingTypeDMO.class);
    }

    @Override
    public Mono<MappingTypeDMO> findById(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        Assert.notNull(id, "Id must not be null!");

        final List<String> columns = getAccessStrategy().getAllColumns(this.getEntity().getJavaType());
        final String idColumnName = getIdColumnName();

        final ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        final SelectSpecWithJoin specWithJoin = mapper.createSelectWithJoin(this.getEntity().getTableName())
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


        final PreparedOperation<?> operation = mapper.getMappedObject(specWithJoin);

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
    @Override
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
