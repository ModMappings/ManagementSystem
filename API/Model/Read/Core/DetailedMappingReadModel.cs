using System;
using System.Collections.Generic;

namespace API.Model.Read.Core
{
    /// <summary>
    /// A detailed read model for mappings.
    /// </summary>
    public class DetailedMappingReadModel
        : MappingReadModel
    {
        /// <summary>
        /// The id of the versioned mapping this mapping belongs to.
        /// </summary>
        public Guid VersionedMapping { get; set; }

        /// <summary>
        /// The Id of the proposal that caused this mapping to be created.
        /// </summary>
        public Guid Proposal { get; set; }

        /// <summary>
        /// The releases this mapping is part of.
        /// </summary>
        public IEnumerable<Guid> Releases { get; set; }
    }
}
