namespace Mcms.IO.Core.Deduplication
{
    /// <summary>
    /// This class contains all the deduplication strategies to be used when duplication conflicts arises
    /// during the writing of an ExternalRelease.
    /// </summary>
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
