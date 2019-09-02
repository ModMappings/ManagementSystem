using System.ComponentModel.DataAnnotations;
using Data.Core.Models.Core;

namespace Data.Core.Models.Parameter
{
    public class ParameterProposalMappingEntry
        : AbstractProposalMappingEntry<ParameterMapping, ParameterVersionedMapping, ParameterTypedMapping, ParameterCommittedMappingEntry, ParameterProposalMappingEntry, ParameterReleaseMember>
    {
    }
}
