using System;
using Mcms.IO.Core.Protocol;
using Mcms.IO.Core.Protocol.Builder;
using Mcms.IO.Core.Protocol.Manager;
using Mcms.IO.Core.Protocol.Manager.Builder;
using Mcms.IO.DI.Protocol.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mcms.IO.DI.Protocol.Manager.Builder
{
    public class IOProtocolManagerBuilder : IIOProtocolManagerConfiguration
    {
        private readonly IServiceCollection _services;

        public IOProtocolManagerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IIOProtocolManagerConfiguration WithProtocol<TProtocolType>() where TProtocolType : class, IIOProtocol
        {
            this._services.AddTransient<IIOProtocol, TProtocolType>();
            this._services.AddTransient<TProtocolType>();

            return this;
        }

        public IIOProtocolManagerConfiguration WithProtocol(Action<IIOProtocolConfiguration> configuration)
        {
            var builder = new IOProtocolBuilder();
            configuration(builder);
            var protocol = builder.Build();
            this._services.AddTransient<IIOProtocol>(services => protocol);
            this._services.AddTransient(services => protocol);

            return this;
        }

        public void Build()
        {
            this._services.AddTransient<IIOProtocolManager, IOProtocolManager>();
            this._services.AddTransient<IOProtocolManager>();
        }
    }
}
