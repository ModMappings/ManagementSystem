package com.mcms.data.repositories.mapping.mappable;

import java.util.UUID;

import com.mcms.data.model.mapping.mappable.VersionedMappableDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.Query;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * This is the R2DBC based implementation of the {@link IVersionedMappableRepository}.
 *
 * This interface, although not directly implementing the methods from the {@link IVersionedMappableRepository},
 * provides all the functionality via the {@link Query}-Annotation that is put on its methods with the relevant SQL Query implemented.
 */
public interface VersionedMappableRepositoryImpl extends IVersionedMappableRepository {

    @Query("Select * from versioned_mappable vm where vm.gameVersionId = $1")
    @Override
    Flux<VersionedMappableDMO> findAllForGameVersion(
                    UUID gameVersionId, final Pageable pageable);

    @Query("Select * from versioned_mappable vm where vm.parentPackageId = $1")
    @Override
    Flux<VersionedMappableDMO> findAllWhichArePartOfPackage(
                    UUID packageVersionedMappableId, final Pageable pageable);

    @Query("Select * from versioned_mappable vm where vm.partOfClassId = $1")
    @Override
    Flux<VersionedMappableDMO> findAllWhichArePartOfClass(
                    UUID classVersionedMappableId, final Pageable pageable);

    @Query("Select * from versioned_mappable vm where vm.partOfMethodId = $1")
    @Override
    Flux<VersionedMappableDMO> findAllWhichArePartOfMethod(
                    UUID methodVersionedMappableId, final Pageable pageable);

    @Query("SELECT * FROM versioned_mappable vm JOIN mapping m ON vm.id = m.versionedMappableId WHERE m.id = $1 TAKE 1")
    @Override
    Mono<VersionedMappableDMO> findAllForMapping(
                    UUID mappingId);

    @Query("SELECT * FROM versioned_mappable vm JOIN mappable_inheritance_data mid ON vm.id = m.superTypeVersionedMappableId WHERE mid.subTypeVersionedMappableId = $1")
    @Override
    Flux<VersionedMappableDMO> findAllSuperTypesOf(
                    UUID classVersionedMappableId, final Pageable pageable);

    @Query("SELECT * FROM versioned_mappable vm JOIN mappable_inheritance_data mid ON vm.id = m.subTypeVersionedMappableId WHERE mid.superTypeVersionedMappableId = $1")
    @Override
    Flux<VersionedMappableDMO> findAllSubTypesOf(
                    UUID classVersionedMappableId, final Pageable pageable);
}
