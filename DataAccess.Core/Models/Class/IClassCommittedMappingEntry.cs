using System;
using DataAccess.Core.Models.Core;

namespace DataAccess.Core.Models.Class
{
    /// <summary>
    /// A single entry in the history of the mapping
    /// </summary>
    public interface IClassCommittedMappingEntry : ICommittedMappingEntry<IClassMappingProposal>
    {
        /// <summary>
        /// The Id of the class mapping.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Package that the class that is being mapped resides in.
        /// </summary>
        IPackage Package { get; set; }
    }
}
