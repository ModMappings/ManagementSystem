package org.modmappings.mmms.api;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.reactive.config.EnableWebFlux;

@SpringBootApplication
public class MMMSApplication {

	public static void main(String[] args) {
		SpringApplication.run(MMMSApplication.class, args);
	}

}
