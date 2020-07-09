package org.modmappings.mmms.api.controllers.mapping.mappings;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.enums.ParameterIn;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.mapping.mappable.MappableTypeDTO;
import org.modmappings.mmms.api.model.mapping.mappings.MappingDTO;
import org.modmappings.mmms.api.services.mapping.mappings.MappingService;
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

@Tag(name = "Mappings", description = "Gives access to available mappings, allows new ones to be created.")
@RequestMapping("/mappings")
@RestController
public class MappingController {

    private final MappingService mappingService;
    private final UserService userService;

    public MappingController(final MappingService mappingService, final UserService userService) {
        this.mappingService = mappingService;
        this.userService = userService;
    }

    @Operation(
            operationId = "getMappingsById",
            summary = "Looks up a mapping using a given id.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the mapping to look up.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Returns the mapping with the given id."),
            @ApiResponse(responseCode = "404", description = "Indicates that no mapping with the given id could be found",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @GetMapping(value = "{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<MappingDTO> getBy(@PathVariable final UUID id, final ServerHttpResponse response) {
        return mappingService.getBy(id, true)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            operationId = "getMappingsBySearchCriteria",
            summary = "Gets all known mappings and finds the ones that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "latestOnly",
                            in = ParameterIn.QUERY,
                            description = "Indicates if only latest mappings for a given versioned mappable should be taken into account. Defaults to true if not supplied.",
                            example = "true"
                    ),
                    @Parameter(
                            name = "versionedMappableId",
                            in = ParameterIn.QUERY,
                            description = "The id of the versioned mappable to filter on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "releaseId",
                            in = ParameterIn.QUERY,
                            description = "The id of the release to filter on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "mappableType",
                            in = ParameterIn.QUERY,
                            description = "The mappable type to filter on.",
                            example = "CLASS"
                    ),
                    @Parameter(
                            name = "inputRegex",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the input of the mapping against.",
                            example = ".*"
                    ),
                    @Parameter(
                            name = "outputRegex",
                            in = ParameterIn.QUERY,
                            description = "The regular expression to match the output of the mapping against.",
                            example = ".*"
                    ),
                    @Parameter(
                            name = "mappingTypeId",
                            in = ParameterIn.QUERY,
                            description = "The id of the mapping type to filter on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "gameVersionId",
                            in = ParameterIn.QUERY,
                            description = "The id of the game version to filter on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "createdBy",
                            in = ParameterIn.QUERY,
                            description = "The id of the user who created a mapping to filter on.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all mappings in the database, that match the search criteria."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no mapping exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                    schema = @Schema()),
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    @PageableAsQueryParam
    public Mono<Page<MappingDTO>> getAll(
            final @RequestParam(value = "latestOnly", required = false, defaultValue = "true") boolean latestOnly,
            final @RequestParam(value = "versionedMappableId", required = false) UUID versionedMappableId,
            final @RequestParam(value = "releaseId", required = false) UUID releaseId,
            final @RequestParam(value = "mappableType", required = false) MappableTypeDTO mappableType,
            final @RequestParam(value = "inputRegex", required = false) String inputRegex,
            final @RequestParam(value = "outputRegex", required = false) String outputRegex,
            final @RequestParam(value = "mappingTypeId", required = false) UUID mappingTypeId,
            final @RequestParam(value = "gameVersionId", required = false) UUID gameVersionId,
            final @RequestParam(value = "createdBy", required = false) UUID userId,
            final @PageableDefault(size = 25) Pageable pageable,
            final ServerHttpResponse response) {
        return mappingService.getAllBy(latestOnly, versionedMappableId, releaseId, mappableType, inputRegex, outputRegex, mappingTypeId, gameVersionId, userId, true, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            operationId = "createMapping",
            summary = "Creates the mapping from the data in the request body.",
            description = "This converts the data in the request body into a full mapping, and stores it in the database. The components of this mapping are populated from the latest available mappings in the given mapping type and game version. The name of the mapping can not already be in use. A user needs to be authorized to perform this request. A user needs to have the role 'MAPPINGS_CREATE' to execute this action successfully.",
            security = {
                    @SecurityRequirement(
                            name = Constants.MOD_MAPPINGS_OFFICIAL_AUTH,
                            scopes = {Constants.SCOPE_ROLES_NAME}
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Creates the mapping with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "400", description = "The name for the given mapping is already in use.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PostMapping(value = "{mappingType}/{versionedMappable}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('MAPPINGS_CREATE')")
    public Mono<MappingDTO> create(
            @PathVariable(name = "versionedMappable") final UUID versionedMappableId,
            @PathVariable(name = "mappingType") final UUID mappingTypeId,
            @RequestBody final MappingDTO newMapping,
            final ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> mappingService.create(versionedMappableId, mappingTypeId, newMapping, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
