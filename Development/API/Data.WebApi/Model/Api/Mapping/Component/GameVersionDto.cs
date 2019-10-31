using System;

namespace Data.WebApi.Model.Api.Mapping.Component
{
    /// <summary>
    /// Represents a single version of the game.
    /// </summary>
    public class GameVersionDto
    {

        /// <summary>
        /// The id of the game version.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the game version.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The moment the game version was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// The id of the user who created the game version.
        /// </summary>
        public Guid CreatedBy { get; }

        /// <summary>
        /// Indicates if this game version is a pre-release.
        /// </summary>
        public bool IsPreRelease { get; set; }

        /// <summary>
        /// Indicates if this game version is a snapshot.
        /// </summary>
        public bool IsSnapshot { get; set; }
    }
}
