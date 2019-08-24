using Data.Core.Models.Class;
using Data.Core.Models.Method;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Class
{
    public interface IClassReader : IMappingReader<IClassMapping, IClassVersionedMapping, IClassCommittedMappingEntry, IClassProposalMappingEntry>
    {
    }
}
