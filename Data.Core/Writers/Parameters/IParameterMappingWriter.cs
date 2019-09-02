using Data.Core.Models.Core;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.Core.Readers.Parameter;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Parameters
{
    public interface IParameterMappingWriter
        : IParameterMappingReader, INoneUniqueNamedMappingWriter<ParameterMapping, ParameterVersionedMapping, ParameterCommittedMappingEntry, ParameterProposalMappingEntry, ParameterReleaseMember>
    {
    }
}
