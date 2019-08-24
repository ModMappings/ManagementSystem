using DataAccess.Core.Models.Core;

namespace DataAccess.Core.Models.Field
{
    /// <summary>
    /// A single entry in the history of the mapping
    /// </summary>
    public interface IFieldCommittedMappingEntry : ICommittedMappingEntry<IFieldProposalMappingEntry>
    {
    }
}
