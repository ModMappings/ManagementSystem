package org.modmappings.mmms.api.services.core;

import java.util.UUID;

import com.github.dozermapper.core.Mapper;
import org.modmappings.mmms.api.controller.core.GameVersionController;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.core.IGameVersionRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

/**
 * Business layer service that handles the interactions of the API with the DataLayer.
 * <p>
 * This services validates data as well as converts between the API models as well as the data models.
 *
 * This services however does not validate if a given user is authorized to execute a given action.
 * It only validates the interaction from a data perspective.
 *
 * The caller is to make sure that any interaction with this service is authorized, for example by checking
 * against a role that a user needs to have.
 */
@Component
public class GameVersionService {

    private final Logger                 logger = LoggerFactory.getLogger(GameVersionController.class);
    private final IGameVersionRepository repository;
    private final Mapper                 mapper;
    private final UserService            userService;

    public GameVersionService(final IGameVersionRepository repository, final Mapper mapper, final UserService userService) {
        this.repository = repository;
        this.mapper = mapper;
        this.userService = userService;
    }

    /**
     * Looks up a game version with a given id.
     *
     * @param id The id to look the game version up for.
     * @return A {@link Mono} containing the requested game version or a errored {@link Mono} that indicates a failure.
     */
    public Mono<GameVersionDTO> getBy(UUID id) {
        return repository.findById(id)
                               .doFirst(() -> logger.debug("Looking up a game version by id: {}", id))
                               .map(dmo -> mapper.map(dmo, GameVersionDTO.class))
                               .doOnNext(dto -> logger.debug("Found game version: {} with id: {}", dto.getName(), dto.getId()))
                               .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "GameVersion")));
    }

    /**
     * Looks up multiple game versions.
     * The returned order is newest to oldest.
     *
     * @return A {@link Flux} with the game versions, or an errored {@link Flux} that indicates a failure.
     */
    public Flux<GameVersionDTO> getAll() {
        return repository.findAll()
                               .doFirst(() -> logger.debug("Looking up game versions."))
                               .map(dmo -> mapper.map(dmo, GameVersionDTO.class))
                               .doOnNext(dto -> logger.debug("Found game version: {} with id: {}", dto.getName(), dto.getId()))
                               .switchIfEmpty(Flux.error(new NoEntriesFoundException("GameVersion")));
    }

    /**
     * Deletes a given game version if it exists.
     *
     * @param id The id of the game version that should be deleted.
     * @return A {@link Mono} indicating success or failure.
     */
    public Mono<Void> deleteBy(UUID id) {
        return repository.deleteById(id)
                        .doFirst(() -> userService.warn(logger, String.format("Deleting game version with id: %s", id)))
                        .doOnNext(aVoid -> userService.warn(logger, String.format("Deleted game version with id: %s", id)));
    }

    /**
     * Creates a new game version from a DTO and saves it in the repository.
     *
     * @param newGameVersion The dto to create a new game version from.
     * @return A {@link Mono} that indicates succes or failure.
     */
    public Mono<GameVersionDTO> create(GameVersionDTO newGameVersion) {
        return Mono.just(newGameVersion)
                        .doFirst(() -> userService.warn(logger, String.format("Creating new game version: %s", newGameVersion.getName())))
                        .map(dto -> mapper.map(dto, GameVersionDMO.class))
                        .flatMap(repository::save)
                        .map(dmo -> mapper.map(dmo, GameVersionDTO.class))
                        .doOnNext(dmo -> {
                            userService.warn(logger, String.format("Created new game version: %s with id: %s", dmo.getName(), dmo.getId()));
                        });
    }

    /**
     * Creates a new game version from a DTO and saves it in the repository.
     *
     * @param newGameVersion The dto to create a new game version from.
     * @return A {@link Mono} that indicates succes or failure.
     */
    public Mono<GameVersionDTO> update(GameVersionDTO newGameVersion) {
        return repository.findById(newGameVersion.getId())
                        .doFirst(() -> userService.warn(logger, String.format("Updating game version: %s with id: %s", newGameVersion.getName(), newGameVersion.getId())))
                        .switchIfEmpty(Mono.error(new EntryNotFoundException(newGameVersion.getId(), "GameVersion")))
                        .doOnNext(dmo -> userService.warn(logger, String.format("Updating db game version: %s with id: %s, and data: %s",dmo.getName(), dmo.getId(), newGameVersion)))
                        .doOnNext(dmo -> mapper.map(newGameVersion, dmo)) //We use doOnNext here since this maps straight into the existing dmo that we just pulled from the DB to update.
                        .doOnNext(dmo -> userService.warn(logger, String.format("Updated db game version to: %s", dmo)))
                        .flatMap(repository::save)
                        .map(dmo -> mapper.map(dmo, GameVersionDTO.class))
                        .doOnNext(dto -> userService.warn(logger, String.format("Updated game version: %s with id: %s, to data: %s", dto.getName(), dto.getId(), dto)));
    }
}
