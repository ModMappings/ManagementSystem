using System.Collections.Generic;
using System.Threading.Tasks;
using Mcms.IO.Core.Artifacts;
using Mcms.IO.Data;

namespace Mcms.IO.Core
{
    /// <summary>
    /// Represents a single reader that reads data from artifacts into the external data scheme.
    /// </summary>
    public interface IIOReader
    {
        /// <summary>
        /// Reads all releases from the given provider.
        /// </summary>
        /// <param name="handler">The provider to read all the releases from.</param>
        /// <returns>The task that represents the reading of all the releases the provider has to offer.</returns>
        Task<IEnumerable<ExternalRelease>> ReadAllFrom(IArtifactHandler handler);

        /// <summary>
        /// Reads all release data from the given artifact.
        /// </summary>
        /// <param name="artifact">The artifact to read all the release data from.</param>
        /// <returns>The task that represents the reading of the release from the artifact.</returns>
        Task<ExternalRelease> ReadFrom(IArtifact artifact);
    }
}
