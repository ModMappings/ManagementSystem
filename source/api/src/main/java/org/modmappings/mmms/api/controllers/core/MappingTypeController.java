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

@Tag(name = "MappingTypes", description = "Gives access to available mapping types, allows existing mapping types to be modified and new ones to be created.")
@RequestMapping("/types")
@RestController
public class MappingTypeController {

    private final MappingTypeService mappingTypeService;
    private final UserService userService;

    public MappingTypeController(final MappingTypeService mappingTypeService, UserService userService) {
        this.mappingTypeService = mappingTypeService;
        this.userService = userService;
    }

    @Operation(
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
    public Mono<MappingTypeDTO> getBy(@PathVariable UUID id, ServerHttpResponse response) {
        return mappingTypeService.getBy(id, true)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Gets all known mapping types and finds the ones that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "name",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the name of the mapping type against.",
                            example = "*"
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
    public Mono<Page<MappingTypeDTO>> getAll(
            final @RequestParam(name = "name", required = false, defaultValue = "*") String nameRegex,
            final @RequestParam(name = "editable", required = false) Boolean editable,
            final @PageableDefault(size = 25, sort="created_by") Pageable pageable,
            ServerHttpResponse response) {
        return mappingTypeService.getAll(nameRegex, editable, true, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Deletes the mapping type with the given id.",
            description = "This looks up the mapping type with the given id from the database and deletes it. A user needs to be authorized to perform this request. A user needs to have the role 'MAPPINGTYPES_DELETE' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the mapping type to delete.",
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
            @ApiResponse(responseCode = "200", description = "Deletes the mapping type with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @DeleteMapping("{id}")
    @PreAuthorize("hasRole('MAPPINGTYPES_DELETE')")
    public Mono<Void> deleteBy(@PathVariable UUID id, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> mappingTypeService.deleteBy(id, true, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Creates the mapping type from the data in the request body.",
            description = "This converts the data in the request body into a full mapping type, and stores it in the database. The name of the mapping type can not already be in use. A user needs to be authorized to perform this request. A user needs to have the role 'MAPPINGTYPES_CREATE' to execute this action successfully.",
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
            @ApiResponse(responseCode = "200", description = "Creates the mapping type with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given mapping type is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PostMapping(value = "", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('MAPPINGTYPES_CREATE')")
    public Mono<MappingTypeDTO> create(@RequestBody MappingTypeDTO newMappingType, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> mappingTypeService.create(newMappingType, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            summary = "Updates, but does not create, the mapping type from the data in the request body.",
            description = "This converts the data in the request body into a full mapping type, then updates the mapping type with the given id, and stores the updated mapping type in the database. The new name of the mapping type can not already be in use by a different mapping type. A user needs to be authorized to perform this request. A user needs to have the role 'MAPPINGTYPES_UPDATE' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the mapping type to update.",
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
            @ApiResponse(responseCode = "200", description = "Updates the mapping type with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given mapping type is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "404", description = "No mapping type with the given id could be found.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PatchMapping(value = "{id}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('MAPPINGTYPES_UPDATE')")
    public Mono<MappingTypeDTO> update(@PathVariable UUID id, @RequestBody MappingTypeDTO gameVersionToUpdate, ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> mappingTypeService.update(id, gameVersionToUpdate, true, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
