using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model.Creation.Core
{
    /// <summary>
    /// Model used to create a new User.
    /// Allows external application (like the authentication server) to create new user identifications.
    /// </summary>
    /// <example>
    /// {
    ///     "id": "00000000-0000-0000-0000-000000000000",
    ///     "name": "bob",
    ///     "canEdit": true,
    ///     "canReview": false,
    ///     "canCommit": false,
    ///     "canRelease": false,
    ///     "canCreateGameVersions": false
    /// }
    /// </example>
    public class CreateUserModel
    {
        /// <summary>
        /// The name of the new user.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Indicates if the new user is allowed to make proposals.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Indicates if the new user is allowed to make review choices.
        /// </summary>
        public bool CanReview { get; set; }

        /// <summary>
        /// Indicates if the new user is allowed to merge a proposal into a committed entry.
        /// </summary>
        public bool CanCommit { get; set; }

        /// <summary>
        /// Indicates if the new user is allowed to make releases.
        /// </summary>
        public bool CanRelease { get; set; }

        /// <summary>
        /// Indicates if the new user is allowed make new game versions from the current mappings.
        /// </summary>
        public bool CanCreateGameVersions { get; set; }
    }
}
