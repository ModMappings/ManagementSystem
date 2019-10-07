using System.Threading.Tasks;
using Data.Core.Models.Core;
using Data.Core.Models.Mapping;
using Data.Core.Models.Mapping.Mappings;
using Data.Core.Readers.Core;

namespace Data.Core.Writers.Core
{
    public interface IComponentWriter
        : IComponentReader,
            IWriter<Component>,
            IWriter<VersionedComponent>,
            IWriter<CommittedMapping>,
            IWriter<ProposedMapping>
    {
    }
}
