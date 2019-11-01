using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Comments;

namespace Mcms.Api.Data.Core.Manager.Comments
{

    /// <summary>
    /// Manager that handles interaction with comment reactions.
    /// </summary>
    public interface ICommentReactionDataManager
    {

        /// <summary>
        /// Finds all comment reactions with the given id.
        /// </summary>
        /// <param name="id">The given id to find comment reactions by,</param>
        /// <returns>The task that looks up the comment reactions that match the given id.</returns>
        Task<IQueryable<CommentReaction>> FindById(Guid id);

        /// <summary>
        /// Finds the comment reactions who's type match the given type.
        /// </summary>
        /// <param name="type">The type to match the comments reaction type against.</param>
        /// <returns>The task that looks up the comment reactions who's type matches the given one-</returns>
        Task<IQueryable<CommentReaction>> FindByType(CommentReactionType type);

        /// <summary>
        /// Finds all comment reactions that are part of a comment who's id matches the given one.
        /// </summary>
        /// <param name="commentId">The id of the comment to get the comment reactions for.</param>
        /// <returns>The task that looks up comment reactions on a comment with the given id.</returns>
        Task<IQueryable<CommentReaction>> FindByComment(Guid commentId);

        /// <summary>
        /// <para>
        /// Finds all comment reactions who match the given filter data.
        /// </para>
        /// <para>
        /// This methods finds the intersection between
        /// <see cref="FindById(Guid)"/> if the <paramref name="id"/> parameter is not <code>null</code>,
        /// <see cref="FindByType(CommentReactionType)"/> if the <paramref name="type"/> is not null,
        /// <see cref="FindByComment(Guid)"/> if the <paramref name="commentId"/> parameter is not <code>null</code>.
        /// </para>
        /// <para>
        /// This means that for a comment reaction to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find comment reactions by,</param>
        /// <param name="type">The type to match the comments reaction type against.</param>
        /// <param name="commentId">The id of the comment to get the comment reactions for.</param>
        /// <returns>The task that represents the lookup of comment reactions that match the filter data, based on intersection.</returns>
        Task<IQueryable<CommentReaction>> FindUsingFilter(
            Guid? id = null,
            CommentReactionType? type = null,
            Guid? commentId = null
        );

        /// <summary>
        /// Creates a new comment reaction.
        /// The created comment reaction is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="commentReaction">The comment reaction to create.</param>
        /// <returns>The task that describes the creation of the comment reaction.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known comment reaction or a uncommitted deleted comment reaction.</exception>
        Task CreateCommentReaction(
            CommentReaction commentReaction
        );

        /// <summary>
        /// Updates an already existing comment reaction.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="commentReaction">The comment reaction to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the comment reaction.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown comment reaction (uncommitted deleted) or an uncommitted created comment reaction.</exception>
        Task UpdateCommentReaction(
            CommentReaction commentReaction
        );

        /// <summary>
        /// Deletes an already existing comment reaction.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="commentReaction">The comment reaction to delete.</param>
        /// <returns>The task that describes the deletion of the comment reaction from the manager.</returns>
        Task DeleteCommentReaction(
            CommentReaction commentReaction
        );

        /// <summary>
        /// Indicates if the manager has pending changes that need to be saved.
        /// </summary>
        bool HasPendingChanges { get; }

        /// <summary>
        /// Saves the pending changes.
        /// If <see cref="HasPendingChanges"/> is <code>false</code> then this method short circuits into a <code>noop;</code>
        /// </summary>
        /// <returns>The task that describes the saving of the pending changes in this manager.</returns>
        Task SaveChanges();
    }
}
