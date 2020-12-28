package org.modmappings.mmms.api.services.core;

import org.modmappings.mmms.api.converters.MappingTypeConverter;
import org.modmappings.mmms.api.model.core.MappingTypeDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.repositories.core.mappingtypes.MappingTypeRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageImpl;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;

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
public class MappingTypeService {

    private final Logger logger = LoggerFactory.getLogger(MappingTypeService.class);
    private final MappingTypeRepository repository;
    private final MappingTypeConverter mappingTypeConverter;

    public MappingTypeService(final MappingTypeRepository repository, final MappingTypeConverter mappingTypeConverter) {
        this.repository = repository;
        this.mappingTypeConverter = mappingTypeConverter;
    }

    /**
     * Looks up a mapping type with a given id.
     *
     * @param id                    The id to look the mapping type up for.
     * @param externallyVisibleOnly Indicates if only externally visible mapping types should be returned.
     * @return A {@link Mono} containing the requested mapping type or a errored {@link Mono} that indicates a failure.
     */
    public Mono<MappingTypeDTO> getBy(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        return repository.findById(id, externallyVisibleOnly)
                .doFirst(() -> logger.debug("Looking up a mapping type by id: {}", id))
                .map(this.mappingTypeConverter::toDTO)
                .doOnNext(dto -> logger.debug("Found mapping type: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "MappingType")));
    }

    /**
     * Looks up multiple mapping types, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameRegex             The regular expression against which the name of the mapping type is matched.
     * @param editable              Indicates if editable mapping types need to be included, null indicates do not care.
     * @param externallyVisibleOnly Indicator if only externally visible mapping types should be returned.
     * @param pageable              The paging and sorting information.
     * @return A {@link Flux} with the mapping types, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Page<MappingTypeDTO>> getAll(
            final String nameRegex,
            final Boolean editable,
            final boolean externallyVisibleOnly,
            final Pageable pageable
    ) {
        return repository.findAllBy(
                nameRegex,
                editable,
                externallyVisibleOnly,
                pageable)
                .doFirst(() -> logger.debug("Looking up mapping types in search mode. Using parameters: {}, {}", nameRegex, editable))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this.mappingTypeConverter::toDTO)
                        .collectList()
                        .map(mappingTypes -> (Page<MappingTypeDTO>) new PageImpl<>(mappingTypes, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found mapping types: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("MappingType")));
    }
}
