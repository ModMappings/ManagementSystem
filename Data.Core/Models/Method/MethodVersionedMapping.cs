using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Parameter;

namespace Data.Core.Models.Method
{
    public class MethodVersionedMapping
        : AbstractVersionedMapping<MethodMapping, MethodVersionedMapping, MethodCommittedMappingEntry, MethodProposalMappingEntry>
    {
        [Required]
        public ClassCommittedMappingEntry MemberOf { get; set; }

        public List<ParameterCommittedMappingEntry> Parameters { get; set; }

        public bool IsStatic { get; set; }

        public string Descriptor { get; set; }
    }
}
