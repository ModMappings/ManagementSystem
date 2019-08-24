using System;
using System.Collections.Generic;
using Data.Core.Models.Class;
using Data.Core.Models.Parameter;

namespace Data.Core.Models.Method
{
    /// <summary>
    /// Represents a single method mapping within a project.
    /// </summary>
    public interface IMethodMapping
    {
        /// <summary>
        /// The id of the method mapping.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The mapping entries made for this method.
        /// </summary>
        ICollection<IMethodCommittedMappingEntry> CommittedMappings { get; set; }

        /// <summary>
        /// The proposals made for the method.
        /// </summary>
        ICollection<IMethodProposalMappingEntry> ProposalMappings { get; set; }

        /// <summary>
        /// The class that this mapping belongs to.
        /// </summary>
        IClassMapping Class { get; set; }

        /// <summary>
        /// The methods parameters.
        /// </summary>
        ICollection<IParameterMapping> Parameters { get; set; }
    }
}
