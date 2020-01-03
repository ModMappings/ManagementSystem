using System.Collections.Generic;
using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Core.Protocol.Writing;

namespace Mcms.IO.Core.Protocol.Manager
{
    public interface IIOProtocolManager
    {
        IEnumerable<IIOProtocol> Protocols { get; }

        IReadOnlyDictionary<string, IIOReader> Readers { get; }

        IReadOnlyDictionary<string, IIOWriter> Writers { get; }
    }
}
