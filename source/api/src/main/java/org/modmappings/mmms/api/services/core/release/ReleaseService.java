package org.modmappings.mmms.api.services.core.release;

import org.modmappings.mmms.api.converters.release.ReleaseConverter;
import org.modmappings.mmms.api.model.core.release.ReleaseDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.InsertionFailureDueToDuplicationException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.comments.comment.CommentRepository;
import org.modmappings.mmms.repository.repositories.core.mappingtypes.MappingTypeRepository;
import org.modmappings.mmms.repository.repositories.core.releases.components.ReleaseComponentRepository;
import org.modmappings.mmms.repository.repositories.core.releases.release.ReleaseRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappings.mapping.MappingRepository;
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
import java.util.stream.Collectors;

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
public class ReleaseService {

    private final Logger logger = LoggerFactory.getLogger(ReleaseService.class);
    private final ReleaseRepository repository;
    private final ReleaseComponentRepository releaseComponentRepository;
    private final MappingRepository mappingRepository;
    private final MappingTypeRepository mappingTypeRepository;
    private final ReleaseConverter releaseConverter;

    private final UserLoggingService userLoggingService;

    public ReleaseService(final ReleaseRepository repository, final ReleaseComponentRepository releaseComponentRepository, final MappingRepository mappingRepository, final MappingTypeRepository mappingTypeRepository, final ReleaseConverter releaseConverter, final UserLoggingService userLoggingService) {
        this.repository = repository;
        this.releaseComponentRepository = releaseComponentRepository;
        this.mappingRepository = mappingRepository;
        this.mappingTypeRepository = mappingTypeRepository;
        this.releaseConverter = releaseConverter;
        this.userLoggingService = userLoggingService;
    }

