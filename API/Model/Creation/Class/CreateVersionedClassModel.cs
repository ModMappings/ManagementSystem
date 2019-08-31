using System;

namespace API.Model.Creation.Class
{
    /// <summary>
    /// Model used to create a new versioned class, if it has already been registered.
    /// </summary>
    public class CreateVersionedClassModel
        : CreateClassModel
    {
        /// <summary>
        /// The id of the class mapping for which a new versioned mapping is created.
        /// </summary>
        public Guid VersionedMappingFor { get; set; }

        /// <summary>
        /// The id of the version for which a new versioned class is being created.
        /// </summary>
        public Guid GameVersion { get; set; }
    }
}
