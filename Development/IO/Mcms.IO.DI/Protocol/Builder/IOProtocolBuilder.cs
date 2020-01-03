using Mcms.IO.Core;
using Mcms.IO.Core.Protocol;
using Mcms.IO.Core.Protocol.Builder;
using Mcms.IO.Core.Protocol.Reading;
using Mcms.IO.Core.Protocol.Writing;

namespace Mcms.IO.DI.Protocol.Builder
{
    public class IOProtocolBuilder : IIOProtocolConfiguration
    {
        private string _name;
        private IIOReader _ioReader;
        private IIOWriter _ioWriter;

        public IOProtocolBuilder()
        {
        }

        public IIOProtocolConfiguration WithName(string name)
        {
            this._name = name;
            return this;
        }

        public IIOProtocolConfiguration WithReader(IIOReader reader)
        {
            this._ioReader = reader;
            return this;
        }

        public IIOProtocolConfiguration WithWriter(IIOWriter writer)
        {
            this._ioWriter = writer;
            return this;
        }

        public IOProtocol Build()
        {
            return new IOProtocol(
                _name,
                _ioReader,
                _ioWriter
                );
        }
    }
}
