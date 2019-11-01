using System;
using System.Threading.Tasks;
using Mcms.Api.Data.Poco.Models.Mapping.Mappings.Voting;

namespace Mcms.Api.Business.Core.Workflows.Mapping.Mappings.Voting
{
    public interface IVoteForProposalWorkflow
    {

        /// <summary>
        /// <para>
        /// Makes the current user vote for the proposal with the given id.
        /// </para>
        /// <para>
        /// If the user has an open vote in favor of the proposal, an exception is thrown.
        /// </para>
        /// <para>
        /// If the user has an open vote against the proposal, this vote is first rescinded before the vote in favor for the proposal is created.
        /// </para>
        /// </summary>
        /// <param name="proposalId">The id of the proposal that the </param>
        /// <returns>The voting record for the vote in favor of the proposal.</returns>
        /// <exception cref="AlreadyVotedInFavorOfProposalException">is thrown when the user already has voted in favor for the exception, and that vote has not been rescinded.</exception>
        /// <exception cref="UnauthorizedAccessException">is thrown when the user is not allowed to vote on the given proposal.</exception>
        Task<VotingRecord> ExecuteFor(Guid proposalId);
    }
}
