package com.mcms.api.webapi.handlers;

import java.util.UUID;

import com.mcms.api.datamodel.mapping.mappable.MappableDMO;
import org.springframework.data.repository.reactive.ReactiveCrudRepository;

public interface ComponentRepositories extends ReactiveCrudRepository<MappableDMO, UUID> {


}
