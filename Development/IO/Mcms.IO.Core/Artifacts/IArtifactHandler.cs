using System.Collections.Generic;
using System.Threading.Tasks;
using Mcms.IO.Data;

namespace Mcms.IO.Core.Artifacts
{
    /// <summary>
    /// A handler for artifacts, can read or write them from its target.
    /// </summary>
    public interface IArtifactHandler
    {
        /// <summary>
        /// Gets all available artifacts from a given provider.
        /// </summary>
        /// <returns>A task representing the retrieval of the artifacts from the target.</returns>
        Task<Dictionary<string, IArtifact>> GetArtifactsAsync();

        /// <summary>
        /// Creates a new artifact with the given name.
        /// </summary>
        /// <param name="name">The name of the artifact to create.</param>
        /// <returns>A task representing the creation of the new artifact.</returns>
        Task<IArtifact> CreateNewArtifactWithName(string name);

        /// <summary>
        /// Puts all of the given artifacts and their names into the target.
        /// </summary>
        /// <param name="artifactsToPut">The artifacts and their names used to write into the target.</param>
        /// <returns>A task representing the writing of the artifacts.</returns>
        Task PutArtifactsAsync(IReadOnlyDictionary<string, IArtifact> artifactsToPut);

        /// <summary>
        /// Creates a new artifact for a given external release.
        /// </summary>
        /// <param name="externalRelease">The external release to create an artifact for.</param>
        /// <returns>The external release to create an artifact for.</returns>
        Task<IArtifact> CreateNewArtifactForRelease(ExternalRelease externalRelease);
    }
}
