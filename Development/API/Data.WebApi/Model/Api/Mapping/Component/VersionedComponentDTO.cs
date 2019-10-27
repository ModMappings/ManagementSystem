using System;
using System.Collections.Generic;

namespace Data.WebApi.Model.Api.Mapping.Component
{
    /// <summary>
    /// Represents a component that is mapped in a given MC version.
    /// This class contains additional data depending on the type of component this represents.
    ///
    /// Any fields tho whom this applies are marked and as such this should be kept in mind when using them.
    /// </summary>
    public class VersionedComponentDTO
    {
        /// <summary>
        /// The id of the versioned component.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The game version for this versioned component.
        /// </summary>
        public string GameVersion { get; set; }

        /// <summary>
        /// The id of the user who created the versioned component.
        /// </summary>
        public Guid CreatedBy { get; }

        /// <summary>
        /// The moment the versioned component was created.
        /// </summary>
        public DateTime CreatedOn { get; }

        /// <summary>
        /// The overall component this versioned component is an versioned instance for.
        /// </summary>
        public Guid Component { get; set; }

        /// <summary>
        /// The committed mappings for this versioned component.
        /// </summary>
        public List<Guid> Mappings { get; }

        /// <summary>
        /// The proposals for this versioned component.
        /// </summary>
        public List<Guid> Proposals { get; }

        /// <summary>
        /// The locked mapping types for this versioned component.
        /// </summary>
        public List<string> LockedMappingTypes { get; }

        /// <summary>
        /// The id of the package, class or method, this versioned component is in.
        /// If this represents the root package, then this field is null.
        /// In all other cases this is never null.
        /// </summary>
        /// <remarks>
        /// Only relevant for PACKAGE and CLASS, MEMBER and FIELDS ComponentTypes.
        /// </remarks>
        public Guid? MemberOf { get; set; }

        /// <summary>
        /// Contains a list of classes or packages which are part of this component, if this component is a PACKAGE.
        /// Contains a list of fields and methods which are part of this component, if this component is a CLASS.
        /// </summary>
        /// <remarks>
        /// Only relevant for PACKAGE and CLASS ComponentTypes.
        /// </remarks>
        public List<Guid> Members { get; set; }

        /// <summary>
        /// Contains a list of class from which this class inherits.
        /// </summary>
        /// <remarks>
        /// Only relevant for CLASS ComponentTypes.
        /// </remarks>
        public List<Guid> InheritsFrom { get; set; }
    }
}
