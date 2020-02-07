package org.modmappings.mmms.api.model.mapping.mappings;

import io.swagger.v3.oas.annotations.media.Schema;

/**
 * Represents all possible distribution a given mapping could be in.
 */
@Schema(name = "Distribution", description = "The distributions a given versioned mappable can be found in, based on a given mapping.", enumAsRef = true)
public enum DistributionDTO {
    BOTH,
    SERVER_ONLY,
    CLIENT_ONLY,
    UNKNOWN
}
