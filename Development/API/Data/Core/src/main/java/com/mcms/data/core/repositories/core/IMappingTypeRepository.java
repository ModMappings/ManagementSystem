package com.mcms.data.core.repositories.core;

import java.util.UUID;

import com.mcms.api.datamodel.core.MappingTypeDMO;
import com.mcms.data.core.repositories.IRepository;
import reactor.core.publisher.Flux;

/**
 * Represents a repository which provides and stores {@link MappingTypeDMO} objects.
 */
public interface IMappingTypeRepository extends IRepository<MappingTypeDMO> {

    /**
     * Finds all mapping types which match the given name regex.
     *
     * The mapping types are returned in newest to oldest order.
     *
     * @param nameRegex The regular expression used to lookup mapping types for.
     * @return The mapping types of which the name match the regex.
     */
    Flux<MappingTypeDMO> findAllForNameRegex(String nameRegex);

    /**
     * Finds all mapping types which where directly made by the user with the given id.
     *
     * The mapping types are sorted newest to oldest.
     *
     * @param userId The id of the user to look up mapping types for.
     * @return The mapping types by the given user.
     * @throws IllegalArgumentException in case the given {@literal userId} is {@literal null}.
     */
    Flux<MappingTypeDMO> findAllForUser(final UUID userId);
}
