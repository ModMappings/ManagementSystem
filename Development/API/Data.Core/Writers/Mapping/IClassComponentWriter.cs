using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Readers.Mapping;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Mapping
{
    public interface IClassComponentWriter
        : IComponentWriter, IClassComponentReader
    {
    }
}
