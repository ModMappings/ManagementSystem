using System;

namespace Data.WebApi.Model.Creation.Core
{
    /// <summary>
    /// Model used to create a new proposal to edit mappings.
    /// </summary>
    public class CreateProposalModel
    {
        /// <summary>
        /// The id of the versioned component for which the proposal was made.
        /// </summary>
        public Guid ProposedFor { get; set; }

        /// <summary>
        /// Indicates if this vote is open to the public.
        /// If the vote is public, any authenticated user can vote on this proposal.
        /// If not then only users with a review right can vote on the proposal.
        /// </summary>
        public bool IsPublicVote { get; set; }

        /// <summary>
        /// The comment made to explain the proposal.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The proposed change to the input mapping.
        /// </summary>
        public string NewInput { get; set; }

        /// <summary>
        /// The proposed change to the mapping.
        /// </summary>
        public string NewOutput { get; set; }

        /// <summary>
        /// The name of the mapping type
        /// </summary>
        public string MappingTypeName { get; set; }
    }
}
