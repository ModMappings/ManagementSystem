package com.mcms.data.repositories.core;

import java.util.UUID;

import com.mcms.data.model.core.MappingTypeDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.CrudRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which provides and stores {@link MappingTypeDMO} objects.
 */
public interface IMappingTypeRepository extends CrudRepository<MappingTypeDMO, UUID> {

    /**
     * Finds all mapping types which match the given name regex.
     *
     * The mapping types are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup mapping types for.
     * @param pageable The pagination information for the query.
     * @return The mapping types of which the name match the regex.
     */
    Flux<MappingTypeDMO> findAllForNameRegex(String nameRegex, final Pageable pageable);

    /**
     * Finds all mapping types which where directly made by the user with the given id.
     *
     * The mapping types are sorted newest to oldest.
     *
     * @param userId The id of the user to look up mapping types for.
     * @param pageable The pagination information for the query.
     * @return The mapping types by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<MappingTypeDMO> findAllForUser(final UUID userId, final Pageable pageable);
}
