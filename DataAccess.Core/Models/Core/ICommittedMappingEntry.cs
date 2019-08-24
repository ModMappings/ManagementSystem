namespace DataAccess.Core.Models.Core
{
    /// <summary>
    /// Represents a mapping between two versions of the same source code.
    /// </summary>
    public interface ICommittedMappingEntry<TProposal> : IMappingEntry
        where TProposal : IProposalMappingEntry
    {
        /// <summary>
        /// The proposal that was made that resulted in this mapping after it was accepted.
        /// </summary>
        TProposal Proposal { get; set; }
    }
}
