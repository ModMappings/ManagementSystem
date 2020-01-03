using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Core.Protocol.Writing;

namespace Mcms.IO.Core.Protocol.Builder
{
    public interface IIOProtocolConfiguration
    {
        IIOProtocolConfiguration WithName(string name);
        IIOProtocolConfiguration WithReader(IIOReader reader);
        IIOProtocolConfiguration WithWriter(IIOWriter writer);
    }
}
