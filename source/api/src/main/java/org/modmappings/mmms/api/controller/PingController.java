package org.modmappings.mmms.api.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;
import reactor.core.publisher.Mono;

@RestController
public class PingController {

    //@ApiResponses(value = {@ApiResponse(responseCode = "200", description = "Returns the string 'pong'.")})
    @GetMapping("/ping")
    public Mono<String> getAllTweets() {
        return Mono.create(sink -> sink.success("pong"));
    }
}
