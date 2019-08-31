using System;

namespace API.Model.Creation.Parameter
{
    /// <summary>
    /// Model used to create a new versioned parameter.
    /// </summary>
    public class CreateVersionedParameterModel
        : CreateParameterModel
    {
        /// <summary>
        /// The id of the core method mapping a versioned mapping is created for.
        /// </summary>
        public Guid VersionedMappingFor { get; set; }

        /// <summary>
        /// The id of the game version for which a mapping is created.
        /// </summary>
        public Guid GameVersion { get; set; }
    }
}
