using Data.Core.Models.Core;

namespace Data.Core.Models.Parameter
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
