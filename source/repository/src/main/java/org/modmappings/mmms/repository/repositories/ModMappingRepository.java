package org.modmappings.mmms.repository.repositories;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.repository.NoRepositoryBean;
import org.springframework.data.repository.reactive.ReactiveCrudRepository;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a repository used inside the mod mappings systems.
 *
 * @param <T> The type of entity that this repository provides.
 */
@NoRepositoryBean
public interface ModMappingRepository<T> extends ReactiveCrudRepository<T, UUID>, R2dbcRepository<T, UUID> {

    /**
     * Gets all entries in the repository.
     * Is identical to {@link #findAll()} but accepts page and sorting information.
     *
     * @param pageable The page and sorting information.
     * @return The page that is requested.
     */
    Mono<Page<T>> findAll(
            Pageable pageable
    );
}
