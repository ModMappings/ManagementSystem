package org.modmappings.mmms.repository.repositories.objects;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.expression.Expressions;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
import org.modmappings.mmms.repository.repositories.IModMappingQuerySupport;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.on;
import static org.modmappings.mmms.er2dbc.data.statements.expression.Expressions.*;
import static org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec.join;

@Primary
@Priority(Integer.MAX_VALUE)
public class PackageRepositoryImpl implements PackageRepository, IModMappingQuerySupport {

    private final DatabaseClient databaseClient;
    private final R2dbcConverter converter;
    private final ExtendedDataAccessStrategy accessStrategy;

    public PackageRepositoryImpl(final DatabaseClient databaseClient, final ExtendedDataAccessStrategy accessStrategy) {
        this.databaseClient = databaseClient;
        this.converter = accessStrategy.getConverter();
        this.accessStrategy = accessStrategy;
    }

    @Override
    public DatabaseClient getDatabaseClient() {
        return databaseClient;
    }

    @Override
    public R2dbcConverter getConverter() {
        return converter;
    }

    @Override
    public ExtendedDataAccessStrategy getAccessStrategy() {
        return accessStrategy;
    }

    @Override
    public Mono<Page<String>> findAllBy(
            final UUID gameVersion,
            final UUID releaseId,
            final UUID mappingTypeId,
            final String inputMatchingRegex,
            final String outputMatchingRegex,
            final Pageable pageable
    ) {

        if (inputMatchingRegex != null && outputMatchingRegex != null) {
            return Mono.error(new IllegalArgumentException("Both input and output matching regex are supplied. Package lookup only supports either output mode or input mode."));
        }

        if (inputMatchingRegex == null && outputMatchingRegex == null) {
            return Mono.error(new IllegalArgumentException("Neither input and output matching regex are supplied. Package lookup requires at least one of the two to be supplied."));
        }

        final String side = outputMatchingRegex != null ? "output" : "input";

        return this.createPagedRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .withProjection(distinct(aliased(invoke("substring", reference("mapping", side), Expressions.parameter(outputMatchingRegex)), "package")))
                        .withJoin(join("versioned_mappable", "vm").withOn(on(reference("versioned_mappable_id")).is(reference("vm", "id"))))
                        .withJoin(join("mappable", "mp").withOn(on(reference("vm", "mappable_id")).is(reference("mp", "id"))))
                        .withJoin(join("release_component", "rc").withOn(on(reference("id")).is(reference("rc", "mapping_id"))))
                        .where(() -> {
                            ColumnBasedCriteria criteria = nonNullAndEqualsCheckForWhere(null, gameVersion, "vm", "game_version_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, releaseId, "rc", "release_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, mappingTypeId, "mapping", "mapping_type_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, MappableTypeDMO.CLASS, "mp", "type");
                            return criteria;
                        })
                ,
                "mapping",
                String.class,
                pageable
        );
    }
}
