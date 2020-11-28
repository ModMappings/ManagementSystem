package org.modmappings.mmms.repository.repositories.comments.reaction;

import org.modmappings.mmms.repository.model.comments.CommentReactionDMO;
import org.modmappings.mmms.repository.repositories.ModMappingRepository;
import org.springframework.stereotype.Repository;

/**
 * Represents a repository that gives access to the comment reactions made on comments.
 * <p>
 * This repository provides both custom access to comment reactions as well as the standard crud access methods.
 */
@Repository
public interface CommentReactionRepository extends ModMappingRepository<CommentReactionDMO>, CommentReactionRepositoryCustom {
}
