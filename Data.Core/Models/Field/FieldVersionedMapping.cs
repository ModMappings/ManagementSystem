using System.ComponentModel.DataAnnotations;
using Data.Core.Models.Class;
using Data.Core.Models.Core;

namespace Data.Core.Models.Field
{
    public class FieldVersionedMapping
        : AbstractVersionedMapping<FieldMapping, FieldVersionedMapping, FieldCommittedMappingEntry, FieldProposalMappingEntry>
    {
        [Required]
        public ClassCommittedMappingEntry MemberOf { get; set; }
    }
}
