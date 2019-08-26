using System.ComponentModel.DataAnnotations;
using Data.Core.Models.Core;

namespace Data.Core.Models.Class
{
    public class ClassCommittedMappingEntry
        : AbstractCommittedMappingEntry<ClassMapping, ClassVersionedMapping, ClassCommittedMappingEntry, ClassProposalMappingEntry>
    {
        [Required]
        public string Package { get; set; }

        public ClassCommittedMappingEntry Parent { get; set; }
    }
}
