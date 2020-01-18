package org.modmappings.mmms.api.controller.core;

import java.util.UUID;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.modmappings.mmms.api.services.core.GameVersionService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.server.reactive.ServerHttpResponse;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

@Tag(name = "GameVersions", description = "Gives access to available game versions, allows existing game versions to be modified and new ones to be created.")
@RequestMapping("/versions")
@RestController
public class GameVersionController {

    private final Logger             logger = LoggerFactory.getLogger(GameVersionController.class);
    private final GameVersionService gameVersionService;

    public GameVersionController(final GameVersionService gameVersionService) {this.gameVersionService = gameVersionService;}

    @Operation(
                    summary = "Looks up a game version using a given id."
    )
    @ApiResponses(value = {
                    @ApiResponse(responseCode = "200", description = "Returns the game version with the given id."),
                    @ApiResponse(responseCode = "404", description = "Indicates that no game version with the given id could be found")
    })
    @GetMapping(value = "{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<GameVersionDTO> getBy(@PathVariable UUID id, ServerHttpResponse response) {
        return gameVersionService.getBy(id)
                               .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                                   response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                                   return Mono.empty();
                               });
    }

    @ApiResponses(value = {
                    @ApiResponse(responseCode = "200",
                                    description = "Returns all game versions in the database."),
                    @ApiResponse(responseCode = "404",
                                    description = "Indicates that no game version exists in the database.",
                                    content = @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                                        schema = @Schema()))
    })
    @GetMapping(value = "", produces = MediaType.TEXT_EVENT_STREAM_VALUE)
    public Flux<GameVersionDTO> getAll(ServerHttpResponse response) {
        return gameVersionService.getAll()
                               .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                                   response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                                   return Flux.empty();
                               });
    }

    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Deletes the game version with the given id.")})
    @DeleteMapping("{id}")
    public Mono<Void> deleteBy(@PathVariable UUID id) {
        return gameVersionService.deleteBy(id);
    }

    @PostMapping("")
    public Mono<GameVersionDTO> create(@RequestBody GameVersionDTO newGameVersion) {
        return gameVersionService.create(newGameVersion);
    }
}
