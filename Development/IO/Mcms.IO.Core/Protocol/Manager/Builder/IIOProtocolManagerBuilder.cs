using System;
using Mcms.IO.Core.Protocol.Builder;

namespace Mcms.IO.Core.Protocol.Manager.Builder
{
    public interface IIOProtocolManagerConfiguration
    {
        IIOProtocolManagerConfiguration WithProtocol<TProtocolType>() where TProtocolType : class, IIOProtocol;
        IIOProtocolManagerConfiguration WithProtocol(Action<IIOProtocolConfiguration> configuration);
    }
}
