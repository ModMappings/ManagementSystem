using System;
using System.Collections.Generic;
using DataAccess.Core.Models.Class;
using DataAccess.Core.Models.Parameter;

namespace DataAccess.Core.Models.Field
{
    /// <summary>
    /// Represents a single field mapping within a project.
    /// </summary>
    public interface IFieldMapping
    {
        /// <summary>
        /// The id of the field mapping.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The mapping entries made for this field.
        /// </summary>
        ICollection<IFieldCommittedMappingEntry> CommittedMappings { get; set; }

        /// <summary>
        /// The proposals made for the field.
        /// </summary>
        ICollection<IFieldProposalMappingEntry> ProposalMappings { get; set; }

        /// <summary>
        /// The class that this mapping belongs to.
        /// </summary>
        IClassMapping Class { get; set; }
    }
}
