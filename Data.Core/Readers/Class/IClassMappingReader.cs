using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Core.Models.Class;
using Data.Core.Models.Core;
using Data.Core.Models.Method;
using Data.Core.Readers.Core;

namespace Data.Core.Readers.Class
{
    public interface IClassMappingReader
        : IUniqueNamedMappingReader<ClassMapping, ClassVersionedMapping, ClassCommittedMappingEntry, ClassProposalMappingEntry, ClassReleaseMember>
    {
    }
}
