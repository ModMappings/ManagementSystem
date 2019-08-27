using System;

namespace API.Model.Read.Core
{
    /// <summary>
    /// Model that represents a release.
    /// </summary>
    public class ReleaseReadModel
    {
        /// <summary>
        /// The id of the release.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the release.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The id of the game version for which a this release was created.
        /// </summary>
        public Guid GameVersion { get; set; }

        /// <summary>
        /// The id of the user who created the release.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The date and time that this release was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
