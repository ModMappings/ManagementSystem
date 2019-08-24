using System;
using System.Collections.Generic;
using Data.Core.Models.Class;
using Data.Core.Models.Core;

namespace Data.Core.Models.Field
{
    /// <summary>
    /// Represents a single field mapping within a project.
    /// </summary>
    public interface IFieldVersionedMapping : IVersionedMapping<IFieldCommittedMappingEntry, IFieldProposalMappingEntry>
    {
        /// <summary>
        /// The class that this mapping belongs to.
        /// </summary>
        IClassVersionedMapping ClassVersioned { get; set; }
    }
}
