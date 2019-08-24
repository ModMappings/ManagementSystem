using System;
using System.Collections.Generic;
using DataAccess.Core.Models.Class;
using DataAccess.Core.Models.Method;

namespace DataAccess.Core.Models.Parameter
{
    /// <summary>
    /// Represents a single parameter mapping within a project.
    /// </summary>
    public interface IParameterMapping
    {
        /// <summary>
        /// The id of the parameter mapping.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The mapping entries made for this parameter.
        /// </summary>
        ICollection<IParameterCommittedMappingEntry> CommittedMappings { get; set; }

        /// <summary>
        /// The proposals made for the parameter.
        /// </summary>
        ICollection<IParameterProposalMappingEntry> ProposalMappings { get; set; }

        /// <summary>
        /// The method that this mapping belongs to.
        /// </summary>
        IMethodMapping Method { get; set; }
    }
}
