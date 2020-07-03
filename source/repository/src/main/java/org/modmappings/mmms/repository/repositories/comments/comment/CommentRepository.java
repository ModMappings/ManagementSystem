package org.modmappings.mmms.repository.repositories.comments.comment;

import org.modmappings.mmms.repository.model.comments.CommentDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the comments made on releases, proposed mappings etc.
 *
 * This repository provides both custom access to comments as well as the standard crud access methods.
 */
@Repository
public interface CommentRepository extends ModMappingRepository<CommentDMO>, CommentRepositoryCustom {
}
