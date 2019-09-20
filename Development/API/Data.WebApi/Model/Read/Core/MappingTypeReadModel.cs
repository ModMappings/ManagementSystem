using System;
using System.Collections.Generic;

namespace Data.WebApi.Model.Read.Core
{
    /// <summary>
    /// The read model for the mapping type.
    /// </summary>
    public class MappingTypeReadModel
    {
        /// <summary>
        /// The id of the mapping type.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the mapping type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The id of the user who created the mapping type.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The date and time when the mapping type was added to the system.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The ids of the release for this mapping type.
        /// </summary>
        public List<Guid> Releases { get; set; }
    }
}
