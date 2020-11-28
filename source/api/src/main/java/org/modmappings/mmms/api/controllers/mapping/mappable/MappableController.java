package org.modmappings.mmms.api.controllers.mapping.mappable;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.enums.ParameterIn;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.mapping.mappable.MappableDTO;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.services.mapping.mappable.MappableService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
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

@Tag(name = "Mappables", description = "Gives access to available mappables, mappables are created and controlled by the importing system, and can not be created or externally modified.")
@RequestMapping("/mappables")
@RestController
public class MappableController {

    private final MappableService mappableService;

    public MappableController(final MappableService mappableService) {
        this.mappableService = mappableService;
    }

    @Operation(
            operationId = "getMappableById",
            summary = "Looks up a mappable using a given id.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the mappable to look up.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Returns the mappable with the given id."),
            @ApiResponse(responseCode = "404", description = "Indicates that no mappable with the given id could be found",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @GetMapping(value = "{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<MappableDTO> getBy(@PathVariable final UUID id, final ServerHttpResponse response) {
        return mappableService.getBy(id)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            operationId = "getMappablesBySearchCriteria",
            summary = "Gets all known mappables that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "type",
                            description = "An optional type to limit the lookup of mappables to."
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all mappables in the database."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no mappable exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                    schema = @Schema()),
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    @PageableAsQueryParam
    public Mono<Page<MappableDTO>> getAll(
            final @RequestParam(value = "type", required = false) MappableTypeDTO type,
            final @PageableDefault(size = 25) Pageable pageable,
            final ServerHttpResponse response) {
        return mappableService.getAll(type, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
