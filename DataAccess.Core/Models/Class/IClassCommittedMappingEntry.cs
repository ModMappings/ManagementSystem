using System;
using DataAccess.Core.Models.Core;

namespace DataAccess.Core.Models.Class
{
    /// <summary>
    /// A single entry in the history of the mapping
    /// </summary>
    public interface IClassCommittedMappingEntry : ICommittedMappingEntry<IClassProposalMappingEntry>
    {
        /// <summary>
        /// Package that the class that is being mapped resides in.
        /// </summary>
        string Package { get; set; }
    }
}
