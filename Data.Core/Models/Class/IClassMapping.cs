using Data.Core.Models.Core;

namespace Data.Core.Models.Class
{
    /// <summary>
    /// Represents a single class during its lifetime in the games source code.
    /// </summary>
    public interface IClassMapping : IMapping<IClassVersionedMapping, IClassCommittedMappingEntry, IClassProposalMappingEntry>
    {

    }
}
