using System.Collections.Generic;

namespace Data.Core.Models.Mapping.Mappings
{

    /// <summary>
    /// Represents a single mapping which is "live".
    /// It is committed to be used in the public, and will be included in the next release.
    /// </summary>
    public class CommittedMapping
            : MappingBase
    {
        /// <summary>
        /// The proposal that was turned into this mapping.
        /// Might be null, if imported, or the mapping was created directly by a user with the right access rights.
        /// </summary>
        public virtual ProposedMapping ProposedMapping { get; set; }

        /// <summary>
        /// The releases that this mapping is part of.
        /// </summary>
        public virtual List<ReleaseComponent> Releases { get; set; }
    }
}
