package org.modmappings.mmms.api.controllers.utils;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.model.objects.PackageDTO;
import org.modmappings.mmms.api.services.objects.PackageService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.springdoc.PageableAsQueryParam;
import org.modmappings.mmms.repository.model.objects.PackageDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.web.PageableDefault;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.server.reactive.ServerHttpResponse;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
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
                            name = "latestOnly",
                            description = "Indicates if only the latest mapping for a versioned mappable should be taken into account."
                    ),
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
                            name = "pathMatchingRegex",
                            description = "The regex to match the path of the packages against."
                    ),
                    @Parameter(
                            name = "parentPackagePath",
                            description = "The path of the parent of the package. An empty string is the root of the package tree."
                    )
            }
    )
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200",
                    description = "Returns all package in the database."),
            @ApiResponse(responseCode = "404",
                    description = "Indicates that no package exists in the database.",
                    content = {
                            @Content(mediaType = MediaType.APPLICATION_JSON_VALUE,
                                    schema = @Schema())
                    })
    })
    @GetMapping(value = "", produces = {MediaType.APPLICATION_JSON_VALUE})
    @PageableAsQueryParam
    public Mono<Page<PackageDTO>> getAll(
            final @RequestParam(value = "latestOnly", required = false) Boolean latestOnly,
            final @RequestParam(value = "gameVersion", required = false) UUID gameVersion,
            final @RequestParam(value = "release", required = false) UUID release,
            final @RequestParam(value = "mappingType", required = false) UUID mappingType,
            final @RequestParam(value = "pathMatchingRegex", required = false) String matchingRegex,
            final @RequestParam(value = "parentPackagePath", required = false) String parentPackagePath,
            final @PageableDefault(size = 25) Pageable pageable,
            final ServerHttpResponse response) {
        return packageService.findAllBy(latestOnly, gameVersion, release, mappingType, matchingRegex, parentPackagePath, true, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
