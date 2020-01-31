package org.modmappings.mmms.repository.repositories.mapping.mappings.mapping;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.mapper.ExtendedStatementMapper;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappings.MappingDMO;
import org.modmappings.mmms.repository.repositories.AbstractModMappingRepository;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.r2dbc.core.PreparedOperation;
import org.springframework.data.relational.repository.query.RelationalEntityInformation;
import org.springframework.stereotype.Repository;
import reactor.core.publisher.Mono;

import java.util.List;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.leftOuterJoin;

/**
 * Represents a repository which can provide and store {@link MappingDMO} objects.
 */
@Primary
class MappingRepositoryImpl extends AbstractModMappingRepository<MappingDMO> implements MappingRepository {

    public MappingRepositoryImpl(RelationalEntityInformation<MappingDMO, UUID> entity, DatabaseClient databaseClient, R2dbcConverter converter, ExtendedDataAccessStrategy accessStrategy) {
        super(entity, databaseClient, converter, accessStrategy);
    }

    /**
     * Finds all mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the mappings for.
     * @param pageable The paging and sorting information.
     * @return The mappings for the given versioned mappable.
     */
    @Override
    public Mono<Page<MappingDMO>> findAllForVersionedMappable(UUID versionedMappableId, final Pageable pageable)
    {
        return createPagedStarSingleWhereRequest(
                "versioned_mappable_id",
                versionedMappableId,
                pageable
        );
    }

    /**
     * Finds the latest mappings for a given versioned mappable id.
     *
     * @param versionedMappableId The id of the versioned mappable to get the latest mapping for.
     * @return The latest mapping for the given versioned mappable.
     */
    @Override
    public Mono<MappingDMO> findLatestForVersionedMappable(UUID versionedMappableId)
    {
        ExtendedStatementMapper mapper = getAccessStrategy().getStatementMapper().forType(this.getEntity().getJavaType());
        SelectSpecWithJoin selectSpec = mapper.createSelectWithJoin(this.getEntity().getTableName());
        List<String> columns = this.getAccessStrategy().getAllColumns(this.getEntity().getJavaType());

        selectSpec
                .withProjectionFromColumnName(columns)
                .withCriteria(where(reference( "versioned_mappable_id")).is(parameter(versionedMappableId)))
                .withPage(PageRequest.of(0, 1, Sort.by(Sort.Order.desc("created_on"))));

        PreparedOperation<?> operation = mapper.getMappedObject(selectSpec);

        return this.getDatabaseClient().execute(operation) //
                .as(this.getEntity().getJavaType()) //
                .fetch()
                .first();
    }

    /**
     * Finds all mappings of which the input matches the given regex.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param pageable The paging and sorting information.
     * @return All mappings who' matches the given regexes and are part of the mapping type and game version if those are specified.
     */
    @Override
    public Mono<Page<MappingDMO>> findAllForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(String inputRegex, String outputRegex, UUID mappingTypeId, UUID gameVersionId, Pageable pageable)
    {
        return createPagedStarRequest(selectSpecWithJoin -> {
            selectSpecWithJoin
                    .join(() -> join("versioned_mappable", "vm").on(() -> on(reference("versioned_mappable_id")).is(reference("vm", "id"))))
                    .where(() -> {
                        ColumnBasedCriteria criteria = nonNullAndMatchesCheckForWhere(
                                null,
                                inputRegex,
                                "",
                                "input"
                        );
                        criteria = nonNullAndMatchesCheckForWhere(
                                criteria,
                                outputRegex,
                                "",
                                "output"
                        );
                        criteria = nonNullAndEqualsCheckForWhere(
                                criteria,
                                mappingTypeId,
                                "",
                                "mapping_type_id"
                        );
                        criteria = nonNullAndEqualsCheckForWhere(
                                criteria,
                                gameVersionId,
                                "vm",
                                "game_version_id"
                        );
                        return criteria;
                    });
            },
            pageable
        );
    }

