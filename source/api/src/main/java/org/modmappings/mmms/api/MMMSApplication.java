package org.modmappings.mmms.api;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.PropertySource;
import org.springframework.context.annotation.PropertySources;

@SpringBootApplication
@PropertySources({
        @PropertySource(value = "classpath:META-INF/build-info.properties", ignoreResourceNotFound = true)
})
public class MMMSApplication {

    public static void main(final String[] args) {
        SpringApplication.run(MMMSApplication.class, args);
    }

}
