using System;
using System.Collections.Generic;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Parameter;

namespace Data.Core.Models.Method
{
    /// <summary>
    /// Represents a single method mapping within a project.
    /// </summary>
    public interface IMethodVersionedMapping : IVersionedMapping<IMethodCommittedMappingEntry, IMethodProposalMappingEntry>
    {
        /// <summary>
        /// The class that this mapping belongs to.
        /// </summary>
        IClassVersionedMapping ClassVersioned { get; set; }

        /// <summary>
        /// The methods parameters.
        /// </summary>
        ICollection<IParameterVersionedMapping> Parameters { get; set; }
    }
}
