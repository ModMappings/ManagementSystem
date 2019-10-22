using System;

namespace Mcms.Api.Business.Core.Workflows.Mapping.Mappings.Voting
{
    public class AlreadyVotedInFavorOfProposalException
        : InvalidOperationException
    {
        public AlreadyVotedInFavorOfProposalException(
            Guid userId, Guid proposalId
            ) : base($"The user: {userId} has already voted in favor of proposal: {proposalId}.")
        {
            UserId = userId;
            ProposalId = proposalId;
        }

        public Guid UserId { get; }

        public Guid ProposalId { get; }
    }
}
