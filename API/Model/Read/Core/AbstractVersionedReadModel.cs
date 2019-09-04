using System;
using System.Collections.Generic;

namespace API.Model.Read.Core
{
    /// <summary>
    /// Abstract view model for a versioned type.
    /// </summary>
    public class AbstractVersionedReadModel
    {
        /// <summary>
        /// The id of the versioned view model.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The id of the view model for the type that this is versioned for.
        /// </summary>
        public Guid VersionedViewModelFor { get; set; }

        /// <summary>
        /// The id of the game version this versioned view model was created for.
        /// </summary>
        public Guid GameVersion { get; set; }

        /// <summary>
        /// The latest mappings of all known game versions.
        /// </summary>
        public IEnumerable<MappingReadModel> CurrentMappings { get; set; }

        /// <summary>
        /// The proposals made for this versioned view model.
        /// </summary>
        public IEnumerable<ProposalReadModel> Proposals { get; set; }
    }
}
