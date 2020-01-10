package com.mcms.data.core.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.MappableInGameVersionDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * This is the R2DBC based implementation of the {@link IMappableInGameVersionRepository}.
 *
 * This interface, although not directly implementing the methods from the {@link IMappableInGameVersionRepository},
 * provides all the functionality via the {@link Query}-Annotation that is put on its methods with the relevant SQL Query implemented.
 */
public interface ISpringR2DBCMappableInGameVersionRepository extends IMappableInGameVersionRepository {

    @Query("Select * from versioned_mappable vm where vm.gameVersionId = $1")
    @Override
    Flux<MappableInGameVersionDMO> findAllForGameVersion(
                    UUID gameVersionId, final Pageable pageable);

    @Query("Select * from versioned_mappable vm where vm.parentPackageId = $1")
    @Override
    Flux<MappableInGameVersionDMO> findAllWhichArePartOfPackage(
                    UUID packageMappableInGameVersionId, final Pageable pageable);

    @Query("Select * from versioned_mappable vm where vm.partOfClassId = $1")
    @Override
    Flux<MappableInGameVersionDMO> findAllWhichArePartOfClass(
                    UUID classMappableInGameVersionId, final Pageable pageable);

    @Query("Select * from versioned_mappable vm where vm.partOfMethodId = $1")
    @Override
    Flux<MappableInGameVersionDMO> findAllWhichArePartOfMethod(
                    UUID methodMappableInGameVersionId, final Pageable pageable);

    @Query("SELECT * FROM versioned_mappable vm JOIN mapping m ON vm.id = m.mappableInGameVersionId WHERE m.id = $1 TAKE 1")
    @Override
    Mono<MappableInGameVersionDMO> findAllForMapping(
                    UUID mappingId);

    @Query("SELECT * FROM versioned_mappable vm JOIN mappable_inheritance_data mid ON vm.id = m.superTypeMappableInGameVersionId WHERE mid.subTypeMappableInGameVersionId = $1")
    @Override
    Flux<MappableInGameVersionDMO> findAllSuperTypesOf(
                    UUID classMappableInGameVersionId, final Pageable pageable);

    @Query("SELECT * FROM versioned_mappable vm JOIN mappable_inheritance_data mid ON vm.id = m.subTypeMappableInGameVersionId WHERE mid.superTypeMappableInGameVersionId = $1")
    @Override
    Flux<MappableInGameVersionDMO> findAllSubTypesOf(
                    UUID classMappableInGameVersionId, final Pageable pageable);
}
