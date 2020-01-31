package org.modmappings.mmms.api.controllers.core;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.enums.ParameterIn;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.services.core.GameVersionService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.api.util.Constants;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.server.reactive.ServerHttpResponse;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ServerWebExchange;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

import java.util.UUID;

@Tag(name = "GameVersions", description = "Gives access to available game versions, allows existing game versions to be modified and new ones to be created.")
@RequestMapping("/versions")
@RestController
public class GameVersionController {

    private final GameVersionService gameVersionService;
    private final UserService userService;

    public GameVersionController(final GameVersionService gameVersionService, UserService userService) {
        this.gameVersionService = gameVersionService;
        this.userService = userService;
    }

    @Operation(
            summary = "Looks up a game version using a given id.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the game version to look up.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Returns the game version with the given id."),
            @ApiResponse(responseCode = "404", description = "Indicates that no game version with the given id could be found",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @GetMapping(value = "{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<GameVersionDTO> getBy(@PathVariable UUID id, ServerHttpResponse response) {
        return gameVersionService.getBy(id)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Looks up all game versions.",
            parameters = {
                    @Parameter(
                            name = "page",
                            in = ParameterIn.QUERY,
                            description = "The 0-based page index to perform pagination lookup.",
                            example = "0"
                    ),
                    @Parameter(
                            name = "size",
                            in = ParameterIn.QUERY,
                            description = "The size of a given page.",
                            example = "10"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all game versions in the database."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no game version exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                    schema = @Schema()),
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    public Flux<GameVersionDTO> getAll(final @RequestParam(name = "page", required = false, defaultValue = "0") int page,
                                       final @RequestParam(name = "size", required = false, defaultValue = "10") int size,
                                       ServerHttpResponse response) {
        return gameVersionService.getAll(page, size)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Flux.empty();
                });
    }

    @Operation(
            summary = "Determines the amount of all available game versions."
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns the count of available game versions.")
    })
    @GetMapping(value = "count", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<Long> countAll(ServerHttpResponse response) {
        return gameVersionService.count()
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Searches through the known game versions and finds the ones that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "name",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the name of the version against.",
                            example = "*"
                    ),
                    @Parameter(
                            name = "isPreRelease",
                            in = ParameterIn.QUERY,
                            description = "Indicator if filtering on pre-releases is needed or not. Leave the parameter out if you do not care for filtering on pre-releases or not.",
                            example = "false"
                    ),
                    @Parameter(
                            name = "isSnapshot",
                            in = ParameterIn.QUERY,
                            description = "Indicator if filtering on snapshots is needed or not. Leave the parameter out if you do not care for filtering on snapshots or not.",
                            example = "false"
                    ),
                    @Parameter(
                            name = "page",
                            in = ParameterIn.QUERY,
                            description = "The 0-based page index to perform pagination lookup.",
                            example = "0"
                    ),
                    @Parameter(
                            name = "size",
                            in = ParameterIn.QUERY,
                            description = "The size of a given page.",
                            example = "10"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all game versions in the database, that match the search criteria."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no game version exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                    schema = @Schema()),
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "search", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    public Flux<GameVersionDTO> search(
            final @RequestParam(name = "name", required = false, defaultValue = "*") String nameRegex,
            final @RequestParam(name = "isPreRelease", required = false) Boolean isPreRelease,
            final @RequestParam(name = "isSnapshot", required = false) Boolean isSnapshot,
            final @RequestParam(name = "page", required = false, defaultValue = "0") int page,
            final @RequestParam(name = "size", required = false, defaultValue = "10") int size,
            ServerHttpResponse response) {
        return gameVersionService.getAll(nameRegex, isPreRelease, isSnapshot, page, size)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Flux.empty();
                });
    }

    @Operation(
            summary = "Determines the amount of game versions which match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "name",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the name of the version against.",
                            example = "*"
                    ),
                    @Parameter(
                            name = "isPreRelease",
                            in = ParameterIn.QUERY,
                            description = "Indicator if filtering on pre-releases is needed or not. Leave the parameter out if you do not care for filtering on pre-releases or not.",
                            example = "false"
                    ),
                    @Parameter(
                            name = "isSnapshot",
                            in = ParameterIn.QUERY,
                            description = "Indicator if filtering on snapshots is needed or not. Leave the parameter out if you do not care for filtering on snapshots or not.",
                            example = "false"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns the count of available game versions, which match the given search parameter.")
    })
    @GetMapping(value = "search/count", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<Long> countForSearch(
            final @RequestParam(name = "name", required = false, defaultValue = "*") String nameRegex,
            final @RequestParam(name = "isPreRelease", required = false) Boolean isPreRelease,
            final @RequestParam(name = "isSnapshot", required = false) Boolean isSnapshot,
            ServerHttpResponse response) {
        return gameVersionService.countForSearch(nameRegex, isPreRelease, isSnapshot)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Deletes the game version with the given id.",
            description = "This looks up the game version with the given id from the database and deletes it. A user needs to be authorized to perform this request. A user needs to have the role 'GAMEVERSIONS_DELETE' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the game version to delete.",
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
            @ApiResponse(responseCode = "200", description = "Deletes the game version with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @DeleteMapping("{id}")
    @PreAuthorize("hasRole('GAMEVERSIONS_DELETE')")
    public Mono<Void> deleteBy(@PathVariable UUID id, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> gameVersionService.deleteBy(id, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Creates the game version from the data in the request body.",
            description = "This converts the data in the request body into a full game version, and stores it in the database. The name of the game version can not already be in use. A user needs to be authorized to perform this request. A user needs to have the role 'GAMEVERSIONS_CREATE' to execute this action successfully.",
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
            @ApiResponse(responseCode = "200", description = "Creates the game version with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given game version is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PostMapping(value = "", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('GAMEVERSIONS_CREATE')")
    public Mono<GameVersionDTO> create(@RequestBody GameVersionDTO newGameVersion, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> gameVersionService.create(newGameVersion, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Updates, but does not create, the game version from the data in the request body.",
            description = "This converts the data in the request body into a full game version, then updates the game version with the given id, and stores the updated game version in the database. The new name of the game version can not already be in use by a different game version. A user needs to be authorized to perform this request. A user needs to have the role 'GAMEVERSION_UPDATES' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the game version to update.",
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
            @ApiResponse(responseCode = "200", description = "Updates the game version with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given game version is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "404", description = "No game version with the given id could be found.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PatchMapping(value = "{id}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('GAMEVERSIONS_UPDATE')")
    public Mono<GameVersionDTO> update(@PathVariable UUID id, @RequestBody GameVersionDTO gameVersionToUpdate, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> gameVersionService.update(id, gameVersionToUpdate, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
