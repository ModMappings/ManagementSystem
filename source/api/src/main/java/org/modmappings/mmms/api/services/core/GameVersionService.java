package org.modmappings.mmms.api.services.core;

import org.modmappings.mmms.api.converters.GameVersionConverter;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.core.gameversions.GameVersionRepository;
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
public class GameVersionService {

    private final Logger logger = LoggerFactory.getLogger(GameVersionService.class);
    private final GameVersionRepository repository;
    private final GameVersionConverter gameVersionConverter;

    public GameVersionService(final GameVersionRepository repository, final GameVersionConverter gameVersionConverter) {
        this.repository = repository;
        this.gameVersionConverter = gameVersionConverter;
    }

    /**
     * Looks up a game version with a given id.
     *
     * @param id The id to look the game version up for.
     * @return A {@link Mono} containing the requested game version or a errored {@link Mono} that indicates a failure.
     */
    public Mono<GameVersionDTO> getBy(final UUID id) {
        return repository.findById(id)
                .doFirst(() -> logger.debug("Looking up a game version by id: {}", id))
                .map(this.gameVersionConverter::toDTO)
                .doOnNext(dto -> logger.debug("Found game version: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "GameVersion")));
    }

    /**
     * Looks up multiple game versions, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameExpression  The like expression against which the name of the game version is matched.
     * @param preRelease Indicates if preReleases need to be included, null indicates do not care.
     * @param snapshot   Indicates if snapshots need to be included, null indicates do not care.
     * @param pageable   The pagination and sorting logic for the request.
     * @return A {@link Flux} with the game versions, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Page<GameVersionDTO>> getAll(final String nameExpression, final Boolean preRelease, final Boolean snapshot, final Pageable pageable) {
        return repository.findAllBy(
                nameExpression,
                preRelease,
                snapshot,
                pageable)
                .doFirst(() -> logger.debug("Looking up game versions in search mode. Using parameters: {}, {}, {}", nameExpression, preRelease, snapshot))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this.gameVersionConverter::toDTO)
                        .collectList()
                        .map(gameVersions -> (Page<GameVersionDTO>) new PageImpl<>(gameVersions, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found game version: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("GameVersion")));
    }
}
