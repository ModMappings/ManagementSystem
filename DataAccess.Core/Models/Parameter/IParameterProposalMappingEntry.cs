using DataAccess.Core.Models.Core;

namespace DataAccess.Core.Models.Parameter
{
    /// <summary>
    /// A proposal for changing a parameter mapping.
    /// The actually mapped data might not changes
    /// </summary>
    public interface IParameterProposalMappingEntry : IProposalMappingEntry
    {
        /// <summary>
        /// The index of the parameter mapping in zero-order.
        /// </summary>
        int Index { get; set; }
    }
}
