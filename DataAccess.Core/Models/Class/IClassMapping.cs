using System;
using System.Collections.Generic;

namespace DataAccess.Core.Models.Class
{
    /// <summary>
    /// Represents a single class mapping within a project.
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
        ICollection<IClassCommittedMappingEntry> CommittedMappings { get; set; }

        /// <summary>
        /// The proposals made for the class.
        /// </summary>
        ICollection<IClassProposalMappingEntry> ProposalMappings { get; set; }
    }
}
