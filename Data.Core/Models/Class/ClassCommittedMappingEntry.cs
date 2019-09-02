using System.ComponentModel.DataAnnotations;
using Data.Core.Models.Core;

namespace Data.Core.Models.Class
{
    public class ClassCommittedMappingEntry
        : AbstractCommittedMappingEntry<ClassMapping, ClassVersionedMapping, ClassTypedMapping, ClassCommittedMappingEntry, ClassProposalMappingEntry, ClassReleaseMember>
    {
    }
}
