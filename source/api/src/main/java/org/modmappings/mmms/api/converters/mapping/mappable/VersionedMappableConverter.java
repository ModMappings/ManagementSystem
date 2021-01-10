package org.modmappings.mmms.api.converters.mapping.mappable;

import org.modmappings.mmms.api.model.mapping.mappable.SimpleVersionedMappableDTO;
import org.modmappings.mmms.api.model.mapping.mappable.VersionedMappableDTO;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.*;
import org.modmappings.mmms.repository.repositories.core.mappingtypes.MappingTypeRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.inheritancedata.InheritanceDataRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.mappable.MappableRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappables.protectedmappableinformation.ProtectedMappableInformationRepository;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Component;
import org.webjars.NotFoundException;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;
import java.util.function.Function;
import java.util.function.Supplier;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of versioned mappables.
 */
@Component
public class VersionedMappableConverter {

    private final VisibilityConverter visibilityConverter;

    private final InheritanceDataRepository inheritanceDataRepository;
    private final MappableRepository mappableRepository;
    private final ProtectedMappableInformationRepository protectedMappableInformationRepository;
    private final MappingTypeRepository mappingTypeRepository;

    public VersionedMappableConverter(final VisibilityConverter visibilityConverter, final InheritanceDataRepository inheritanceDataRepository, final MappableRepository mappableRepository, final ProtectedMappableInformationRepository protectedMappableInformationRepository, final MappingTypeRepository mappingTypeRepository) {
        this.visibilityConverter = visibilityConverter;
        this.inheritanceDataRepository = inheritanceDataRepository;
        this.mappableRepository = mappableRepository;
        this.protectedMappableInformationRepository = protectedMappableInformationRepository;
        this.mappingTypeRepository = mappingTypeRepository;
    }

    public Mono<VersionedMappableDTO> toDTO(
            final VersionedMappableDMO dmo
    ) {
        return this.toDTO(
                dmo,
                mappableRepository.findById(dmo.getMappableId()),
                inheritanceDataRepository.findAllForSuperType(dmo.getId(), Pageable.unpaged())
                        .flatMapIterable(Function.identity())
                        .map(InheritanceDataDMO::getSubTypeVersionedMappableId),
                inheritanceDataRepository.findAllForSubType(dmo.getId(), Pageable.unpaged())
                        .flatMapIterable(Function.identity())
                        .map(InheritanceDataDMO::getSuperTypeVersionedMappableId),
                protectedMappableInformationRepository.findAllByVersionedMappable(dmo.getId(), Pageable.unpaged())
                        .flatMapIterable(Function.identity())
                        .map(ProtectedMappableInformationDMO::getMappingTypeId)
                        .filterWhen(mappingTypeId -> mappingTypeRepository.findById(mappingTypeId)
                                .filter(MappingTypeDMO::isVisible)
                                .hasElement()
                        )
        );
    }

    public Mono<VersionedMappableDTO> toDTO(
            final VersionedMappableDMO dmo,
            final Mono<MappableDMO> mappable,
            final Flux<UUID> superTypeInheritance,
            final Flux<UUID> subTypeInheritance,
            final Flux<UUID> lockedMappingTypes
    ) {
        return Mono.zip(
                mappable,
                superTypeInheritance.collectList(),
                subTypeInheritance.collectList(),
                lockedMappingTypes.collectList()
        ).map((t) -> new VersionedMappableDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                dmo.getGameVersionId(),
                dmo.getMappableId(),
                dmo.getParentClassId(),
                dmo.getParentMethodId(),
                this.visibilityConverter.toDTO(dmo.getVisibility()),
                dmo.isStatic(),
                dmo.getType(),
                dmo.getDescriptor(),
                dmo.getSignature(),
                dmo.isExternal(),
                dmo.getIndex(),
                t.getT4(),
                t.getT1().getType() == MappableTypeDMO.CLASS ? t.getT2() : null,
                t.getT1().getType() == MappableTypeDMO.CLASS ? t.getT3() : null,
                t.getT1().getType() == MappableTypeDMO.METHOD ? t.getT2() : null,
                t.getT1().getType() == MappableTypeDMO.METHOD ? t.getT3() : null)
        );
    }

    public Mono<VersionedMappableDMO> toNewDMO(final VersionedMappableDTO dto,
                                         final Supplier<UUID> userIdSupplier) {
        return mappableRepository.findById(dto.getMappableId())
                .switchIfEmpty(Mono.error(() -> new NotFoundException("Mappable with the given id does not exist.")))
                .map(mappableDMO -> new VersionedMappableDMO(
                        userIdSupplier.get(),
                        dto.getGameVersionId(),
                        dto.getMappableId(),
                        mappableDMO.getType(),
                        this.visibilityConverter.toDMO(dto.getVisibility()),
                        dto.isStatic(),
                        dto.getType(),
                        dto.getParentClassId(),
                        dto.getDescriptor(),
                        dto.getParentMethodId(),
                        dto.getSignature(),
                        dto.isExternal(),
                        dto.getIndex()
                ));
    }
}
