using System.ComponentModel.DataAnnotations;
using Data.Core.Models.Class;
using Data.Core.Models.Core;

namespace Data.Core.Models.Field
{
    public class FieldVersionedMapping
        : AbstractVersionedMapping<FieldMapping, FieldVersionedMapping, FieldCommittedMappingEntry, FieldProposalMappingEntry>
    {
        [Required]
        public ClassVersionedMapping MemberOf { get; set; }

        [Required]
        public bool IsStatic { get; set; }
    }
}
