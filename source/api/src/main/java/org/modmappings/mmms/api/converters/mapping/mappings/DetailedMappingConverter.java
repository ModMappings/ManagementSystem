package org.modmappings.mmms.api.converters.mapping.mappings;

import org.modmappings.mmms.api.converters.mapping.mappable.MappableConverter;
import org.modmappings.mmms.api.converters.mapping.mappable.VersionedMappableConverter;
import org.modmappings.mmms.api.model.mapping.mappable.DetailedMappingDTO;
import org.modmappings.mmms.repository.model.mapping.mappings.DetailedMappingDMO;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.List;

/**
 * Business layer converter that handles the conversion between DMO and DTO,
 * of detailed mappings.
 */
@Component
public class DetailedMappingConverter {

    private final MappingConverter mappingConverter;
    private final VersionedMappableConverter versionedMappableConverter;
    private final MappableConverter mappableConverter;

    public DetailedMappingConverter(final MappingConverter mappingConverter, final VersionedMappableConverter versionedMappableConverter, final MappableConverter mappableConverter) {
        this.mappingConverter = mappingConverter;
        this.versionedMappableConverter = versionedMappableConverter;
        this.mappableConverter = mappableConverter;
    }

    public Mono<DetailedMappingDTO> toDTO(final DetailedMappingDMO dmo) {
        return this.versionedMappableConverter.toDTO(dmo.getVersionedMappable())
                .map(vmDto -> new DetailedMappingDTO(
                        this.mappableConverter.toDTO(dmo.getMappable()),
                        vmDto,
                        this.mappingConverter.toDTO(dmo.getMapping())));
    }
}
