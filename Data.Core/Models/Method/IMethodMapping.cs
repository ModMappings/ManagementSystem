using Data.Core.Models.Method;
using Data.Core.Models.Core;

namespace Data.Core.Models.Method
{
    /// <summary>
    /// Represents a single method during its lifetime in the games source code.
    /// </summary>
    public interface IMethodMapping : IMapping<IMethodVersionedMapping, IMethodCommittedMappingEntry, IMethodProposalMappingEntry>
    {

    }
}
