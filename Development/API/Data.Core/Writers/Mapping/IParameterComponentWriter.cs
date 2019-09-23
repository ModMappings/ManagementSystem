using Data.Core.Readers.Mapping;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Mapping
{
    public interface IParameterComponentWriter
        : IComponentWriter, IParameterComponentReader
    {
    }
}
