using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Field;
using Data.Core.Readers.Field;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Field
{
    public interface IFieldMappingWriter
        : IFieldMappingReader, INoneUniqueNamedMappingWriter<FieldMapping, FieldVersionedMapping, FieldCommittedMappingEntry, FieldProposalMappingEntry, FieldReleaseMember>
    {
    }
}
