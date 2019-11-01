using System;
using System.Collections.Generic;
using Mcms.Api.Data.Poco.Models.Core;

namespace Data.WebApi.Model.Api.Mapping.Mappings
{
    /// <summary>
    /// Represents a single committed mapping in the system.
    /// Committed mappings are mappings which are actively visible to external systems and can be used during modding.
    /// </summary>
    public class CommittedMappingDto
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
        /// The proposal with which this committed mapping was created.
        /// If an admin created this mapping, then it might be possible that no proposal exists.
        /// Additionally mappings for new MC versions also do not have a proposal since they where created by the system
        /// once the game version was released.
        /// </summary>
        public Guid? Proposal { get; set; }

        /// <summary>
        /// The releases which contain this committed mapping.
        /// </summary>
        public ISet<Guid> Releases { get; }
    }
}
