using System;
using System.Collections.Generic;

namespace DataAccess.Core.Models.Class
{
    /// <summary>
    /// Represents a single mapping within a project.
    /// </summary>
    public interface IClassMapping
    {
        /// <summary>
        /// The id of the class mapping.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The mapping entries made for this class.
        /// </summary>
        ICollection<IClassCommittedMappingEntry> MappingEntries { get; set; }

        /// <summary>
        /// The proposals made for the class.
        /// </summary>
        ICollection<IClassMappingProposal> Proposals { get; set; }
    }
}
