using System.Threading.Tasks;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Data;

namespace Mcms.IO.Core.Protocol.Writing
{
    /// <summary>
    /// Represents a single writer that writes data from external releases, into artifacts.
    /// </summary>
    public interface IIOWriter
    {
        /// <summary>
        /// Writes all release data to the given artifact.
        /// </summary>
        /// <param name="externalRelease">The release to write.</param>
        /// <param name="artifact">The artifact to write all the release data to.</param>
        /// <param name="context">The writing context that contains additional information on how to handle the writing.</param>
        /// <returns>The task that represents the writing of the release to the artifact.</returns>
        Task WriteTo(ExternalRelease externalRelease, IArtifact artifact, WriteContext context);
    }
}
