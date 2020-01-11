package com.mcms.api.webapi.handlers;

import java.util.UUID;

import com.mcms.data.model.datamodel.mapping.mappable.MappableDMO;
import org.springframework.data.domain.Pageable;
import org.springframework.data.repository.reactive.ReactiveCrudRepository;

public interface ComponentRepositories extends ReactiveCrudRepository<MappableDMO, UUID> {

    Pageable

}
