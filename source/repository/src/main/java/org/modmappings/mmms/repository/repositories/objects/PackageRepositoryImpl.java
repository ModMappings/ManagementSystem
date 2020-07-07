package org.modmappings.mmms.repository.repositories.objects;

import org.modmappings.mmms.er2dbc.data.access.strategy.ExtendedDataAccessStrategy;
import org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria;
import org.modmappings.mmms.er2dbc.data.statements.join.JoinSpec;
import org.modmappings.mmms.er2dbc.data.statements.select.SelectSpecWithJoin;
import org.modmappings.mmms.repository.repositories.IModMappingQuerySupport;
import org.springframework.context.annotation.Primary;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.r2dbc.convert.R2dbcConverter;
import org.springframework.data.r2dbc.core.DatabaseClient;
import org.springframework.data.relational.core.sql.Expressions;
import reactor.core.publisher.Mono;

import javax.annotation.Priority;
import java.util.UUID;

import static org.modmappings.mmms.er2dbc.data.statements.criteria.ColumnBasedCriteria.*;
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
    public Mono<Page<String>> findAllBy(final UUID gameVersion,
                                        final UUID releaseId,
                                        final UUID mappingTypeId,
                                        String packagePrefix,
                                        Integer minAdditionalPackageDepth,
                                        Integer maxAdditionalPackageDepth,
                                        final Pageable pageable) {
        //Assure that if no prefix is requested we do not get an NPE
        if (packagePrefix == null)
            packagePrefix = "";

        if (minAdditionalPackageDepth == null)
            minAdditionalPackageDepth = 0;

        if (maxAdditionalPackageDepth == null)
            maxAdditionalPackageDepth = Integer.MAX_VALUE;

        final String subStringTrimSelection = String.format("trim(both '/' from substring(m.output, '(\\A%s([a-zA-Z]+/){%d,%d})'))", packagePrefix, minAdditionalPackageDepth, maxAdditionalPackageDepth);

        return this.createPagedRequest(
                selectSpecWithJoin -> selectSpecWithJoin
                        .withProjection(spring(Expressions.just("DISTINCT ON (" + subStringTrimSelection + ") " + subStringTrimSelection + " as package")))
                        .withJoin(join("versioned_mappable", "vm").withOn(on(reference("versioned_mappable_id")).is(reference("vm", "id"))))
                        .withJoin(join("mappable", "mp").withOn(on(reference("vm", "mappable_id")).is(reference("mp", "id"))))
                        .withJoin(join("release_component", "rc").withOn(on(reference("id")).is(reference("rc", "id"))))
                        .where(() -> {
                            ColumnBasedCriteria criteria = nonNullAndEqualsCheckForWhere(null, gameVersion, "vm", "game_version_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, releaseId, "rc", "release_id");
                            criteria = nonNullAndEqualsCheckForWhere(criteria, mappingTypeId, "mapping","mapping_type_id");
                            return criteria;
                        })
                        .withSort(Sort.by(subStringTrimSelection))
                ,
                "mapping",
                null,
                pageable
        );
    }
}
