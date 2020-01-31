package org.modmappings.mmms.api.services.core;

import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.model.core.release.ReleaseDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.InsertionFailureDueToDuplicationException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.core.GameVersionRepository;
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
public class GameVersionService {

    private final Logger logger = LoggerFactory.getLogger(GameVersionService.class);
    private final GameVersionRepository repository;
    private final UserLoggingService userLoggingService;

    public GameVersionService(GameVersionRepository repository, UserLoggingService userLoggingService) {
        this.repository = repository;
        this.userLoggingService = userLoggingService;
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
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found game version: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "GameVersion")));
    }

    /**
     * Looks up multiple game versions, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameRegex The regular expression against which the name of the game version is matched.
     * @param preRelease Indicates if preReleases need to be included, null indicates do not care.
     * @param snapshot Indicates if snapshots need to be included, null indicates do not care.
     * @param pageable The pagination and sorting logic for the request.
     * @return A {@link Flux} with the game versions, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Page<GameVersionDTO>> getAll(String nameRegex, Boolean preRelease, Boolean snapshot, Pageable pageable) {
        return repository.findAllBy(
                nameRegex,
                preRelease,
                snapshot,
                pageable)
                .doFirst(() -> logger.debug("Looking up game versions in search mode. Using parameters: {}, {}, {}", nameRegex, preRelease, snapshot))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this::toDTO)
                        .collectList()
                        .map(gameVersions -> (Page<GameVersionDTO>) new PageImpl<>(gameVersions, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found game version: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("GameVersion")));
    }

    /**
     * Deletes a given game version if it exists.
     *
     * @param id The id of the game version that should be deleted.
     * @param userIdSupplier The provider that gives access to the user id of the currently interacting user or service.
     * @return A {@link Mono} indicating success or failure.
     */
    public Mono<Void> deleteBy(
            final UUID id,
            final Supplier<UUID> userIdSupplier
    ) {
        return repository.deleteById(id)
                .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleting game version with id: %s", id)))
                .doOnNext(aVoid -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleted game version with id: %s", id)));
    }

    /**
     * Creates a new game version from a DTO and saves it in the repository.
     *
     * @param newGameVersion The dto to create a new game version from.
     * @param userIdSupplier The provider that gives access to the user id of the currently interacting user or service.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<GameVersionDTO> create(
            final GameVersionDTO newGameVersion,
            final Supplier<UUID> userIdSupplier
    ) {
        return Mono.just(newGameVersion)
                .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Creating new game version: %s", newGameVersion.getName())))
                .map(dto -> this.toNewDMO(dto, userIdSupplier))
                .flatMap(repository::save)
                .map(this::toDTO)
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Created new game version: %s with id: %s", dmo.getName(), dmo.getId())))
                .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_game_version_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("GameVersion", "Name")));
    }

    /**
     * Updates an existing game version with the data in the dto and saves it in the repo.
     *
     * @param idToUpdate The id of the dmo to update with the data from the dto.
     * @param newGameVersion The dto to update the data in the dmo with.
     * @param userIdSupplier The provider that gives access to the user id of the currently interacting user or service.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<GameVersionDTO> update(
            final UUID idToUpdate,
            final GameVersionDTO newGameVersion,
            final Supplier<UUID> userIdSupplier
    ) {
        return repository.findById(idToUpdate)
                .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Updating game version: %s", idToUpdate)))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(newGameVersion.getId(), "GameVersion")))
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Updating db game version: %s with id: %s, and data: %s", dmo.getName(), dmo.getId(), newGameVersion)))
                .doOnNext(dmo -> this.updateDMO(newGameVersion, dmo)) //We use doOnNext here since this maps straight into the existing dmo that we just pulled from the DB to update.
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Updated db game version to: %s", dmo)))
                .flatMap(dmo -> repository.save(dmo)
                        .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_game_version_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("GameVersion", "Name"))))
                .map(this::toDTO)
                .doOnNext(dto -> userLoggingService.warn(logger, userIdSupplier, String.format("Updated game version: %s with id: %s, to data: %s", dto.getName(), dto.getId(), dto)));
    }

    private GameVersionDTO toDTO(GameVersionDMO dmo) {
        return new GameVersionDTO(
                dmo.getId(),
                dmo.getCreatedBy(),
                dmo.getCreatedOn(),
                dmo.getName(),
                dmo.isPreRelease(),
                dmo.isSnapshot()
        );
    }

    private GameVersionDMO toNewDMO(GameVersionDTO dto, Supplier<UUID> userIdSupplier) {
        return new GameVersionDMO(
                userIdSupplier.get(),
                dto.getName(),
                dto.getPreRelease(),
                dto.getSnapshot()
        );
    }

    private void updateDMO(GameVersionDTO dto, GameVersionDMO dmo) {
        dmo.setName(dto.getName());
        dmo.setPreRelease(dto.getPreRelease());
        dmo.setSnapshot(dto.getSnapshot());
    }
}
