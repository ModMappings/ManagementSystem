using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model.Creation.Core
{
    /// <summary>
    /// Model used to create a new game version.
    /// </summary>
    /// <example>
    /// {
    ///     "id": "00000000-0000-0000-0000-000000000000",
    ///     "name": "1.14.0",
    ///     "isPreRelease": false,
    ///     "isSnapshot": false
    /// }
    /// </example>
    public class CreateGameVersionModel
    {
        /// <summary>
        /// The name of the new game version.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Indicates if this new game version is a pre release.
        /// </summary>
        public bool IsPreRelease { get; set; }

        /// <summary>
        /// Indicates if this new game version is a snapshot.
        /// </summary>
        public bool IsSnapshot { get; set; }
    }
}
