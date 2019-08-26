using Data.Core.Models.Core;
using Data.Core.Models.Method;

namespace Data.Core.Models.Parameter
{
    public class ParameterVersionedMapping
        : AbstractVersionedMapping<ParameterMapping, ParameterVersionedMapping, ParameterCommittedMappingEntry, ParameterProposalMappingEntry>
    {
        public MethodCommittedMappingEntry ParameterOf { get; set; }
    }
}
