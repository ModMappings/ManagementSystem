using System.Collections.Generic;
using System.Linq;
using Mcms.IO.Core.Protocol;
using Mcms.IO.Core.Protocol.Manager;
using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Core.Protocol.Writing;

namespace Mcms.IO.DI.Protocol.Manager
{
    public class IOProtocolManager : IIOProtocolManager
    {
        public IOProtocolManager(IEnumerable<IIOProtocol> protocols)
        {
            Protocols = protocols.ToList();
            Readers = Protocols.Where(p => p.Reader != null).ToDictionary(p => p.Name, p => p.Reader);
            Writers = Protocols.Where(p => p.Writer != null).ToDictionary(p => p.Name, p => p.Writer);
        }

        public IEnumerable<IIOProtocol> Protocols { get; }
        public IReadOnlyDictionary<string, IIOReader> Readers { get; }
        public IReadOnlyDictionary<string, IIOWriter> Writers { get; }
    }
}
