using Data.Core.Models.Core;

namespace Data.Core.Models.Class
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

        /// <summary>
        /// The parent class.
        /// Is null if this is not an internal class.
        /// </summary>
        IClassMapping Parent { get; set; }
    }
}
