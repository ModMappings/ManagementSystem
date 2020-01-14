package org.modmappings.mmms.api.controller.core;

import java.util.UUID;
import java.util.logging.LogManager;
import java.util.logging.Logger;

import javax.validation.constraints.NotNull;

import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import org.apache.commons.logging.Log;
import org.dozer.Mapper;
import org.modmappings.mmms.repository.repositories.core.IGameVersionRepository;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import reactor.core.publisher.Flux;
import reactor.core.publisher.Mono;

@RequestMapping("/versions")
@RestController
public class GameVersionController {

    private final Logger LOG =
                    LogManager.getLogManager().getLogger(GameVersionController.class.getName());

    private final IGameVersionRepository repository;
    private final Mapper mapper;

    public GameVersionController(final IGameVersionRepository repository, final Mapper mapper) {
        this.repository = repository;
        this.mapper = mapper;
    }

    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Returns the game version with the given id.")})
    @GetMapping("{id}")
    public Mono<GameVersionDTO> getFor(@PathVariable UUID id) {
        return repository.findById(id).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }


    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Returns the game version with the given id.")})
    @GetMapping("")
    public Flux<GameVersionDTO> getAll(final @RequestParam(name = "page") int page,
                                       final @RequestParam(name = "size") int size) {
        return repository.findAllSimple().map(dmo -> {
            LOG.warning(dmo.toString());
            return dmo;
        }).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }
}
