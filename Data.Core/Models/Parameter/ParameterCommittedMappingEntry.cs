using System.ComponentModel.DataAnnotations;
using Data.Core.Models.Core;

namespace Data.Core.Models.Parameter
{
    public class ParameterCommittedMappingEntry
        : AbstractCommittedMappingEntry<ParameterMapping, ParameterVersionedMapping, ParameterCommittedMappingEntry, ParameterProposalMappingEntry>
    {
        [Required]
        public int Index { get; set; }
    }
}
