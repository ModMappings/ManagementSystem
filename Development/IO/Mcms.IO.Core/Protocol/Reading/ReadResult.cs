using System;
using Mcms.IO.Core.Deduplication;
using Mcms.IO.Data;

namespace Mcms.IO.Core.Protocol.Reading
{
    /// <summary>
    /// Represents a single result of a read operation of a given artifact.
    /// </summary>
    public class ReadResult
    {
        public ReadResult(ExternalRelease release, DeduplicationStrategies strategies)
        {
            Release = release ?? throw new ArgumentNullException(nameof(release));
            Strategies = strategies ?? throw new ArgumentNullException(nameof(strategies));
        }

        /// <summary>
        /// The release that has been imported.
        /// </summary>
        public ExternalRelease Release { get; }
        
        /// <summary>
        /// The deduplication strategies to be used when this release is written.
        /// </summary>
        public DeduplicationStrategies Strategies { get; }
    }
}