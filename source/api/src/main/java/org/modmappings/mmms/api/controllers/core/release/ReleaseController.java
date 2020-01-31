package org.modmappings.mmms.api.controllers.core.release;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.enums.ParameterIn;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.core.release.ReleaseDTO;
import org.modmappings.mmms.api.services.core.release.ReleaseService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.api.util.Constants;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.server.reactive.ServerHttpResponse;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ServerWebExchange;
import reactor.core.publisher.Mono;

import java.util.UUID;

@Tag(name = "Releases", description = "Gives access to available releases, allows existing releases to be modified and new ones to be created.")
@RequestMapping("/releases")
@RestController
public class ReleaseController {

    private final ReleaseService releaseService;
    private final UserService userService;

    public ReleaseController(final ReleaseService releaseService, UserService userService) {
        this.releaseService = releaseService;
        this.userService = userService;
    }

    @Operation(
            summary = "Looks up a release using a given id.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the release to look up.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Returns the release with the given id."),
            @ApiResponse(responseCode = "404", description = "Indicates that no release with the given id could be found",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @GetMapping(value = "{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<ReleaseDTO> getBy(@PathVariable UUID id, ServerHttpResponse response) {
        return releaseService.getBy(id, true)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Gets all known releases and finds the ones that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "name",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the name of the release against.",
                            example = "*"
                    ),
                    @Parameter(
                            name = "gameVersion",
                            in = ParameterIn.QUERY,
                            description = "The id of the game version to filter releases on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "mappingType",
                            in = ParameterIn.QUERY,
                            description = "The id of the mapping type to filter releases on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "isSnapshot",
                            in = ParameterIn.QUERY,
                            description = "Determines if snapshot releases are supposed to be filtered out, leave the parameter out to not filter on snapshot state of releases.",
                            example = "*"
                    ),
                    @Parameter(
                            name = "mapping",
                            in = ParameterIn.QUERY,
                            description = "The id of the mapping to filter releases on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "user",
                            in = ParameterIn.QUERY,
                            description = "The id of the user who created the release to filter releases on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all releases in the database, that match the search criteria."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no release exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                    schema = @Schema()),
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    public Mono<Page<ReleaseDTO>> getAll(
            final @RequestParam(name = "nameRegex", required = false) String nameRegex,
            final @RequestParam(name = "gameVersion", required = false) UUID gameVersionId,
            final @RequestParam(name = "mappingType", required = false) UUID mappingTypeId,
            final @RequestParam(name = "snapshot", required = false) Boolean isSnapshot,
            final @RequestParam(name = "mapping", required = false) UUID mappingId,
            final @RequestParam(name = "user", required = false) UUID userId,
            final @PageableDefault(size = 25, sort="created_by") Pageable pageable,
            ServerHttpResponse response) {
        return releaseService.getAllBy(nameRegex, gameVersionId, mappingTypeId, isSnapshot, mappingId, userId, true, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Deletes the release with the given id.",
            description = "This looks up the release with the given id from the database and deletes it. A user needs to be authorized to perform this request. A user needs to have the role 'RELEASES_DELETE' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the release to delete.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            },
            security = {
                    @SecurityRequirement(
                            name = Constants.MOD_MAPPINGS_OFFICIAL_AUTH,
                            scopes = {Constants.SCOPE_ROLES_NAME}
                    ),
                    @SecurityRequirement(
                            name = Constants.MOD_MAPPINGS_DEV_AUTH,
                            scopes = {Constants.SCOPE_ROLES_NAME}
                    ),
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Deletes the release with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @DeleteMapping("{id}")
    @PreAuthorize("hasRole('RELEASES_DELETE')")
    public Mono<Void> deleteBy(@PathVariable UUID id, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> releaseService.deleteBy(id, true, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Creates the release from the data in the request body.",
            description = "This converts the data in the request body into a full release, and stores it in the database. The components of this release are populated from the latest available mappings in the given mapping type and game version. The name of the release can not already be in use. A user needs to be authorized to perform this request. A user needs to have the role 'RELEASES_CREATE' to execute this action successfully.",
            security = {
                    @SecurityRequirement(
                            name = Constants.MOD_MAPPINGS_OFFICIAL_AUTH,
                            scopes = {Constants.SCOPE_ROLES_NAME}
                    ),
                    @SecurityRequirement(
                            name = Constants.MOD_MAPPINGS_DEV_AUTH,
                            scopes = {Constants.SCOPE_ROLES_NAME}
                    ),
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Creates the release with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given release is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PostMapping(value = "{mappingType}/{gameVersion}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('RELEASES_CREATE')")
    public Mono<ReleaseDTO> create(
            @PathVariable(name = "gameVersion") UUID gameVersionId,
            @PathVariable(name = "mappingType") UUID mappingTypeId,
            @RequestBody ReleaseDTO newRelease,
            ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> releaseService.create(gameVersionId, mappingTypeId, newRelease, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Updates, but does not create, the release from the data in the request body.",
            description = "This converts the data in the request body into a full release, then updates the release with the given id, and stores the updated release in the database. The new name of the release can not already be in use by a different release. A user needs to be authorized to perform this request. A user needs to have the role 'RELEASE_UPDATES' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the release to update.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            },
            security = {
                    @SecurityRequirement(
                            name = Constants.MOD_MAPPINGS_OFFICIAL_AUTH,
                            scopes = {Constants.SCOPE_ROLES_NAME}
                    ),
                    @SecurityRequirement(
                            name = Constants.MOD_MAPPINGS_DEV_AUTH,
                            scopes = {Constants.SCOPE_ROLES_NAME}
                    ),
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Updates the release with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given release is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "404", description = "No release with the given id could be found.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PatchMapping(value = "{id}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('RELEASES_UPDATE')")
    public Mono<ReleaseDTO> update(@PathVariable UUID id, @RequestBody ReleaseDTO releaseToUpdate, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> releaseService.update(id, releaseToUpdate, true, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
