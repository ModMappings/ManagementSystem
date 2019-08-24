using Data.Core.Models.Core;

namespace Data.Core.Models.Parameter
{
    /// <summary>
    /// A single entry in the history of the mapping
    /// </summary>
    public interface IParameterCommittedMappingEntry : ICommittedMappingEntry<IParameterProposalMappingEntry>
    {
        /// <summary>
        /// The index of the parameter mapping in zero-order.
        /// </summary>
        int Index { get; set; }
    }
}
