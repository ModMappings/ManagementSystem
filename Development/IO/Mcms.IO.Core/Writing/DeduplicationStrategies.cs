namespace Mcms.IO.Core.Writing
{
    public class DeduplicationStrategies
    {
        public DeduplicationStrategies(DeduplicationStrategy package, DeduplicationStrategy @class, DeduplicationStrategy method, DeduplicationStrategy field, DeduplicationStrategy parameter)
        {
            Package = package;
            Class = @class;
            Method = method;
            Field = field;
            Parameter = parameter;
        }

        public DeduplicationStrategy Package { get; }

        public DeduplicationStrategy Class { get; }

        public DeduplicationStrategy Method { get; }

        public DeduplicationStrategy Field { get; }

        public DeduplicationStrategy Parameter { get; }
    }
}
