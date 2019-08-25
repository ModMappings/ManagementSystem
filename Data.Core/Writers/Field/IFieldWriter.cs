using Data.Core.Models.Field;
using Data.Core.Readers.Field;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Field
{
    public interface IFieldWriter
        : IFieldReader, IMappingWriter<IFieldMapping, IFieldVersionedMapping, IFieldCommittedMappingEntry, IFieldProposalMappingEntry>
    {
    }
}