    /**
     * Finds all mappings of which the input matches the given regex.
     * Will only return the latest mapping for any given versioned mappable.
     * The mapping also has to be for a mapping type with the given id.
     * The mapping also has to be for a versioned mappable who targets the game version with
     * the given id.
     *
     * @param inputRegex The regex against which the input of the mappings is matched to be included in the result.
     * @param outputRegex The regex against which the output of the mappings is matched to be included in the result.
     * @param mappingTypeId The id of the mapping type that a mapping needs to be for. Use an empty optional for any mapping type.
     * @param gameVersionId The id of the game version that the mapping needs to be for. Use an empty optional for any game version.
     * @param pageable The paging and sorting information.
     * @return All latest mappings who' matches the given regexes and are part of the mapping type and game version if those are specified.
     */
    @Override
    public Mono<Page<MappingDMO>> findLatestForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(String inputRegex, String outputRegex, UUID mappingTypeId, UUID gameVersionId, Pageable pageable)
    {
        return createPagedStarRequest(selectSpecWithJoin -> {
                    selectSpecWithJoin
                            .join(() -> leftOuterJoin("mapping", "m2").on(() ->
                                            on(reference("versioned_mappable_id")).is(reference("m2", "versioned_mappable_id"))
                                                .and(reference("mapping_type_id")).is(reference("m2", "versioned_mappable_id"))
                                                .and(reference("created_on")).lessThan(reference("m2", "versioned_mappable_id")))
                            )
                            .join(() -> join("versioned_mappable", "vm").on(() -> on(reference("versioned_mappable_id")).is(reference("vm", "id"))))
                            .where(() -> {
                                ColumnBasedCriteria criteria = where(reference("m2", "id")).isNull();
                                criteria = nonNullAndMatchesCheckForWhere(
                                        criteria,
                                        inputRegex,
                                        "",
                                        "input"
                                );
                                criteria = nonNullAndMatchesCheckForWhere(
                                        criteria,
                                        outputRegex,
                                        "",
                                        "output"
                                );
                                criteria = nonNullAndEqualsCheckForWhere(
                                        criteria,
                                        mappingTypeId,
                                        "",
                                        "mapping_type_id"
                                );
                                criteria = nonNullAndEqualsCheckForWhere(
                                        criteria,
                                        gameVersionId,
                                        "vm",
                                        "game_version_id"
                                );
                                return criteria;
                            });
                },
                pageable
        );
    }

    /**
     * Finds all mappings which are part of a given release and who are for a given type.
     *
     * @param releaseId The id of the release where mappings are being looked up for.
     * @param type The type of the mappable where ids are being looked up for. Use a empty optional to get all.
     * @param pageable The paging and sorting information.
     * @return All mappings which are part of a given release and are for a given mappable type.
     */
    @Override
    public Mono<Page<MappingDMO>> findAllInReleaseAndMappableType(UUID releaseId, MappableTypeDMO type, Pageable pageable)
    {
        return createPagedStarRequest(selectSpecWithJoin -> {
                selectSpecWithJoin
                    .join(() -> join("release_component", "rc")
                                    .on(() -> on(reference("id")).is(reference("rc", "mappable_Id"))))
                    .join(() -> join("versioned_mappable", "vm")
                                    .on(() -> on(reference("versioned_mappable_id")).is(reference("vm", "id"))))
                    .join(() -> join("mappable", "mp")
                                    .on(() -> on(reference("vm", "mappable_id")).is(reference("mp", "id"))))
                    .where(() -> {
                        ColumnBasedCriteria criteria = where(reference("rc", "release_id")).is(parameter(releaseId));
                        criteria = nonNullAndEqualsCheckForWhere(
                                criteria,
                                type,
                                "mp",
                                "type"
                        );
                        return criteria;
                    });
            },
            pageable
        );
    }
}
