using Data.Core.Models.Field;
using Data.Core.Models.Core;

namespace Data.Core.Models.Field
{
    /// <summary>
    /// Represents a single field during its lifetime in the games source code.
    /// </summary>
    public interface IFieldMapping : IMapping<IFieldVersionedMapping, IFieldCommittedMappingEntry, IFieldProposalMappingEntry>
    {

    }
}
