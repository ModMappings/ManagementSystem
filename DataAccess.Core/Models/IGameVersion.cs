using System;

namespace DataAccess.Core.Models
{
    /// <summary>
    /// Represents a single game version or a possible snapshot of a game version.
    /// </summary>
    public interface IGameVersion
    {
        /// <summary>
        /// The id of the version.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The name of the version.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Indicates if this version is a release candidate.
        /// </summary>
        bool IsReleaseCandidate { get; set; }

        /// <summary>
        /// Indicates if this version is a snapshot.
        /// </summary>
        bool IsSnapshot { get; set; }
    }
}
