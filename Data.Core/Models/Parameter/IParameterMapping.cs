using Data.Core.Models.Parameter;
using Data.Core.Models.Core;

namespace Data.Core.Models.Parameter
{
    /// <summary>
    /// Represents a single parameter during its lifetime in the games source code.
    /// </summary>
    public interface IParameterMapping : IMapping<IParameterVersionedMapping, IParameterCommittedMappingEntry, IParameterProposalMappingEntry>
    {

    }
}
