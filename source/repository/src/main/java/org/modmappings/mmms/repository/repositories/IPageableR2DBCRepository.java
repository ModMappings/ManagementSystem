package org.modmappings.mmms.repository.repositories;

import java.util.UUID;

import org.springframework.data.domain.Pageable;
import org.springframework.data.r2dbc.repository.R2dbcRepository;
import org.springframework.data.repository.NoRepositoryBean;
import reactor.core.publisher.Flux;

/**
 * An extended variant of the {@link R2dbcRepository} that adds support
 * to get the standard methods of the {@link R2dbcRepository} in a pageable
 * variant.
 *
 * Also sets the type that the id field needs to be to {@link UUID}.
 *
 * @param <T> The type stored in the repository.
 */
@NoRepositoryBean
public interface IPageableR2DBCRepository<T> extends R2dbcRepository<T, UUID> {
    /**
     * Returns all instances of the type.
     *
     * @return {@link Flux} emitting all entities in paged form.
     */
    Flux<T> findAll();
}
