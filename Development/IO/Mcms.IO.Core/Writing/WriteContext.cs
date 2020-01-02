using Mcms.IO.Core.Deduplication;

namespace Mcms.IO.Core.Writing
{
    /// <summary>
    /// Represents the context used during writing of an external release,
    /// Contains information that is collected from the ReadResult and passed on to the writer
    /// to help with the way the data needs to be written.
    /// </summary>
    public class WriteContext
    {
        public WriteContext(DeduplicationStrategies deduplicationStrategies)
        {
            DeduplicationStrategies = deduplicationStrategies;
        }

        /// <summary>
        /// The strategies used during the deduplication phases when data is written and the writer
        /// needs to determine how to merge the existing data with the data in the external release.
        /// </summary>
        public DeduplicationStrategies DeduplicationStrategies { get; }
    }
}
