package org.modmappings.mmms.api.services.core;

import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.InsertionFailureDueToDuplicationException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.core.IGameVersionRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.security.Principal;
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
public class GameVersionService {

    private final Logger logger = LoggerFactory.getLogger(GameVersionService.class);
    private final IGameVersionRepository repository;
    private final UserService userService;

    public GameVersionService(final IGameVersionRepository repository, final UserService userService) {
        this.repository = repository;
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
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found game version: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "GameVersion")));
    }

    /**
     * Looks up multiple game versions.
     * The returned order is newest to oldest.
     *
     * @param page The 0-based page index used during pagination logic.
     * @param size The maximum amount of items on a given page.
     * @return A {@link Flux} with the game versions, or an errored {@link Flux} that indicates a failure.
     */
    public Flux<GameVersionDTO> getAll(int page, int size) {
        return repository.findAll()
                .doFirst(() -> logger.debug("Looking up game versions."))
                .skip(page * size)
                .limitRequest(size)
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found game version: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Flux.error(new NoEntriesFoundException("GameVersion")));
    }

    /**
     * Determines the amount of game versions that exist in the database.
     *
     * @return A {@link Mono} that indicates the amount of game versions in the database.
     */
    public Mono<Long> count() {
        return repository
                .count()
                .doFirst(() -> logger.debug("Determining the available amount of game versions"))
                .doOnNext((cnt) -> logger.debug("There are {} game versions available.", cnt));
    }

    /**
     * Looks up multiple game versions, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameRegex The regular expression against which the name of the game version is matched.
     * @param preRelease Indicates if preReleases need to be included, null indicates do not care.
     * @param snapshot Indicates if snapshots need to be included, null indicates do not care.
     * @param page The 0-based page index used during pagination logic.
     * @param size The maximum amount of items on a given page.
     * @return A {@link Flux} with the game versions, or an errored {@link Flux} that indicates a failure.
     */
    public Flux<GameVersionDTO> search(String nameRegex, Boolean preRelease, Boolean snapshot, int page, int size) {
        return repository.findAllFor(nameRegex, preRelease, snapshot)
                .doFirst(() -> logger.debug("Looking up game versions in search mode. Using parameters: {}, {}, {}", nameRegex, preRelease, snapshot))
                .skip(page * size)
                .limitRequest(size)
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found game version: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Flux.error(new NoEntriesFoundException("GameVersion")));
    }

    /**
     * Counts the game versions, that match the search criteria.
     *
     * @param nameRegex The regular expression against which the name of the game version is matched.
     * @param preRelease Indicates if preReleases need to be included, null indicates do not care.
     * @param snapshot Indicates if snapshots need to be included, null indicates do not care.
     * @return A {@link Flux} with the game versions, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Long> countForSearch(String nameRegex, Boolean preRelease, Boolean snapshot) {
        return repository.findAllFor(nameRegex, preRelease, snapshot)
                .doFirst(() -> logger.debug("Counting game versions in search mode: {}, {}, {}", nameRegex, preRelease, snapshot))
                .count()
                .doOnNext(cnt -> logger.debug("Found game versions: {}", cnt));
    }

    /**
     * Deletes a given game version if it exists.
     *
     * @param id The id of the game version that should be deleted.
     * @return A {@link Mono} indicating success or failure.
     */
    public Mono<Void> deleteBy(UUID id, Principal principal) {
        return repository.deleteById(id)
                .doFirst(() -> userService.warn(logger, principal, String.format("Deleting game version with id: %s", id)))
                .doOnNext(aVoid -> userService.warn(logger, principal, String.format("Deleted game version with id: %s", id)));
    }

    /**
     * Creates a new game version from a DTO and saves it in the repository.
     *
     * @param newGameVersion The dto to create a new game version from.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<GameVersionDTO> create(GameVersionDTO newGameVersion, Principal principal) {
        return Mono.just(newGameVersion)
                .doFirst(() -> userService.warn(logger, principal, String.format("Creating new game version: %s", newGameVersion.getName())))
                .map(dto -> this.toNewDMO(dto, principal))
                .flatMap(repository::save)
                .map(this::toDTO)
                .doOnNext(dmo -> userService.warn(logger, principal, String.format("Created new game version: %s with id: %s", dmo.getName(), dmo.getId())))
                .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_game_version_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("GameVersion", "Name")));
    }

    /**
     * Updates an existing game version with the data in the dto and saves it in the repo.
     *
     * @param newGameVersion The dto to update the data in the dmo with.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<GameVersionDTO> update(UUID idToUpdate, GameVersionDTO newGameVersion, Principal principal) {
        return repository.findById(idToUpdate)
                .doFirst(() -> userService.warn(logger, principal, String.format("Updating game version: %s", idToUpdate)))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(newGameVersion.getId(), "GameVersion")))
                .doOnNext(dmo -> userService.warn(logger, principal, String.format("Updating db game version: %s with id: %s, and data: %s", dmo.getName(), dmo.getId(), newGameVersion)))
                .doOnNext(dmo -> this.updateDMO(newGameVersion, dmo)) //We use doOnNext here since this maps straight into the existing dmo that we just pulled from the DB to update.
                .doOnNext(dmo -> userService.warn(logger, principal, String.format("Updated db game version to: %s", dmo)))
                .flatMap(dmo -> repository.save(dmo)
                        .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_game_version_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("GameVersion", "Name"))))
                .map(this::toDTO)
                .doOnNext(dto -> userService.warn(logger, principal, String.format("Updated game version: %s with id: %s, to data: %s", dto.getName(), dto.getId(), dto)));
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

    private GameVersionDMO toNewDMO(GameVersionDTO dto, Principal principal) {
        return new GameVersionDMO(
                userService.getCurrentUserId(principal),
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