    /**
     * Looks up a release with a given id.
     *
     * @param id                    The id to look the release up for.
     * @param externallyVisibleOnly Indicator if only externally visible releases should be taken into account.
     * @return A {@link Mono} containing the requested release or a errored {@link Mono} that indicates a failure.
     */
    public Mono<ReleaseDTO> getBy(
            final UUID id,
            final boolean externallyVisibleOnly
    ) {
        return repository.findById(id, externallyVisibleOnly)
                .doFirst(() -> logger.debug("Looking up a release by id: {}", id))
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Only return a release when it is supposed to be visible.
                .map(this.releaseConverter::toDTO)
                .doOnNext(dto -> logger.debug("Found release: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Release")));
    }

    /**
     * Looks up multiple releases, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameRegex             The regex to filter the name on-
     * @param gameVersionId         The id of the game version to filter releases on.
     * @param mappingTypeId         The id of the mapping type to filter releases on.
     * @param isSnapshot            Indicate if snapshots should be included or not.
     * @param mappingId             The id of the mapping to filter releases on.
     * @param userId                The id of the creating user to filter releases on.
     * @param externallyVisibleOnly Indicates if only externally visible releases (those for visible mapping types) should be returned.
     *                              This value should be true for any call coming from an api endpoint.
     * @param pageable              The paging and sorting information for the request.
     * @return A {@link Flux} with the releases, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Page<ReleaseDTO>> getAllBy(final String nameRegex,
                                           final UUID gameVersionId,
                                           final UUID mappingTypeId,
                                           final Boolean isSnapshot,
                                           final UUID mappingId,
                                           final UUID userId,
                                           final boolean externallyVisibleOnly,
                                           final Pageable pageable) {
        return repository.findAllBy(nameRegex, gameVersionId, mappingTypeId, isSnapshot, mappingId, userId, externallyVisibleOnly, pageable)
                .doFirst(() -> logger.debug("Looking up releases: {}, {}, {}, {}, {}, {}, {}, {}", nameRegex, gameVersionId, mappingTypeId, isSnapshot, mappingId, userId, externallyVisibleOnly, pageable))
                .flatMap(page -> Flux.fromIterable(page)
                        .map(this.releaseConverter::toDTO)
                        .collectList()
                        .map(releases -> (Page<ReleaseDTO>) new PageImpl<>(releases, page.getPageable(), page.getTotalElements())))
                .doOnNext(page -> logger.debug("Found releases: {}", page))
                .switchIfEmpty(Mono.error(new NoEntriesFoundException("Release")));
    }

    /**
     * Deletes a given release if it exists.
     *
     * @param id                    The id of the release that should be deleted.
     * @param externallyVisibleOnly Indicator if only externally visible releases should be taken into account.
     * @param userIdSupplier        A provider that gives access to the id of the current user.
     * @return A {@link Mono} indicating success or failure.
     */
    public Mono<Void> deleteBy(
            final UUID id,
            final boolean externallyVisibleOnly,
            final Supplier<UUID> userIdSupplier) {
        return repository
                .findById(id, externallyVisibleOnly)
                .flatMap(dmo -> repository.deleteById(id)
                        .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleting release with id: %s", id)))
                        .doOnNext(aVoid -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleted release with id: %s", id))));

    }

    /**
     * Creates a new release from a DTO and saves it in the repository.
     *
     * @param gameVersionId  The id of the game version to create a new release for.
     * @param mappingTypeId  The id of the mapping type to create a new release for.
     * @param newRelease     The dto to create a new release from.
     * @param userIdSupplier The provider that gives access to the user id of the current interacting user or system.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<ReleaseDTO> create(
            final UUID gameVersionId,
            final UUID mappingTypeId,
            final ReleaseDTO newRelease,
            final Supplier<UUID> userIdSupplier
    ) {
        return mappingTypeRepository.findById(mappingTypeId)
                .filter(MappingTypeDMO::isVisible)
                .flatMap(mdto -> Mono.just(newRelease)
                        .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Creating new release: %s", newRelease.getName())))
                        .map(dto -> this.releaseConverter.toNewDMO(gameVersionId, mappingTypeId, dto, userIdSupplier))
                        .flatMap(repository::save) //Creates the release object in the database
                        .flatMap(dmo -> mappingRepository.findAllOrLatestFor(true, null, null, null, null, null, mappingTypeId, gameVersionId, null, null, null, true, Pageable.unpaged()) // Gets all latest mappings of the mapping type and game version.
                                .flatMapIterable(Function.identity()) //Unwrap the page.
                                .map(mdmo -> new ReleaseComponentDMO(dmo.getId(), mdmo.getId())) //Turns them into release components.
                                .collect(Collectors.toList()) //Collects them
                                .map(releaseComponentRepository::saveAll) //Saves them all in one go in the DB.
                                .then()
                                .map(aVoid -> dmo)) //Return the original DMO again so we can continue with with construction of a DTO from it.
                        .map(this.releaseConverter::toDTO) //Create the DTO from it.
                        .doOnNext(dto -> userLoggingService.warn(logger, userIdSupplier, String.format("Created new release: %s with id: %s", dto.getName(), dto.getId())))
                        .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_release_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("Release", "Name"))));
    }

    /**
     * Updates an existing release with the data in the dto and saves it in the repo.
     *
     * @param newRelease            The dto to update the data in the dmo with.
     * @param idToUpdate            The id of the dmo to update with the data from the dto.
     * @param externallyVisibleOnly Indicates if updates are only allowed to happen on externally visible releases.
     * @param userIdSupplier        The supplier for the user id of the current interacting user or system.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<ReleaseDTO> update(
            final UUID idToUpdate,
            final ReleaseDTO newRelease,
            final boolean externallyVisibleOnly,
            final Supplier<UUID> userIdSupplier) {
        return repository.findById(idToUpdate, externallyVisibleOnly)
                .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Updating release: %s", idToUpdate)))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(newRelease.getId(), "Release")))
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Updating db release: %s with id: %s, and data: %s", dmo.getName(), dmo.getId(), newRelease)))
                .doOnNext(dmo -> this.releaseConverter.updateDMO(newRelease, dmo)) //We use doOnNext here since this maps straight into the existing dmo that we just pulled from the DB to update.
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Updated db release to: %s", dmo)))
                .flatMap(dmo -> repository.save(dmo)
                        .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_release_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("Release", "Name"))))
                .map(this.releaseConverter::toDTO)
                .doOnNext(dto -> userLoggingService.warn(logger, userIdSupplier, String.format("Updated release: %s with id: %s, to data: %s", dto.getName(), dto.getId(), dto)));
    }
}