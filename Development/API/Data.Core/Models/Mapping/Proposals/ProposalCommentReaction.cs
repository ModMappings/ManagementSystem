using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Core.Models.Mapping.Proposals
{
    /// <summary>
    /// Represents a single reaction on a proposal comment.
    /// </summary>
    public class ProposalCommentReaction
    {
        /// <summary>
        /// The id of the reaction.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The id of the user who created the reaction.
        /// </summary>
        [Required]
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The moment the reaction was created.
        /// </summary>
        [Required]
        public Guid CreatedOn { get; set; }

        /// <summary>
        /// The type of reaction.
        /// </summary>
        [Required]
        public ProposalCommentReactionType Type { get; set; }

        /// <summary>
        /// The comments content.
        /// </summary>
        [Required]
        public ProposalComment Comment { get; set; }
    }
}
