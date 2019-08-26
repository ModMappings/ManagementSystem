using Data.Core.Models.Core;

namespace Data.Core.Models.Method
{
    public class MethodCommittedMappingEntry
        : AbstractCommittedMappingEntry<MethodMapping, MethodVersionedMapping, MethodCommittedMappingEntry, MethodProposalMappingEntry>
    {
        public bool IsStatic { get; set; }
    }
}
