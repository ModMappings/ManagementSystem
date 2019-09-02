using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Readers.Class;
using Data.Core.Writers.Core;

namespace Data.Core.Writers.Class
{
    public interface IClassMappingWriter
        : IClassMappingReader, IUniqueNamedMappingWriter<ClassMapping, ClassVersionedMapping, ClassCommittedMappingEntry, ClassProposalMappingEntry, ClassReleaseMember>
    {
    }
}
