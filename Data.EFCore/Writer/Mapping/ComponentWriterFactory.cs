using System;
using Data.Core.Models.Mapping;
using Data.Core.Writers.Core;
using Data.EFCore.Context;

namespace Data.EFCore.Writer.Mapping
{
    public class ComponentWriterFactory
    {
        private readonly MCPContext _mcpContext;

        public ComponentWriterFactory(MCPContext mcpContext)
        {
            _mcpContext = mcpContext;
        }

        public IComponentWriter Build(ComponentType type)
        {
            switch (type)
            {
                case ComponentType.CLASS:
                    return new ClassWriter(_mcpContext);
                case ComponentType.METHOD:
                    return new MethodWriter(_mcpContext);
                case ComponentType.FIELD:
                    return new FieldWriter(_mcpContext);
                case ComponentType.PARAMETER:
                    return new ParameterWriter(_mcpContext);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
