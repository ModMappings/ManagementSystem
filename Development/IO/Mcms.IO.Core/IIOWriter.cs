using System.Collections.Generic;
using System.Threading.Tasks;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Data;

namespace Mcms.IO.Core
{
    /// <summary>
    /// Represents a single writer that writes data from external releases, into artifacts.
    /// </summary>
    public interface IIOWriter
    {
        /// <summary>
        /// Reads all releases from the given handler.
        /// </summary>
        /// <param name="externalReleases">The handler to read all the releases from.</param>
        /// <param name="artifactHandler">The handler to write the artifacts too.</param>
        /// <returns>The task that represents the writing of all the releases to the handler.</returns>
        Task WriteAll(IEnumerable<ExternalRelease> externalReleases, IArtifactHandler artifactHandler);

        /// <summary>
        /// Writes all release data to the given artifact.
        /// </summary>
        /// <param name="release">The release to write.</param>
        /// <param name="artifact">The artifact to write all the release data to.</param>
        /// <returns>The task that represents the writing of the release to the artifact.</returns>
        Task WriteTo(ExternalRelease release, IArtifact artifact);
    }
}
