using System;
using System.Collections.Generic;
using System.Threading;
using Data.Core.Models.Class;
using Data.Core.Models.Field;
using Data.Core.Models.Method;
using Data.Core.Models.Parameter;

namespace Data.Core.Models.Core
{
    /// <summary>
    /// Represents a single release of the current mappings at the time.
    /// </summary>
    public interface IRelease
    {
        /// <summary>
        /// The id of the release.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The name of the release.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The moment this release was created on.
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// The game version this release was created for.
        /// </summary>
        IGameVersion GameVersion { get; set; }

        /// <summary>
        /// The Class methods that are part of this release.
        /// </summary>
        ICollection<IClassCommittedMappingEntry> Classes { get; set; }

        /// <summary>
        /// The method mappings that are part of this release.
        /// </summary>
        ICollection<IMethodCommittedMappingEntry> Methods { get; set; }

        /// <summary>
        /// The parameter mappings that are part of this release.
        /// </summary>
        ICollection<IParameterCommittedMappingEntry> Parameters { get; set; }

        /// <summary>
        /// The field mappings that are part of this release.
        /// </summary>
        ICollection<IFieldCommittedMappingEntry> Fields { get; set; }
    }
}
