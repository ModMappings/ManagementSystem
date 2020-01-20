package org.modmappings.mmms.api.services.core.release;

import org.modmappings.mmms.api.model.core.release.ReleaseDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.InsertionFailureDueToDuplicationException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.comments.ICommentRepository;
import org.modmappings.mmms.repository.repositories.core.IMappingTypeRepository;
import org.modmappings.mmms.repository.repositories.core.release.IReleaseComponentRepository;
import org.modmappings.mmms.repository.repositories.core.release.IReleaseRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.security.Principal;
import java.util.UUID;
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
    private final IReleaseRepository repository;
    private final IReleaseComponentRepository releaseComponentRepository;
    private final IMappingTypeRepository mappingTypeRepository;
    private final ICommentRepository commentRepository;
    private final UserService userService;

    public ReleaseService(IReleaseRepository repository, IReleaseComponentRepository releaseComponentRepository, IMappingTypeRepository mappingTypeRepository, ICommentRepository commentRepository, UserService userService) {
        this.repository = repository;
        this.releaseComponentRepository = releaseComponentRepository;
        this.mappingTypeRepository = mappingTypeRepository;
        this.commentRepository = commentRepository;
        this.userService = userService;
    }

    /**
     * Looks up a release with a given id.
     *
     * @param id The id to look the release up for.
     * @return A {@link Mono} containing the requested release or a errored {@link Mono} that indicates a failure.
     */
    public Mono<ReleaseDTO> getBy(UUID id) {
        return repository.findById(id)
                .doFirst(() -> logger.debug("Looking up a release by id: {}", id))
                .filter(ReleaseDMO::isVisible)
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found release: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(id, "Release")));
    }

    /**
     * Looks up multiple releases.
     * The returned order is newest to oldest.
     *
     * @param page The 0-based page index used during pagination logic.
     * @param size The maximum amount of items on a given page.
     * @return A {@link Flux} with the releases, or an errored {@link Flux} that indicates a failure.
     */
    public Flux<ReleaseDTO> getAll(int page, int size) {
        return repository.findAll()
                .doFirst(() -> logger.debug("Looking up releases."))
                .skip(page * size)
                .limitRequest(size)
                .filter(ReleaseDMO::isVisible)
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found release: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Flux.error(new NoEntriesFoundException("Release")));
    }

    /**
     * Determines the amount of releases that exist in the database.
     *
     * @return A {@link Mono} that indicates the amount of releases in the database.
     */
    public Mono<Long> count() {
        return repository
                .findAll()
                .filter(ReleaseDMO::isVisible)
                .count()
                .doFirst(() -> logger.debug("Determining the available amount of releases"))
                .doOnNext((cnt) -> logger.debug("There are {} releases available.", cnt));
    }

    /**
     * Looks up multiple releases, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameRegex The regular expression against which the name of the release is matched.
     * @param editable Indicates if editable releases need to be included, null indicates do not care.
     * @param page The 0-based page index used during pagination logic.
     * @param size The maximum amount of items on a given page.
     * @return A {@link Flux} with the releases, or an errored {@link Flux} that indicates a failure.
     */
    public Flux<ReleaseDTO> search(String nameRegex, Boolean editable, int page, int size) {
        return repository.findAllFor(nameRegex, editable)
                .doFirst(() -> logger.debug("Looking up releases in search mode. Using parameters: {}, {}", nameRegex, editable))
                .skip(page * size)
                .limitRequest(size)
                .filter(ReleaseDMO::isVisible)
                .map(this::toDTO)
                .doOnNext(dto -> logger.debug("Found release: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Flux.error(new NoEntriesFoundException("Release")));
    }

    /**
     * Counts the releases, that match the search criteria.
     *
     * @param nameRegex The regular expression against which the name of the release is matched.
     * @param editable Indicates if editable releases need to be included, null indicates do not care.
     * @return A {@link Flux} with the releases, or an errored {@link Flux} that indicates a failure.
     */
    public Mono<Long> countForSearch(String nameRegex, Boolean editable) {
        return repository.findAllFor(nameRegex, editable)
                .doFirst(() -> logger.debug("Counting releases in search mode: {}, {}", nameRegex, editable))
                .filter(ReleaseDMO::isVisible)
                .count()
                .doOnNext(cnt -> logger.debug("Found releases: {}", cnt));
    }

    /**
     * Deletes a given release if it exists.
     *
     * @param id The id of the release that should be deleted.
     * @return A {@link Mono} indicating success or failure.
     */
    public Mono<Void> deleteBy(UUID id, Principal principal) {
        return repository
                .findById(id)
                .filter(ReleaseDMO::isVisible)
                .flatMap(dmo -> repository.deleteById(id)
                        .doFirst(() -> userService.warn(logger, principal, String.format("Deleting release with id: %s", id)))
                        .doOnNext(aVoid -> userService.warn(logger, principal, String.format("Deleted release with id: %s", id))));

    }

    /**
     * Creates a new release from a DTO and saves it in the repository.
     *
     * @param newRelease The dto to create a new release from.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<ReleaseDTO> create(ReleaseDTO newRelease, Principal principal) {
        return Mono.just(newRelease)
                .doFirst(() -> userService.warn(logger, principal, String.format("Creating new release: %s", newRelease.getName())))
                .map(dto -> this.toNewDMO(dto, principal))
                .flatMap(repository::save)
                .map(this::toDTO)
                .doOnNext(dmo -> userService.warn(logger, principal, String.format("Created new release: %s with id: %s", dmo.getName(), dmo.getId())))
                .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_game_version_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("Release", "Name")));
    }

    /**
     * Updates an existing release with the data in the dto and saves it in the repo.
     *
     * @param newRelease The dto to update the data in the dmo with.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<ReleaseDTO> update(UUID idToUpdate, ReleaseDTO newRelease, Principal principal) {
        return repository.findById(idToUpdate)
                .filter(ReleaseDMO::isVisible)
                .doFirst(() -> userService.warn(logger, principal, String.format("Updating release: %s", idToUpdate)))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(newRelease.getId(), "Release")))
                .doOnNext(dmo -> userService.warn(logger, principal, String.format("Updating db release: %s with id: %s, and data: %s", dmo.getName(), dmo.getId(), newRelease)))
                .filter(ReleaseDMO::isVisible)
                .doOnNext(dmo -> this.updateDMO(newRelease, dmo)) //We use doOnNext here since this maps straight into the existing dmo that we just pulled from the DB to update.
                .doOnNext(dmo -> userService.warn(logger, principal, String.format("Updated db release to: %s", dmo)))
                .flatMap(dmo -> repository.save(dmo)
                        .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_game_version_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("Release", "Name"))))
                .map(this::toDTO)
                .doOnNext(dto -> userService.warn(logger, principal, String.format("Updated release: %s with id: %s, to data: %s", dto.getName(), dto.getId(), dto)));
    }

    private Mono<ReleaseDTO> toDTO(ReleaseDMO dmo) {
        return releaseComponentRepository.findAllPackageMappingIdsForRelease(dmo.getId())
            .collect(Collectors.toSet())
            .flatMap(packageComponents -> releaseComponentRepository.findAllClassMappingIdsForRelease(dmo.getId())
                .collect(Collectors.toSet())
                .flatMap(classComponents -> releaseComponentRepository.findAllMethodMappingIdsForRelease(dmo.getId())
                        .collect(Collectors.toSet())
                        .flatMap(methodComponents -> releaseComponentRepository.findAllFieldMappingIdsForRelease(dmo.getId())
                                .collect(Collectors.toSet())
                                .flatMap(fieldComponents -> releaseComponentRepository.findAllParameterMappingIdsForRelease(dmo.getId())
                                        .collect(Collectors.toSet())
                                        .flatMap(parameterComponents -> commentRepository.findAllForRelease(dmo.getId())
                                                .collect(Collectors.toSet())
                                                .map(comments -> comments.stream().map(CommentDMO::getId).collect(Collectors.toSet()))
                                                .map(comments -> new ReleaseDTO(
                                                            dmo.getId(),
                                                            dmo.getCreatedBy(),
                                                            dmo.getCreatedOn(),
                                                            dmo.getName(),
                                                            dmo.getGameVersionId(),
                                                            dmo.getMappingType(),
                                                            dmo.isSnapshot(),
                                                            packageComponents,
                                                            classComponents,
                                                            methodComponents,
                                                            fieldComponents,
                                                            parameterComponents,
                                                            comments
                                                    )))))));
    }

    private ReleaseDMO toNewDMO(ReleaseDTO dto, Principal principal) {
        return new ReleaseDMO(
            userService.getCurrentUserId(principal),
            dto.getName(),
            dto.getGameVersionId(),
            dto.getMappingType(),
            dto.isSnapshot()
        );
    }

    private void updateDMO(ReleaseDTO dto, ReleaseDMO dmo) {
        dmo.setName();
    }
}