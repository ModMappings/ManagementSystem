using System.Collections.Generic;
using Data.Core.Models.Core;
using Data.Core.Models.Field;
using Data.Core.Models.Method;

namespace Data.Core.Models.Class
{
    /// <summary>
    /// Represents a single class mapping within a project.
    /// </summary>
    public interface IClassVersionedMapping : IVersionedMapping<IClassCommittedMappingEntry, IClassProposalMappingEntry>
    {
        /// <summary>
        /// The methods that are part of this class.
        /// </summary>
        ICollection<IMethodVersionedMapping> Methods { get; set; }

        /// <summary>
        /// The fields that are part of this class.
        /// </summary>
        ICollection<IFieldVersionedMapping> Fields { get; set; }
    }
}
