using System;

namespace API.Model.Read.Core
{
    /// <summary>
    /// Read model for game versions.
    /// </summary>
    public class GameVersionReadModel
    {
        /// <summary>
        /// The id of the game version.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the game version.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates if this game version is a pre release.
        /// </summary>
        public bool IsPreRelease { get; set; }

        /// <summary>
        /// Indicates if this game version is a snapshot.
        /// </summary>
        public bool IsSnapshot { get; set; }

        /// <summary>
        /// The id of the user who created this game version.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The date and time that this game version was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
