using System;
using Mcms.Api.Data.Poco.Models.Comments;

namespace Data.WebApi.Model.Api.Mapping.Comment
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
        public CommentReactionType Type { get; set; }

        /// <summary>
        /// The comments content.
        /// </summary>
        public Guid Comment { get; set; }
    }
}
