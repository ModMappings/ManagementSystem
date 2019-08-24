using System;
using System.Collections.Generic;
using Data.Core.Models.Core;
using Data.Core.Models.Method;

namespace Data.Core.Models.Parameter
{
    /// <summary>
    /// Represents a single parameter mapping within a project.
    /// </summary>
    public interface IParameterVersionedMapping : IVersionedMapping<IParameterCommittedMappingEntry, IParameterProposalMappingEntry>
    {
        /// <summary>
        /// The method that this mapping belongs to.
        /// </summary>
        IMethodVersionedMapping MethodVersioned { get; set; }
    }
}
