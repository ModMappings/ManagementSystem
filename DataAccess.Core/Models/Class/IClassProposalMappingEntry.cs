using DataAccess.Core.Models.Core;

namespace DataAccess.Core.Models.Class
{
    /// <summary>
    /// A proposal for changing a class mapping.
    /// The actually mapped data might not changes
    /// </summary>
    public interface IClassProposalMappingEntry : IProposalMappingEntry
    {

        /// <summary>
        /// Indicates the proposed package the class is in.
        /// </summary>
        string Package { get; set; }
    }
}
