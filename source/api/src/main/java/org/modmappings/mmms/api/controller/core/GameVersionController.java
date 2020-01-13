package org.modmappings.mmms.api.controller.core;

import java.util.UUID;

import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import org.dozer.Mapper;
import org.modmapping.mmms.repository.repositories.core.IGameVersionRepository;
import org.modmappings.mmms.api.model.core.GameVersionDTO;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import reactor.core.publisher.Mono;

@RequestMapping("/versions")
@RestController
public class GameVersionController {

    private final IGameVersionRepository repository;
    private final Mapper mapper;

    public GameVersionController(final IGameVersionRepository repository, final Mapper mapper) {
        this.repository = repository;
        this.mapper = mapper;
    }

    @ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Returns the game version with the given id.")})
    @GetMapping("get")
    public Mono<GameVersionDTO> getAllTweets(@RequestParam UUID id) {
        return repository.findById(id).map(dmo -> mapper.map(dmo, GameVersionDTO.class));
    }
}
