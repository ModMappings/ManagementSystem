using System;
using System.Collections.Generic;

namespace Data.WebApi.Model.Api.Core.Releases
{
    /// <summary>
    /// Represents a single release made from mappings within the system.
    /// </summary>
    public class ReleaseDto
    {

        /// <summary>
        /// The id of the release.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the release.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The moment the release was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// The od of the user who created the release.
        /// </summary>
        public Guid CreatedBy { get; }

        /// <summary>
        /// The id of the game version this release was created for.
        /// </summary>
        public Guid GameVersion { get; }

        /// <summary>
        /// The id of the mapping type this release was created for.
        /// </summary>
        public Guid MappingType { get; }

        /// <summary>
        /// Indicates if this release is a snapshot.
        /// </summary>
        public bool IsSnapshot { get; }

        /// <summary>
        /// The ids of the committed mappings made for packages.
        /// </summary>
        public ISet<Guid> PackageMappings { get; }

        /// <summary>
        /// The ids of the committed mappings made for classes.
        /// </summary>
        public ISet<Guid> ClassMappings { get; }

        /// <summary>
        /// The ids of the committed mappings made for methods.
        /// </summary>
        public ISet<Guid> MethodMappings { get; }

        /// <summary>
        /// The ids of the committed mappings made for fields.
        /// </summary>
        public ISet<Guid> FieldMappings { get; }

        /// <summary>
        /// The ids of the committed mappings made for parameters.
        /// </summary>
        public ISet<Guid> ParameterMappings { get; }

        /// <summary>
        /// The comments made on the release.
        /// </summary>
        public ISet<Guid> Comments { get; set; }
    }
}
