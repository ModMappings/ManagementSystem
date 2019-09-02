using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;
using Data.Core.Readers.Method;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Method
{
    public interface IMethodMappingWriter
        : IMethodMappingReader, INoneUniqueNamedMappingWriter<MethodMapping, MethodVersionedMapping, MethodCommittedMappingEntry, MethodProposalMappingEntry, MethodReleaseMember>
    {
    }
}
