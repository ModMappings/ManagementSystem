using System;

namespace Mcms.Api.Business.Poco.Api.REST.Mapping.Comment
{
    /// <summary>
    /// Represents a single reaction to a given comment.
    /// </summary>
    public class CommentReactionDto
    {
        /// <summary>
        /// The id of the reaction.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The id of the user who created the reaction.
        /// </summary>
        public Guid CreatedBy { get; }

        /// <summary>
        /// The moment the reaction was created.
        /// </summary>
        public Guid CreatedOn { get; }

        /// <summary>
        /// The type of reaction.
        /// </summary>
        public CommentReactionTypeDto Type { get; set; }

        /// <summary>
        /// The comments content.
        /// </summary>
        public Guid Comment { get; set; }
    }
}
