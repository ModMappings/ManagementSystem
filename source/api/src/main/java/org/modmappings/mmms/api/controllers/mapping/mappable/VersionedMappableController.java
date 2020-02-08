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
import org.modmappings.mmms.api.model.mapping.mappable.VersionedMappableDTO;
import org.modmappings.mmms.api.services.mapping.mappable.VersionedMappableService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.services.utils.user.UserService;
import org.modmappings.mmms.api.springdoc.PageableAsQueryParam;
import org.modmappings.mmms.api.util.Constants;
import org.modmappings.mmms.repository.model.mapping.mappable.MappableTypeDMO;
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

@Tag(name = "Versioned Mappables", description = "Gives access to available versioned mappables, versioned mappables are created and controlled by the importing system, and can not be created or externally modified.")
@RequestMapping("/versioned_mappables")
@RestController
public class VersionedMappableController {

    private final VersionedMappableService versionedMappableService;
    private final UserService userService;

    public VersionedMappableController(final VersionedMappableService versionedMappableService, final UserService userService) {
        this.versionedMappableService = versionedMappableService;
        this.userService = userService;
    }

    @Operation(
            operationId = "getVersionedMappableById",
            summary = "Looks up a versioned mappable using a given id.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the versioned mappable to look up.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Returns the versioned mappable with the given id."),
            @ApiResponse(responseCode = "404", description = "Indicates that no versioned mappable with the given id could be found",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @GetMapping(value = "{id}", produces = MediaType.APPLICATION_JSON_VALUE)
    public Mono<VersionedMappableDTO> getBy(@PathVariable final UUID id, final ServerHttpResponse response) {
        return versionedMappableService.getBy(id)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }

    @Operation(
            operationId = "getVersionedMappablesBySearchCriteria",
            summary = "Gets all known versioned mappables that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "gameVersionId",
                            in = ParameterIn.QUERY,
                            description = "The id of the game version. Null to ignore.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "mappableType",
                            in = ParameterIn.QUERY,
                            description = "The type of the mappable to look up. Null to ignore.",
                            example = "PACKAGE"
                    ),
                    @Parameter(
                            name = "packageId",
                            in = ParameterIn.QUERY,
                            description = "The id of the package to find versioned mappables in. Null to ignore.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "classId",
                            in = ParameterIn.QUERY,
                            description = "The id of the class to find versioned mappables in. Null to ignore.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "methodId",
                            in = ParameterIn.QUERY,
                            description = "The id of the method to find versioned mappables in. Null to ignore.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "mappingId",
                            in = ParameterIn.QUERY,
                            description = "The id of the mapping to find the versioned mappables for. Null to ignore. If parameter is passed, either a single result is returned or none. Since each mapping can only target a single versioned mappable.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "superTypeTargetId",
                            in = ParameterIn.QUERY,
                            description = "The id of the class to find the super types for. Null to ignore.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    ),
                    @Parameter(
                            name = "subTypeTargetId",
                            in = ParameterIn.QUERY,
                            description = "The id of the class to find the sub types for. Null to ignore.",
                            example = "9b4a9c76-3588-48b5-bedf-b0df90b00381"
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all the versioned mappables in the database, that match the search criteria."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no versioned mappables exists in the database.",
                    content = {
                        @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                            schema = @Schema()),
                        @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    @PageableAsQueryParam
    public Mono<Page<VersionedMappableDTO>> getAll(
            final @RequestParam(value = "gameVersionId", required = false) UUID gameVersionId,
            final @RequestParam(value = "mappableType", required = false) MappableTypeDMO mappableTypeDMO,
            final @RequestParam(value = "packageId", required = false) UUID packageId,
            final @RequestParam(value = "classId", required = false) UUID classId,
            final @RequestParam(value = "methodId", required = false) UUID methodId,
            final @RequestParam(value = "mappingId", required = false) UUID mappingId,
            final @RequestParam(value = "superTypeTargetId", required = false) UUID superTypeTargetId,
            final @RequestParam(value = "subTypeTargetId", required = false) UUID subTypeTargetId,
            final @PageableDefault(size = 25, sort="created_on", direction = Sort.Direction.DESC) Pageable pageable,
            final ServerHttpResponse response) {
        return versionedMappableService.getAll(
                gameVersionId, mappableTypeDMO, packageId, classId, methodId, mappingId, superTypeTargetId, subTypeTargetId, pageable
        )
            .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                return Mono.empty();
            });
    }

    @Operation(
            operationId = "updateVersionedMappable",
            summary = "Updates, but does not create, the versioned mappable from the data in the request body.",
            description = "This converts the data in the request body into a full versioned mappable, then updates the versioned mappable with the given id, and stores the updated versioned mappable in the database. A user needs to be authorized to perform this request. A user needs to have the role 'VERSIONED_MAPPABLES_UPDATE' to execute this action successfully.",
            parameters = {
                    @Parameter(
                            name = "id",
                            in = ParameterIn.PATH,
                            required = true,
                            description = "The id of the versioned mappable to update.",
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
            @ApiResponse(responseCode = "200", description = "Updates the versioned mappable with the given id."),
            @ApiResponse(responseCode = "403", description = "The user is not authorized to perform this action.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema())),
            @ApiResponse(responseCode = "404", description = "No versioned mappable with the given id could be found.",
                    content = @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                            schema = @Schema()))
    })
    @PatchMapping(value = "{id}", consumes = MediaType.APPLICATION_JSON_VALUE, produces = MediaType.APPLICATION_JSON_VALUE)
    @PreAuthorize("hasRole('VERSIONED_MAPPABLE_UPDATE')")
    public Mono<VersionedMappableDTO> update(@PathVariable final UUID id, @RequestBody final VersionedMappableDTO releaseToUpdate, final ServerWebExchange exchange) {
        return exchange.getPrincipal()
                .flatMap(principal -> versionedMappableService.update(id, releaseToUpdate, () -> userService.getCurrentUserId(principal)))
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    exchange.getResponse().setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
