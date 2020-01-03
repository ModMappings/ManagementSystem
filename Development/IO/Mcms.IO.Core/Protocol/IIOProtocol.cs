using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Core.Protocol.Writing;

namespace Mcms.IO.Core.Protocol
{
    public interface IIOProtocol
    {
        string Name { get; }

        IIOReader Reader { get; }

        IIOWriter Writer { get; }
    }
}
