using Mcms.IO.Core;
using Mcms.IO.Core.Protocol;
using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Core.Protocol.Writing;

namespace Mcms.IO.DI.Protocol
{
    public class IOProtocol : IIOProtocol
    {
        internal IOProtocol(string name, IIOReader reader, IIOWriter writer)
        {
            Name = name;
            Reader = reader;
            Writer = writer;
        }

        public string Name { get; }
        public IIOReader Reader { get; }
        public IIOWriter Writer { get; }
    }
}
