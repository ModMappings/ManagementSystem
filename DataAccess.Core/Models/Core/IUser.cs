using System;
using System.Collections.Generic;

namespace DataAccess.Core.Models.Core
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// The id of the user.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Indicates if this user can create mapping proposals.
        /// </summary>
        bool canEdit { get; set; }

        /// <summary>
        /// Indicates if this user can review a edited mapping proposal.
        /// </summary>
        bool canReview { get; set; }

        /// <summary>
        /// Indicates if this user can commit (accept) a edited mapping proposal.
        /// </summary>
        bool canCommit { get; set; }

        /// <summary>
        /// Indicates if this user can release a set of mappings to the general public.
        /// </summary>
        bool canRelease { get; set; }

        /// <summary>
        /// Indicates if this user can create new game versions.
        /// </summary>
        bool canCreateGameVersions { get; set; }
    }
}
