using DataAccess.Core.Models.Core;

namespace DataAccess.Core.Models.Method
{
    /// <summary>
    /// A single entry in the history of the mapping
    /// </summary>
    public interface IMethodCommittedMappingEntry : ICommittedMappingEntry<IMethodProposalMappingEntry>
    {
    }
}
