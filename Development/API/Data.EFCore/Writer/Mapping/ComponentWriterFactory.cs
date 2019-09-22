using System;
using Data.Core.Models.Mapping;
using Data.Core.Writers.Core;
using Data.EFCore.Context;

namespace Data.EFCore.Writer.Mapping
{
    public class ComponentWriterFactory
    {
        private readonly MCMSContext _mcmsContext;

        public ComponentWriterFactory(MCMSContext mcmsContext)
        {
            _mcmsContext = mcmsContext;
        }

        public IComponentWriter Build(ComponentType type)
        {
            switch (type)
            {
                case ComponentType.CLASS:
                    return new ClassWriter(_mcmsContext);
                case ComponentType.METHOD:
                    return new MethodWriter(_mcmsContext);
                case ComponentType.FIELD:
                    return new FieldWriter(_mcmsContext);
                case ComponentType.PARAMETER:
                    return new ParameterWriter(_mcmsContext);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
