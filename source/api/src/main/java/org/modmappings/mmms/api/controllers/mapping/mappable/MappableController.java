package org.modmappings.mmms.api.controllers.mapping.mappable;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.enums.ParameterIn;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.core.mapping.mappable.MappableDTO;
import org.modmappings.mmms.api.model.core.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.services.mapping.mappable.MappableService;
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

@Tag(name = "Mappables", description = "Gives access to available mappables, allows existing mappables to be modified and new ones to be created.")
@RequestMapping("/mappables")
@RestController
public class MappableController {

    private final MappableService mappableService;
    private final UserService userService;

    public MappableController(final MappableService mappableService, UserService userService) {
        this.mappableService = mappableService;
        this.userService = userService;
    }

    @Operation(
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
    public Mono<MappableDTO> getBy(@PathVariable UUID id, ServerHttpResponse response) {
        return mappableService.getBy(id)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
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
    public Mono<Page<MappableDTO>> getAll(
            final @RequestParam(value = "type", required = false) MappableTypeDTO type,
            final @PageableDefault(size = 25, sort="created_by") Pageable pageable,
                                       ServerHttpResponse response) {
        return mappableService.getAll(type, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Deletes the mappable with the given id.",
            description = "This looks up the mappable with the given id from the database and deletes it. A user needs to be authorized to perform this request. A user needs to have the role 'MAPPABLES_DELETE' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the mappable to delete.",
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
            @ApiResponse(responseCode = "200", description = "Deletes the mappable with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @DeleteMapping("{id}")
    @PreAuthorize("hasRole('MAPPABLES_DELETE')")
    public Mono<Void> deleteBy(@PathVariable UUID id, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> mappableService.deleteBy(id, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Creates the mappable from the data in the mappable type.",
            description = "This converts the data in the mappable into a full mappable, and stores it in the database. A user needs to be authorized to perform this request. A user needs to have the role 'MAPPABLES_CREATE' to execute this action successfully.",
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
            @ApiResponse(responseCode = "200", description = "Creates the mappable with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given mappable is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PostMapping(value = "{type}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('MAPPABLES_CREATE')")
    public Mono<MappableDTO> create(@PathVariable("type") MappableTypeDTO newMappable, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> mappableService.create(newMappable, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
