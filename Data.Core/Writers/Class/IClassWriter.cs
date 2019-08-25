using Data.Core.Models.Class;
using Data.Core.Readers.Class;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Class
{
    public interface IClassWriter
        : IClassReader, IMappingWriter<IClassMapping, IClassVersionedMapping, IClassCommittedMappingEntry, IClassProposalMappingEntry>
    {
    }
}
