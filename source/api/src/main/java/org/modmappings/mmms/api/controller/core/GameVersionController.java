package org.modmappings.mmms.api.controller.core;

import java.util.UUID;

import com.github.dozermapper.core.Mapper;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import org.modmappings.mmms.repository.repositories.core.IGameVersionRepository;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
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

    private final Logger logger = LoggerFactory.getLogger(GameVersionController.class);
    private final IGameVersionRepository repository;
    private final Mapper                 mapper;

    public GameVersionController(final IGameVersionRepository repository, final Mapper mapper) {
        this.repository = repository;
        this.mapper = mapper;
    }

    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Returns the game version with the given id.")})
    @GetMapping("{id}")
    public Mono<GameVersionDTO> getFor(@PathVariable UUID id) {
        return repository.findById(id).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }


    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Returns all game versions in the database.")})
    @GetMapping("")
    public Flux<GameVersionDTO> getAll(final @RequestParam(name = "page", required = false, defaultValue = "0") int page,
                                       final @RequestParam(name = "size", required = false, defaultValue = "10") int size) {
        return repository.findAll().skip((page == 0 ? 0 : page * size)).limitRequest(size).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }
}
