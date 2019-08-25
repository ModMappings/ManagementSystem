using Data.Core.Models.Method;
using Data.Core.Readers.Method;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Method
{
    public interface IMethodWriter
        : IMethodReader, IMappingWriter<IMethodMapping, IMethodVersionedMapping, IMethodCommittedMappingEntry, IMethodProposalMappingEntry>
    {
    }
}
