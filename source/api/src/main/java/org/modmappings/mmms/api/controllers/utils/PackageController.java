package org.modmappings.mmms.api.controllers.utils;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import org.modmappings.mmms.api.services.objects.PackageService;
import org.modmappings.mmms.api.services.utils.exceptions.AbstractHttpResponseException;
import org.modmappings.mmms.api.springdoc.PageableAsQueryParam;
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
                            name = "inputMatchingRegex",
                            description = "The regex to match the input mapping of the packages against. Either this or the output variant needs to be specified."
                    ),
                    @Parameter(
                            name = "outputMatchingRegex",
                            description = "The regex to match the output mapping of the packages against. Either this or the input variant needs to be specified."
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
    public Mono<Page<String>> getAll(
            final @RequestParam(value = "gameVersion", required = false) UUID gameVersion,
            final @RequestParam(value = "release", required = false) UUID release,
            final @RequestParam(value = "mappingType", required = false) UUID mappingType,
            final @RequestParam(value = "inputMatchingRegex", required = false) String inputMatchingRegex,
            final @RequestParam(value = "outputMatchingRegex", required = false) String outputMatchingRegex,
            final @PageableDefault(size = 25) Pageable pageable,
            final ServerHttpResponse response) {
        return packageService.findAllBy(gameVersion, release, mappingType, inputMatchingRegex, outputMatchingRegex, pageable)
                .onErrorResume(AbstractHttpResponseException.class, (ex) -> {
                    response.setStatusCode(HttpStatus.valueOf(ex.getResponseCode()));
                    return Mono.empty();
                });
    }
}
