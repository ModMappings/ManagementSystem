package org.modmappings.mmms.api.controller.core;

import java.util.UUID;

import com.github.dozermapper.core.Mapper;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import org.modmappings.mmms.repository.model.core.GameVersionDMO;
import org.modmappings.mmms.repository.repositories.core.IGameVersionRepository;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
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
    public Mono<GameVersionDTO> getBy(@PathVariable UUID id) {
        return repository.findById(id).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }


    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Returns all game versions in the database.")})
    @GetMapping("")
    public Flux<GameVersionDTO> findAll(final @RequestParam(name = "page", required = false, defaultValue = "0") int page,
                                       final @RequestParam(name = "size", required = false, defaultValue = "10") int size) {
        return repository.findAll().skip((page == 0 ? 0 : page * size)).limitRequest(size).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }

    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Deletes the game version with the given id.")})
    @DeleteMapping("{id}")
    public Mono<Void> deleteBy(@PathVariable UUID id) {
        logger.warn("Deleting game version: {}", id);
        return repository.deleteById(id).then(Mono.fromRunnable(() -> logger.warn("Deleted game version: {}", id)));
    }

    @PostMapping("")
    public Mono<GameVersionDTO> create(@RequestBody GameVersionDTO newGameVersion) {
        logger.warn("Creating new game version: {}", newGameVersion.getName());
        final GameVersionDMO newDto = mapper.map(newGameVersion, GameVersionDMO.class);

        return repository.save(newDto).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }
}
