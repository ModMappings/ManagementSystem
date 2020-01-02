namespace Mcms.IO.Core.Writing
{
    public class WriteContext
    {
        public WriteContext(DeduplicationStrategies deduplicationStrategies)
        {
            DeduplicationStrategies = deduplicationStrategies;
        }

        public DeduplicationStrategies DeduplicationStrategies { get; }
    }
}
