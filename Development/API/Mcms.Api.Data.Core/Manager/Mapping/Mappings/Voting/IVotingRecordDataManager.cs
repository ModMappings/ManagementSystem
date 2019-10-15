using System;
using System.Linq;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;

namespace Mcms.Api.Data.Core.Manager.Mapping.Mappings.Voting
{
    /// <summary>
    /// Manager that handles interactions with voting records.
    /// </summary>
    public interface IVotingRecordDataManager
    {

        /// <summary>
        /// Finds all voting records with a given id.
        /// </summary>
        /// <param name="id">The id of the voting record to match against.</param>
        /// <returns>The task that describes the look up of voting records by id.</returns>
        Task<IQueryable<VotingRecord>> FindById(Guid id);

        /// <summary>
        /// Finds all voting records for a given proposal.
        /// </summary>
        /// <param name="proposalId">The id of the proposal to find the voting records for.</param>
        /// <returns>The task that describes the look up of voting records by proposal.</returns>
        Task<IQueryable<VotingRecord>> FindByProposal(Guid proposalId);

        /// <summary>
        /// Finds all voting records made by a given user.
        /// </summary>
        /// <param name="userId">The user id to find the voting records for.</param>
        /// <returns>The task that describes the look up of voting records by user.</returns>
        Task<IQueryable<VotingRecord>> FindByUser(Guid userId);

         ///  <summary>
        /// <para>
        /// Finds all voting records who match the given filter data.
        /// </para>
        /// <para>
        /// This methods finds the intersection between <see cref="FindById(Guid)"/> if the <paramref name="id"/> parameter is not <code>null</code>,
        /// <see cref="FindByProposal(Guid)"/> if the <paramref name="proposalId"/> is not null,
        /// <see cref="FindByUser(Guid)"/> if the <paramref name="userId"/> parameter is not <code>null</code>.
        /// </para>
        /// <para>
        /// This means that for a voting record to be returned it has to match all the provided filter data.
        /// </para>
        /// <para>
        /// </para>
        /// If no parameter is provided, or all are <code>null</code>, then all entries are returned.
        /// </summary>
        /// <param name="id">The given id to find committed mappings by,</param>
         /// <param name="proposalId">The id of the proposal to find the voting records for.</param>
         /// <param name="userId">The user id to find the voting records for.</param>
        /// <returns>The task that represents the lookup of voting records that match the filter data, based on intersection.</returns>
        Task<IQueryable<VotingRecord>> FindUsingFilter(Guid? id = null, Guid? proposalId = null, Guid? userId = null);

        /// <summary>
        /// Creates a new voting record.
        /// The created voting record is not saved directly, but has to be saved separately.
        /// </summary>
        /// <param name="votingRecord">The voting record to create.</param>
        /// <returns>The task that describes the creation of the voting record.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either a known voting record or a uncommitted deleted voting record.</exception>
        Task CreateVotingRecord(
            VotingRecord votingRecord
        );

        /// <summary>
        /// Updates an already existing voting record.
        /// Updates are not committed to the backing store directly, but have to be saved separately.
        /// </summary>
        /// <param name="votingRecord">The voting record to update.</param>
        /// <returns>The task that describes the updating of the manager with the data from the voting record.</returns>
        /// <exception cref="InvalidOperationException">is thrown when this method is called with either an unknown voting record (uncommitted deleted) or an uncommitted created voting record.</exception>
        Task UpdateVotingRecord(
            VotingRecord votingRecord
        );

        /// <summary>
        /// Deletes an already existing voting record.
        /// Deletions are not committed to the backing store directly but have to be saved separately.
        /// </summary>
        /// <param name="votingRecord">The voting record to delete.</param>
        /// <returns>The task that describes the deletion of the voting record from the manager.</returns>
        Task DeleteVotingRecord(
            VotingRecord votingRecord
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
