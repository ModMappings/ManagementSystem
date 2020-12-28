package org.modmappings.mmms.api.controllers.core;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.enums.ParameterIn;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.services.core.GameVersionService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.api.springdoc.PageableAsQueryParam;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.server.reactive.ServerHttpResponse;
import org.springframework.web.bind.annotation.*;
import reactor.core.publisher.Mono;

import java.util.UUID;

@Tag(name = "GameVersions", description = "Gives access to available game versions. The system creates the game versions automatically when new Minecraft releases are created and published by Mojang.")
@RequestMapping("/versions")
@RestController
public class GameVersionController {

    private final GameVersionService gameVersionService;
    private final UserService userService;

    public GameVersionController(final GameVersionService gameVersionService, final UserService userService) {
        this.gameVersionService = gameVersionService;
        this.userService = userService;
    }

    @Operation(
            operationId = "getGameVersionById",
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
    public Mono<GameVersionDTO> getBy(@PathVariable final UUID id, final ServerHttpResponse response) {
        return gameVersionService.getBy(id)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            operationId = "getGameVersionsBySearchCriteria",
            summary = "Gets all known game versions and finds the ones that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "name",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the name of the version against.",
                            example = ".*"
                    ),
                    @Parameter(
                            name = "preRelease",
                            in = ParameterIn.QUERY,
                            description = "Indicator if filtering on pre-releases is needed or not. Leave the parameter out if you do not care for filtering on pre-releases or not.",
                            example = "false"
                    ),
                    @Parameter(
                            name = "snapshot",
                            in = ParameterIn.QUERY,
                            description = "Indicator if filtering on snapshots is needed or not. Leave the parameter out if you do not care for filtering on snapshots or not.",
                            example = "false"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all game versions in the database, that match the search criteria."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no game version exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.APPLICATION_JSON_VALUE})
    @PageableAsQueryParam
    public Mono<Page<GameVersionDTO>> getAll(
            final @RequestParam(name = "name", required = false) String nameRegex,
            final @RequestParam(name = "preRelease", required = false) Boolean isPreRelease,
            final @RequestParam(name = "snapshot", required = false) Boolean isSnapshot,
            final @PageableDefault(size = 25) Pageable pageable,
            final ServerHttpResponse response) {
        return gameVersionService.getAll(nameRegex, isPreRelease, isSnapshot, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
