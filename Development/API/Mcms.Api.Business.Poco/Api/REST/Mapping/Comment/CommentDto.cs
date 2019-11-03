using System;
using System.Collections.Generic;

namespace Mcms.Api.Business.Poco.Api.REST.Mapping.Comment
{
    /// <summary>
    /// Represents a single comment made on either:
    ///  * A release,
    ///  * A proposal, or
    ///  * Another comment.
    ///
    /// Content is Markdown.
    /// </summary>
    public class CommentDto
    {
        /// <summary>
        /// The id of the comment.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The id of the user who created the comment.
        /// </summary>
        public Guid CreatedBy { get; }

        /// <summary>
        /// The moment the comment was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// The content of the comment.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The reactions applied to the comment.
        /// </summary>
        public ISet<Guid> Reactions { get; set; }

        /// <summary>
        /// Indicates if the comment has been edited.
        /// </summary>
        public bool HasBeenEdited { get;  }

        /// <summary>
        /// Indicates if the comment has been deleted.
        /// </summary>
        public bool IsDeleted { get; }

        /// <summary>
        /// The user who deleted the comment, can be the creating user or any user with enough rights.
        /// </summary>
        public Guid? DeletedBy { get; }

        /// <summary>
        /// The moment this comment was deleted.
        /// </summary>
        public DateTime? DeletedOn { get; }

        /// <summary>
        /// The proposed mapping this comment was made on.
        /// </summary>
        public Guid? ProposedMapping { get; set; }

        /// <summary>
        /// The release this comment was made on.
        /// </summary>
        public Guid? Release { get; set; }

        /// <summary>
        /// The comment this comment was made on.
        /// </summary>
        public Guid? Parent { get; set; }

        /// <summary>
        /// The comments made on this comment.
        /// </summary>
        public ISet<Guid> Children { get; set; }
    }
}
