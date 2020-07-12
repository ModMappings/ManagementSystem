package org.modmappings.mmms.api.converters.mapping.mappable;

import org.modmappings.mmms.api.model.mapping.mappable.SimpleVersionedMappableDTO;
import org.modmappings.mmms.api.model.mapping.mappable.VersionedMappableDTO;
import org.modmappings.mmms.repository.model.mapping.mappable.*;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;
import java.util.function.Supplier;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of versioned mappables.
 */
@Component
public class VersionedMappableConverter {

    private final VisibilityConverter visibilityConverter;

    public VersionedMappableConverter(final VisibilityConverter visibilityConverter) {
        this.visibilityConverter = visibilityConverter;
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

    public SimpleVersionedMappableDTO toSimpleDTO(
            final VersionedMappableDMO dmo
    ) {
        return new SimpleVersionedMappableDTO(
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
                dmo.getIndex());
    }

    public VersionedMappableDMO toNewDMO(final VersionedMappableDTO dto,
                                         final Supplier<UUID> userIdSupplier) {
        return new VersionedMappableDMO(
                userIdSupplier.get(),
                dto.getGameVersionId(),
                dto.getMappableId(),
                this.visibilityConverter.toDMO(dto.getVisibility()),
                dto.isStatic(),
                dto.getType(),
                dto.getParentClassId(),
                dto.getDescriptor(),
                dto.getParentMethodId(),
                dto.getSignature(),
                dto.isExternal(),
                dto.getIndex()
        );
    }
}
