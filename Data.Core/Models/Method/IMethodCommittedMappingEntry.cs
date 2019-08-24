using Data.Core.Models.Core;

namespace Data.Core.Models.Method
{
    /// <summary>
    /// A single entry in the history of the mapping
    /// </summary>
    public interface IMethodCommittedMappingEntry : ICommittedMappingEntry<IMethodProposalMappingEntry>
    {
    }
}
