package org.modmappings.mmms.api.services.core.release;

import org.modmappings.mmms.api.model.core.release.ReleaseDTO;
import org.modmappings.mmms.api.services.utils.exceptions.EntryNotFoundException;
import org.modmappings.mmms.api.services.utils.exceptions.InsertionFailureDueToDuplicationException;
import org.modmappings.mmms.api.services.utils.exceptions.NoEntriesFoundException;
import org.modmappings.mmms.api.services.utils.user.UserLoggingService;
import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.modmappings.mmms.repository.model.core.release.ReleaseComponentDMO;
import org.modmappings.mmms.repository.model.core.release.ReleaseDMO;
import org.modmappings.mmms.repository.repositories.comments.CommentRepository;
import org.modmappings.mmms.repository.repositories.core.IMappingTypeRepository;
import org.modmappings.mmms.repository.repositories.core.release.ReleaseComponentRepository;
import org.modmappings.mmms.repository.repositories.core.release.ReleaseRepository;
import org.modmappings.mmms.repository.repositories.mapping.mappings.IMappingRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Component;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;
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
    private final IMappingRepository mappingRepository;
    private final IMappingTypeRepository mappingTypeRepository;
    private final CommentRepository commentRepository;
    
    private final UserLoggingService userLoggingService;

    public ReleaseService(ReleaseRepository repository, ReleaseComponentRepository releaseComponentRepository, IMappingRepository mappingRepository, IMappingTypeRepository mappingTypeRepository, CommentRepository commentRepository, UserLoggingService userLoggingService) {
        this.repository = repository;
        this.releaseComponentRepository = releaseComponentRepository;
        this.mappingRepository = mappingRepository;
        this.mappingTypeRepository = mappingTypeRepository;
        this.commentRepository = commentRepository;
        this.userLoggingService = userLoggingService;
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
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Only return a release when it is supposed to be visible.
                .flatMap(this::toDTO)
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
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Only return releases which are visible to the outside world.
                .skip(page * size)
                .limitRequest(size)
                .flatMap(this::toDTO)
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
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Only count releases which are visible to the outside world.
                .count()
                .doFirst(() -> logger.debug("Determining the available amount of releases"))
                .doOnNext((cnt) -> logger.debug("There are {} releases available.", cnt));
    }

    /**
     * Looks up multiple releases, that match the search criteria.
     * The returned order is newest to oldest.
     *
     * @param nameRegex The regex to filter the name on-
     * @param gameVersionId The id of the game version to filter releases on.
     * @param mappingTypeId The id of the mapping type to filter releases on.
     * @param isSnapshot Indicate if snapshots should be included or not.
     * @param mappingId The id of the mapping to filter releases on.
     * @param userId The id of the creating user to filter releases on.
     * @param page The 0-based page index used during pagination logic.
     * @param size The maximum amount of items on a given page.
     * @return A {@link Flux} with the releases, or an errored {@link Flux} that indicates a failure.
     */
    public Flux<ReleaseDTO> search(String nameRegex,
                                   UUID gameVersionId,
                                   UUID mappingTypeId,
                                   Boolean isSnapshot,
                                   UUID mappingId,
                                   UUID userId,
                                   int page,
                                   int size) {
        return repository.findAllFor(nameRegex, gameVersionId, mappingTypeId, isSnapshot, mappingId, userId)
                .doFirst(() -> logger.debug("Looking up releases in search mode: {}, {}, {}, {}, {}, {}", nameRegex, gameVersionId, mappingId, isSnapshot, mappingId, userId))
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Only return releases which are not visible to the outside world.
                .skip(page * size)
                .limitRequest(size)
                .flatMap(this::toDTO)
                .doOnNext(dto -> logger.debug("Found release: {} with id: {}", dto.getName(), dto.getId()))
                .switchIfEmpty(Flux.error(new NoEntriesFoundException("Release")));
    }

    /**
     * Counts the releases, that match the search criteria.
     *
     * @param nameRegex The regex to filter the name on-
     * @param gameVersionId The id of the game version to filter releases on.
     * @param mappingTypeId The id of the mapping type to filter releases on.
     * @param isSnapshot Indicate if snapshots should be included or not.
     * @param mappingId The id of the mapping to filter releases on.
     * @param userId The id of the creating user to filter releases on.
     * @return A {@link Mono} with the count, or an errored {@link Mono} that indicates a failure.
     */
    public Mono<Long> countForSearch(String nameRegex,
                                     UUID gameVersionId,
                                     UUID mappingTypeId,
                                     Boolean isSnapshot,
                                     UUID mappingId,
                                     UUID userId) {
        return repository.findAllFor(nameRegex, gameVersionId, mappingTypeId, isSnapshot, mappingId, userId)
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Exclude all releases for mapping types which are not visible to the outside world.
                .count()
                .doFirst(() -> logger.debug("Counting releases in search mode: {}, {}, {}, {}, {}, {}", nameRegex, gameVersionId, mappingId, isSnapshot, mappingId, userId))
                .doOnNext(cnt -> logger.debug("Found releases: {}", cnt));
    }

    /**
     * Deletes a given release if it exists.
     *
     * @param id The id of the release that should be deleted.
     * @return A {@link Mono} indicating success or failure.
     */
    public Mono<Void> deleteBy(UUID id, Supplier<UUID> userIdSupplier) {
        return repository
                .findById(id)
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible)) //Only allow deletion if the mapping type is even visible to the outside world.
                .flatMap(dmo -> repository.deleteById(id)
                        .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleting release with id: %s", id)))
                        .doOnNext(aVoid -> userLoggingService.warn(logger, userIdSupplier, String.format("Deleted release with id: %s", id))));

    }

    /**
     * Creates a new release from a DTO and saves it in the repository.
     *
     * @param gameVersionId The id of the game version to create a new release for.
     * @param mappingTypeId The id of the mapping type to create a new release for.
     * @param newRelease The dto to create a new release from.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<ReleaseDTO> create(UUID gameVersionId, UUID mappingTypeId, ReleaseDTO newRelease, Supplier<UUID> userIdSupplier) {
        return mappingTypeRepository.findById(mappingTypeId)
                .filter(MappingTypeDMO::isVisible)
                .flatMap(mdto -> Mono.just(newRelease)
                        .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Creating new release: %s", newRelease.getName())))
                        .map(dto -> this.toNewDMO(gameVersionId, mappingTypeId, dto, userIdSupplier))
                        .flatMap(repository::save) //Creates the release object in the database
                        .flatMap(dmo -> mappingRepository.findLatestForInputRegexAndOutputRegexAndMappingTypeAndGameVersion(null, null, mappingTypeId, gameVersionId) // Gets all latest mappings of the mapping type and game version.
                                .map(mdmo -> new ReleaseComponentDMO(dmo.getId(), mdmo.getId())) //Turns them into release components.
                                .collect(Collectors.toList()) //Collects them
                                .map(releaseComponentRepository::saveAll) //Saves them all in one go in the DB.
                                .then()
                                .map(aVoid -> dmo)) //Return the original DMO again so we can continue with with construction of a DTO from it.
                        .flatMap(this::toDTO) //Create the DTO from it.
                        .doOnNext(dto -> userLoggingService.warn(logger, userIdSupplier, String.format("Created new release: %s with id: %s", dto.getName(), dto.getId())))
                        .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_release_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("Release", "Name"))));
    }

    /**
     * Updates an existing release with the data in the dto and saves it in the repo.
     *
     * @param newRelease The dto to update the data in the dmo with.
     * @return A {@link Mono} that indicates success or failure.
     */
    public Mono<ReleaseDTO> update(UUID idToUpdate, ReleaseDTO newRelease, Supplier<UUID> userIdSupplier) {
        return repository.findById(idToUpdate)
                .filterWhen((dto) -> mappingTypeRepository.findById(dto.getMappingTypeId())
                        .map(MappingTypeDMO::isVisible))
                .doFirst(() -> userLoggingService.warn(logger, userIdSupplier, String.format("Updating release: %s", idToUpdate)))
                .switchIfEmpty(Mono.error(new EntryNotFoundException(newRelease.getId(), "Release")))
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Updating db release: %s with id: %s, and data: %s", dmo.getName(), dmo.getId(), newRelease)))
                .doOnNext(dmo -> this.updateDMO(newRelease, dmo)) //We use doOnNext here since this maps straight into the existing dmo that we just pulled from the DB to update.
                .doOnNext(dmo -> userLoggingService.warn(logger, userIdSupplier, String.format("Updated db release to: %s", dmo)))
                .flatMap(dmo -> repository.save(dmo)
                        .onErrorResume(throwable -> throwable.getMessage().contains("duplicate key value violates unique constraint \"IX_release_name\""), dive -> Mono.error(new InsertionFailureDueToDuplicationException("Release", "Name"))))
                .flatMap(this::toDTO)
                .doOnNext(dto -> userLoggingService.warn(logger, userIdSupplier, String.format("Updated release: %s with id: %s, to data: %s", dto.getName(), dto.getId(), dto)));
    }

    /**
     * Creates a new release DTO from the DMO.
     * Looks up the relevant components of the release in the db.
     *
     * @param dmo The DMO to turn into a DTO.
     * @return The release DMO in a Mono.
     */
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
                                                            dmo.getMappingTypeId(),
                                                            dmo.isSnapshot(),
                                                            packageComponents,
                                                            classComponents,
                                                            methodComponents,
                                                            fieldComponents,
                                                            parameterComponents,
                                                            comments
                                                    )))))));
    }

    /**
     * Creates a new initial DMO from the given parameters.
     * Pulls the name and snapshot state from the DTO passed in,
     * additionally pulls the creating user id from the supplier.
     *
     * @param gameVersionId The id of the game version to create a release for.
     * @param mappingTypeId The id of the mapping type to create a release for.
     * @param dto The DTO to pull name and snapshot state from.
     * @param userIdSupplier The supplier for the ID of the creating user.
     * @return The initial DMO with the data.
     */
    private ReleaseDMO toNewDMO(UUID gameVersionId, UUID mappingTypeId, ReleaseDTO dto, Supplier<UUID> userIdSupplier) {
        return new ReleaseDMO(
            userIdSupplier.get(),
            dto.getName(),
            gameVersionId,
            mappingTypeId,
            dto.isSnapshot()
        );
    }

    /**
     * Updates an existing DMO with information from the DTO.
     *
     * @param dto The DTO to pull information from.
     * @param dmo The DMO to write information into.
     */
    private void updateDMO(ReleaseDTO dto, ReleaseDMO dmo) {
        dmo.setName(dto.getName());
        dmo.setSnapshot(dto.isSnapshot());
    }
}