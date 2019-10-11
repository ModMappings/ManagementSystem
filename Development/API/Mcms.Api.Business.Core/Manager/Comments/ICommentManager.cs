using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Comments;

namespace Mcms.Api.Business.Core.Manager.Comments
{

    /// <summary>
    /// Manager that handles interaction with comments.
    /// </summary>
    public interface ICommentManager
    {

        /// <summary>
        /// Finds all comments with the given id.
        /// </summary>
        /// <param name="id">The given id to find comments by,</param>
        /// <returns>The task that looks up the comments that match the given id.</returns>
        Task<IQueryable<Comment>> FindById(Guid id);

        /// <summary>
        /// Finds the comments who's content match the given regex.
        /// </summary>
        /// <param name="contentRegex">The regex to match the content of comments against.</param>
        /// <returns>The task that looks up the comments who's content match the given regex.</returns>
        Task<IQueryable<Comment>> FindByContent(string contentRegex);

        /// <summary>
        /// Finds all comments that are part of a release who's name matches the given regex.
        /// </summary>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <returns>The task that looks up comments that are made on a release who name matches the given regex.</returns>
        Task<IQueryable<Comment>> FindByRelease(string releaseNameRegex);

        /// <summary>
        /// Finds all comments that are part of a proposed mapping who's id matches the given one.
        /// </summary>
        /// <param name="proposedMappingId">The id of the proposed mapping to get the comments for.</param>
        /// <returns>The task that looks up comments on a proposed mapping with the given id.</returns>
        Task<IQueryable<Comment>> FindByProposedMapping(Guid proposedMappingId);

        /// <summary>
        /// <para>
        /// Finds all comments who match the given filter data.
        /// </para>
        /// <para>
        /// This methods finds the intersection between
        /// <see cref="FindById(Guid)"/> if the <paramref name="id"/> parameter is not <code>null</code>,
        /// <see cref="FindByContent(string)"/> if the <paramref name="contentRegex"/> is not null,
        /// <see cref="FindByRelease(string)"/> if the <paramref name="releaseNameRegex"/> parameter is not <code>null</code>, and
        /// <see cref="FindByProposedMapping(Guid)"/> if the <paramref name="proposedMappingId"/> parameter is not <code>null</code>.
        /// </para>
        /// <para>
        /// This means that for a comment to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find comments by,</param>
        /// <param name="contentRegex">The regex to match the content of comments against.</param>
        /// <param name="releaseNameRegex">The regex to match release names against.</param>
        /// <param name="proposedMappingId">The id of the proposed mapping to get the comments for.</param>
        /// <returns>The task that represents the lookup of comments that match the filter data, based on intersection.</returns>
        Task<IQueryable<Comment>> FindUsingFilter(
            Guid? id = null,
            string contentRegex = null,
            string releaseNameRegex = null,
            Guid? proposedMappingId = null
        );

        /// <summary>
        /// Creates a new comment.
        /// The created comment is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="comment">The comment to create.</param>
        /// <returns>The task that describes the creation of the comment.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known comment or a uncommitted deleted comment.</exception>
        Task CreateComment(
            Comment comment
        );

        /// <summary>
        /// Updates an already existing comment.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="comment">The comment to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the comment.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown comment (uncommitted deleted) or an uncommitted created comment.</exception>
        Task UpdateComment(
            Comment comment
        );

        /// <summary>
        /// Deletes an already existing comment.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="comment">The comment to delete.</param>
        /// <returns>The task that describes the deletion of the comment from the manager.</returns>
        Task DeleteComment(
            Comment comment
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
