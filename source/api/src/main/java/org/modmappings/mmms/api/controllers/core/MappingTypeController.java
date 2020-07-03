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
import org.modmappings.mmms.api.model.core.MappingTypeDTO;
import org.modmappings.mmms.api.services.core.MappingTypeService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.api.springdoc.PageableAsQueryParam;
import org.modmappings.mmms.api.util.Constants;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.server.reactive.ServerHttpResponse;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.server.ServerWebExchange;
import reactor.core.publisher.Mono;

import java.util.UUID;

@Tag(name = "MappingTypes", description = "Gives access to available mapping types. The system creates new mapping types when needed, however no external systems can create mapping types.")
@RequestMapping("/types")
@RestController
public class MappingTypeController {

    private final MappingTypeService mappingTypeService;

    public MappingTypeController(final MappingTypeService mappingTypeService) {
        this.mappingTypeService = mappingTypeService;
    }

    @Operation(
            operationId = "getMappingTypeById",
            summary = "Looks up a mapping type using a given id.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the mapping type to look up.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Returns the mapping type with the given id."),
            @ApiResponse(responseCode = "404", description = "Indicates that no mapping type with the given id could be found",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @GetMapping(value = "{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<MappingTypeDTO> getBy(@PathVariable final UUID id, final ServerHttpResponse response) {
        return mappingTypeService.getBy(id, true)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            operationId = "getMappingTypesBySearchCriteria",
            summary = "Gets all known mapping types and finds the ones that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "name",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the name of the mapping type against.",
                            example = ".*"
                    ),
                    @Parameter(
                            name = "editable",
                            in = ParameterIn.QUERY,
                            description = "Indicator if filtering on editable is needed or not. Leave the parameter out if you do not care for filtering on editable.",
                            example = "false"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all mapping types in the database, that match the search criteria."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no mapping type exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                    schema = @Schema()),
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    @PageableAsQueryParam
    public Mono<Page<MappingTypeDTO>> getAll(
            final @RequestParam(name = "name", required = false) String nameRegex,
            final @RequestParam(name = "editable", required = false) Boolean editable,
            final @PageableDefault(size = 25, sort="created_on", direction = Sort.Direction.DESC) Pageable pageable,
            final ServerHttpResponse response) {
        return mappingTypeService.getAll(nameRegex, editable, true, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
