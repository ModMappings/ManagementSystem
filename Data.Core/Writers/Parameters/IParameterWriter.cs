using Data.Core.Models.Parameter;
using Data.Core.Readers.Parameter;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Parameters
{
    public interface IParameterWriter
        : IParameterReader, IMappingWriter<IParameterMapping, IParameterVersionedMapping, IParameterCommittedMappingEntry, IParameterProposalMappingEntry>
    {
    }
}
