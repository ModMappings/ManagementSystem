using System;

namespace Mcms.Api.Business.Core.Workflows.Mapping.Mappings.Voting
{
    public class AlreadyVotedAgainstProposalException
        : InvalidOperationException
    {
        public AlreadyVotedAgainstProposalException(
            Guid userId, Guid proposalId
            ) : base($"The user: {userId} has already voted against proposal: {proposalId}.")
        {
            UserId = userId;
            ProposalId = proposalId;
        }

        public Guid UserId { get; }

        public Guid ProposalId { get; }
    }
}
