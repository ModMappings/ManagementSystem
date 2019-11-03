using System;
using System.Collections.Generic;
using Mcms.Api.Data.Poco.Models.Core;

namespace Data.WebApi.Model.Api.Mapping.Mappings
{
    /// <summary>
    /// Represents a single proposal for a new mapping that is to be merged into the system.
    /// </summary>
    public class ProposedMappingDto
    {
        /// <summary>
        /// The id of the mapping.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The versioned component that this mapping is for.
        /// </summary>
        public Guid VersionedComponent { get; }

        /// <summary>
        /// The moment the mapping was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// The id of the user who created the mapping.
        /// Is the user who merged the mapping after approval.
        ///
        /// To find the id of the user who proposed the mapping, see the referenced proposal.
        /// </summary>
        public Guid CreatedBy { get; }

        /// <summary>
        /// The input of the mapping.
        /// </summary>
        public string InputMapping { get; set; }

        /// <summary>
        /// The output of the mapping.
        /// </summary>
        public string OutputMapping { get; set; }

        /// <summary>
        /// The documentation that accompanies the mapping.
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// The distribution that this mapping can be found in.
        /// </summary>
        public Distribution Distribution { get; set; }

        /// <summary>
        /// The mapping type for which this mapping is made.
        /// </summary>
        public Guid MappingType { get; }

        /// <summary>
        /// Indicates if the proposal is still open, or if it has been closed.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Indicates if the proposal is a public vote.
        /// Else voting rights are required.
        /// </summary>
        public bool IsPublicVote { get; set; }

        /// <summary>
        /// The votes for or against this proposal made by users.
        /// </summary>
        public virtual ISet<Guid> Votes { get; set; }

        /// <summary>
        /// The comments made by users, at least one comment is required (the comment made by the opener).
        /// </summary>
        public virtual ISet<Guid> Comments { get; set; }

        /// <summary>
        /// The id of the user who closed the proposal.
        /// </summary>
        public virtual Guid? ClosedBy { get; set; }

        /// <summary>
        /// The moment the proposal was closed.
        /// </summary>
        public DateTime? ClosedOn { get; set; }

        /// <summary>
        /// Indicates if the proposal was merged into a committed mapping during closing.
        /// </summary>
        public bool? Merged { get; set; }

        /// <summary>
        /// The id of the committed mapping (if it exists, and the proposal has been merged successfully), with whom this proposal was merged into the committed data.
        /// </summary>
        public Guid? CommittedWith { get; set; }
    }
}
