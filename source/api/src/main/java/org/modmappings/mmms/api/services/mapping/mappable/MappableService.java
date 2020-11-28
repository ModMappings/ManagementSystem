package org.modmappings.mmms.api.services.mapping.mappable;

import org.modmappings.mmms.api.converters.mapping.mappable.MappableConverter;
import org.modmappings.mmms.api.converters.mapping.mappable.MappableTypeConverter;
import org.modmappings.mmms.api.model.mapping.mappable.MappableDTO;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.repository.repositories.mapping.mappables.mappable.MappableRepository;
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
public class MappableService {

    private final Logger logger = LoggerFactory.getLogger(MappableService.class);
    private final MappableRepository repository;
    private final MappableConverter mappableConverter;
    private final MappableTypeConverter mappableTypeConverter;

    public MappableService(final MappableRepository repository, final MappableConverter mappableConverter, final MappableTypeConverter mappableTypeConverter) {
        this.repository = repository;
        this.mappableConverter = mappableConverter;
        this.mappableTypeConverter = mappableTypeConverter;
    }

    /**
     * Looks up a mappable with a given id.
     *
     * @param id The id to look the mappable up for.
     * @return A {@link Mono} containing the requested mappable or a errored {@link Mono} that indicates a failure.
     */
    public Mono<MappableDTO> getBy(final UUID id) {
        return repository.findById(id)
                .doFirst(() -> logger.debug("Looking up a mappable by id: {}", id))
                .map(this.mappableConverter::toDTO)
                .doOnNext(dto -> logger.debug("Found mappable: {} with id: {}", dto.getType(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Mappable")));
    }

    /**
     * Looks up multiple mappables.
     *
     * @param type     The type of mappables to lookup.
     * @param pageable The pagination and sorting info for the request.
     * @return A {@link Flux} with the mappables, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Page<MappableDTO>> getAll(
            final MappableTypeDTO type,
            final Pageable pageable
    ) {
        return repository.findAllBy(
                this.mappableTypeConverter.toDMO(type),
                pageable
        )
                .doFirst(() -> logger.debug("Looking up mappables."))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this.mappableConverter::toDTO)
                        .collectList()
                        .map(mappables -> (Page<MappableDTO>) new PageImpl<>(mappables, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found mappables: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("Mappable")));
    }
}
