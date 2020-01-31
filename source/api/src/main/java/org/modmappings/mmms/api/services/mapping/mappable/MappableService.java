package org.modmappings.mmms.api.services.mapping.mappable;

import org.modmappings.mmms.api.model.core.mapping.mappable.MappableDTO;
import org.modmappings.mmms.api.model.core.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.InsertionFailureDueToDuplicationException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableDMO;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
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
import java.util.function.Supplier;

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
    private final UserLoggingService userLoggingService;

    public MappableService(MappableRepository repository, UserLoggingService userLoggingService) {
        this.repository = repository;
        this.userLoggingService = userLoggingService;
    }

    /**
     * Looks up a mappable with a given id.
     *
     * @param id The id to look the mappable up for.
     * @return A {@link Mono} containing the requested mappable or a errored {@link Mono} that indicates a failure.
     */
    public Mono<MappableDTO> getBy(UUID id) {
        return repository.findById(id)
                .doFirst(() -> logger.debug("Looking up a mappable by id: {}", id))
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found mappable: {} with id: {}", dto.getType(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Mappable")));
    }

    /**
     * Looks up multiple mappables.
     *
     * @param type The type of mappables to lookup.
     * @param pageable The pagination and sorting info for the request.
     * @return A {@link Flux} with the mappables, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Page<MappableDTO>> getAll(
            final MappableTypeDTO type,
            final Pageable pageable
            ) {
        return repository.findAllBy(
                toTypeDMO(type),
                pageable
        )
                .doFirst(() -> logger.debug("Looking up mappables."))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this::toDTO)
                        .collectList()
                        .map(mappables -> (Page<MappableDTO>) new PageImpl<>(mappables, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found mappables: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("Mappable")));
    }

    /**
     * Deletes a given mappable if it exists.
     *
     * @param id The id of the mappable that should be deleted.
     * @param userIdSupplier The provider that gives access to the user id of the currently interacting user or service.
     * @return A {@link Mono} indicating success or failure.
     */
    public Mono<Void> deleteBy(
            final UUID id,
            final Supplier<UUID> userIdSupplier
    ) {
        return repository.deleteById(id)
                .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleting mappable with id: %s", id)))
                .doOnNext(aVoid -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleted mappable with id: %s", id)));
    }

    /**
     * Creates a new mappable from a type DTO and saves it in the repository.
     *
     * @param mappableTypeDMO The type dto to create a new mappable for.
     * @param userIdSupplier The provider that gives access to the user id of the currently interacting user or service.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<MappableDTO> create(
            final MappableTypeDTO mappableTypeDMO,
            final Supplier<UUID> userIdSupplier
    ) {
        return Mono.just(mappableTypeDMO)
                .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Creating new mappable: %s", mappableTypeDMO)))
                .map(dto -> this.toNewDMO(dto, userIdSupplier))
                .flatMap(repository::save)
                .map(this::toDTO)
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Created new mappable: %s with id: %s", dmo.getType(), dmo.getId())))
                .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_game_version_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("Mappable", "Name")));
    }

    private MappableDTO toDTO(MappableDMO dmo) {
        return new MappableDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                this.toTypeDTO(dmo.getType())
        );
    }

    private MappableDMO toNewDMO(MappableTypeDTO dto, Supplier<UUID> userIdSupplier) {
        return new MappableDMO(
                userIdSupplier.get(),
                this.toTypeDMO(dto)
        );
    }

    private MappableTypeDTO toTypeDTO(MappableTypeDMO dmo) {
        if (dmo == null)
            return null;

        return MappableTypeDTO.valueOf(MappableTypeDTO.class, dmo.name());
    }

    private MappableTypeDMO toTypeDMO(MappableTypeDTO dto) {
        if (dto == null)
            return null;

        return MappableTypeDMO.valueOf(MappableTypeDMO.class, dto.name());
    }
}
