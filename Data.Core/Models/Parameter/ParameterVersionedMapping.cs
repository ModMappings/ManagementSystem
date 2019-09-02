using System.ComponentModel.DataAnnotations;
using Data.Core.Models.Core;
using Data.Core.Models.Method;

namespace Data.Core.Models.Parameter
{
    public class ParameterVersionedMapping
        : AbstractVersionedMapping<ParameterMapping, ParameterVersionedMapping, ParameterTypedMapping, ParameterCommittedMappingEntry, ParameterProposalMappingEntry, ParameterReleaseMember>
    {
        [Required]
        public virtual MethodVersionedMapping ParameterOf { get; set; }

        [Required]
        public int Index { get; set; }
    }
}
