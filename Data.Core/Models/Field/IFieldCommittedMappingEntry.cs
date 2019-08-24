using Data.Core.Models.Core;

namespace Data.Core.Models.Field
{
    /// <summary>
    /// A single entry in the history of the mapping
    /// </summary>
    public interface IFieldCommittedMappingEntry : ICommittedMappingEntry<IFieldProposalMappingEntry>
    {
    }
}
