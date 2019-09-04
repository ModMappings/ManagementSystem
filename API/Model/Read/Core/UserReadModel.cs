using System;

namespace API.Model.Read.Core
{
    /// <summary>
    /// Model that represents an already registered user.
    /// </summary>
    public class UserReadModel
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates if the user is allowed to make proposals.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Indicates if the user is allowed to make review choices.
        /// </summary>
        public bool CanReview { get; set; }

        /// <summary>
        /// Indicates if the user is allowed to merge a proposal into a committed entry.
        /// </summary>
        public bool CanCommit { get; set; }

        /// <summary>
        /// Indicates if the user is allowed to make releases.
        /// </summary>
        public bool CanRelease { get; set; }

        /// <summary>
        /// Indicates if the user is allowed make new game versions from the current mappings.
        /// </summary>
        public bool CanCreateGameVersions { get; set; }

        /// <summary>
        /// The date and time that this user was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
