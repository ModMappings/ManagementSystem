package org.modmappings.mmms.repository.repositories.core.mappingtypes;

import org.modmappings.mmms.repository.model.core.MappingTypeDMO;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import reactor.core.publisher.Mono;

import java.util.UUID;

/**
 * Defines a custom repository which gives access to mapping types via
 * its custom accessor methods.
 *
 * These repositories take the external visibility of mapping types into account.
 */
public interface MappingTypeRepositoryCustom {
    Mono<MappingTypeDMO> findById(
            UUID id,
            boolean externallyVisibleOnly
    );

    /**
     * Finds all mapping types which match the given name regex.
     * and which are editable if that parameter is supplied.
     *
     * @param nameRegex The regular expression used to lookup mapping types for.
     * @param editable Indicates if filtering on editables is needed, and if editables should be included or not. Pass null as do not care indicator.
     * @param externallyVisibleOnly Indicator if only externally visible mapping types should be returned.
     * @param pageable The paging and sorting information.
     * @return The mapping types of which the name match the regex.
     */
    Mono<Page<MappingTypeDMO>> findAllBy(
            String nameRegex,
            Boolean editable,
            boolean externallyVisibleOnly,
            Pageable pageable);
}
