package org.modmappings.mmms.api.controllers.utils;

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
import org.modmappings.mmms.api.services.objects.PackageService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.springdoc.PageableAsQueryParam;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.server.reactive.ServerHttpResponse;
import org.springframework.web.bind.annotation.*;
import reactor.core.publisher.Mono;

import java.util.UUID;

@Tag(name = "Package", description = "Gives access to packages in a mapping type.")
@RequestMapping("/packages")
@RestController
public class PackageController {

    private final PackageService packageService;

    public PackageController(final PackageService packageService) {
        this.packageService = packageService;
    }

    @Operation(
            operationId = "getPackagesBySearchCriteria",
            summary = "Gets all known packages that match the given parameters.",
            parameters = {
                    @Parameter(
                            name = "gameVersion",
                            description = "The id of the game version to get the package for."
                    ),
                    @Parameter(
                            name = "release",
                            description = "The id of the release to get the package for."
                    ),
                    @Parameter(
                            name = "mappingType",
                            description = "The id of the mapping type to get the package for."
                    ),
                    @Parameter(
                            name = "packagePrefix",
                            description = "The mapping specific mapping prefix."
                    ),
                    @Parameter(
                            name = "minAdditionalPackageDepth",
                            description = "Minimum depth of the package behind the prefix."
                    ),
                    @Parameter(
                            name = "maxAdditionalPackageDepth",
                            description = "Maximum depth of the package behind the prefix."
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all package in the database."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no package exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.TEXT_EVENT_STREAM_VALUE,
                                    schema = @Schema()),
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.TEXT_EVENT_STREAM_VALUE, MediaType.APPLICATION_JSON_VALUE})
    @PageableAsQueryParam
    public Mono<Page<String>> getAll(
            final @RequestParam(value = "gameVersion") UUID gameVersion,
            final @RequestParam(value = "release", required = false) UUID release,
            final @RequestParam(value = "mappingType") UUID mappingType,
            final @RequestParam(value = "packagePrefix", required = false) String packagePrefix,
            final @RequestParam(value = "minAdditionalPackageDepth", required = false) Integer minAdditionalPackageDepth,
            final @RequestParam(value = "maxAdditionalPackageDepth", required = false) Integer maxAdditionalPackageDepth,
            final @PageableDefault(size = 25, sort="created_on", direction = Sort.Direction.DESC) Pageable pageable,
            final ServerHttpResponse response) {
        return packageService.findAllBy(gameVersion, release, mappingType, packagePrefix, minAdditionalPackageDepth, maxAdditionalPackageDepth, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
