package org.modmappings.mmms.api.services.mapping.mappable;

import org.modmappings.mmms.api.model.mapping.mappable.VersionedMappableDTO;
import org.modmappings.mmms.api.model.mapping.mappable.VisibilityDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.*;
import org.modmappings.mmms.repository.repositories.core.mappingtypes.MappingTypeRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.inheritancedata.InheritanceDataRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.mappable.MappableRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.protectedmappableinformation.ProtectedMappableInformationRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.versionedmappables.VersionedMappableRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;
import java.util.function.Function;

/**
 * Business layer service which handles the interactions of the API with the DataLayer.
 * <p>
 * This services validates data as well as converts between the API models as well as the data models.
 * <p>
 * This services however does not validate if a given user is authorized to execute a given action.
 * It only validates the interaction from a data perspective.
 * <p>
 * The caller is to make sure that any interaction with this service is authorized, for example by checking
 * against a role that a user needs to have.
 */
@Component
public class VersionedMappableService {

    private final Logger logger = LoggerFactory.getLogger(VersionedMappableService.class);

    private final VersionedMappableRepository repository;
    private final InheritanceDataRepository inheritanceDataRepository;
    private final MappableRepository mappableRepository;
    private final ProtectedMappableInformationRepository protectedMappableInformationRepository;
    private final MappingTypeRepository mappingTypeRepository;

    public VersionedMappableService(VersionedMappableRepository repository, InheritanceDataRepository inheritanceDataRepository, MappableRepository mappableRepository, ProtectedMappableInformationRepository protectedMappableInformationRepository, MappingTypeRepository mappingTypeRepository) {
        this.repository = repository;
        this.inheritanceDataRepository = inheritanceDataRepository;
        this.mappableRepository = mappableRepository;
        this.protectedMappableInformationRepository = protectedMappableInformationRepository;
        this.mappingTypeRepository = mappingTypeRepository;
    }

    /**
     * Looks up a mappable with a given id.
     *
     * @param id The id to look the mappable up for.
     * @return A {@link Mono} containing the requested mappable or a errored {@link Mono} that indicates a failure.
     */
    public Mono<VersionedMappableDTO> getBy(UUID id) {
        return repository.findById(id)
                .doFirst(() -> logger.debug("Looking up a mappable by id: {}", id))
                .flatMap(this::toDTO)
                .doOnNext(dto -> logger.debug("Found mappable: {} with id: {}", dto.getType(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Mappable")));
    }

    /**
     * Look up all versioned mappables who match the given search criteria.
     *
     * @param gameVersionId The id of the game version. Null to ignore.
     * @param mappableTypeDMO The type of the mappable to look up. Null to ignore.
     * @param packageId The id of the package to find versioned mappables in. Null to ignore.
     * @param classId The id of the class to find versioned mappables in. Null to ignore.
     * @param methodId The id of the method to find versioned mappables in. Null to ignore.
     * @param mappingId The id of the mapping to find the versioned mappables for. Null to ignore. If parameter is passed, either a single result is returned or none. Since each mapping can only target a single versioned mappable.
     * @param superTypeTargetId The id of the class to find the super types for. Null to ignore.
     * @param subTypeTargetId The id of the class to find the sub types for. Null to ignore.
     * @param pageable The pagination and sorting information for the request.
     *
     * @return The page that returns the requested versioned mappables.
     */
    public Mono<Page<VersionedMappableDTO>> getAll(
            final UUID gameVersionId,
            final MappableTypeDMO mappableTypeDMO,
            final UUID packageId,
            final UUID classId,
            final UUID methodId,
            final UUID mappingId,
            final UUID superTypeTargetId,
            final UUID subTypeTargetId,
            final Pageable pageable
            ) {
        return repository.findAllFor(
                gameVersionId, mappableTypeDMO, packageId, classId, methodId, mappingId, superTypeTargetId, subTypeTargetId, pageable
        )
                .doFirst(() -> logger.debug("Looking up mappables."))
                .flatMap(page -> Flux.fromIterable(page)
                        .flatMap(this::toDTO)
                        .collectList()
                        .map(mappables -> (Page<VersionedMappableDTO>) new PageImpl<>(mappables, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found mappables: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("Mappable")));
    }

    private Mono<VersionedMappableDTO> toDTO(VersionedMappableDMO dmo) {
        return mappableRepository.findById(dmo.getMappableId())
                .flatMap(mappableDMO -> inheritanceDataRepository.findAllForSuperType(dmo.getId(), Pageable.unpaged())
                        .flatMapIterable(Function.identity())
                        .map(InheritanceDataDMO::getSubTypeVersionedMappableId)
                        .collectList()
                        .flatMap(superTypeIds -> inheritanceDataRepository.findAllForSubType(dmo.getId(), Pageable.unpaged())
                            .flatMapIterable(Function.identity())
                            .map(InheritanceDataDMO::getSuperTypeVersionedMappableId)
                            .collectList()
                            .flatMap(subTypeIds -> protectedMappableInformationRepository.findAllByVersionedMappable(dmo.getId(), Pageable.unpaged())
                                .flatMapIterable(Function.identity())
                                .map(ProtectedMappableInformationDMO::getMappingType)
                                .filterWhen(mappingTypeId -> mappingTypeRepository.findById(mappingTypeId)
                                    .filter(MappingTypeDMO::isVisible)
                                    .hasElement()
                                )
                                .collectList()
                                .map(lockedIds -> new VersionedMappableDTO(
                                        dmo.getId(),
                                        dmo.getCreatedBy(),
                                        dmo.getCreatedOn(),
                                        dmo.getGameVersionId(),
                                        dmo.getMappableId(),
                                        dmo.getParentPackageId(),
                                        dmo.getParentClassId(),
                                        dmo.getParentMethodId(),
                                        toTypeDTO(dmo.getVisibility()),
                                        dmo.isStatic(),
                                        dmo.getType(),
                                        dmo.getDescriptor(),
                                        lockedIds,
                                        superTypeIds,
                                        subTypeIds
                                ))
                            )
                        )

                );
    }

    private VisibilityDTO toTypeDTO(VisibilityDMO dmo) {
        if (dmo == null)
            return null;

        return VisibilityDTO.valueOf(VisibilityDTO.class, dmo.name());
    }
}
