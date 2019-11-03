using System;
using System.Collections.Generic;

namespace Mcms.Api.Business.Poco.Api.REST.Mapping.Component
{
    /// <summary>
    /// Represents a component that is mapped in a given MC version.
    /// This class contains additional data depending on the type of component this represents.
    ///
    /// Any fields tho whom this applies are marked and as such this should be kept in mind when using them.
    ///
    /// </summary>
    public class VersionedComponentDto
    {
        /// <summary>
        /// The id of the versioned component.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The type of the versioned component needs to be equal to the component type of its component
        /// </summary>
        public ComponentTypeDto Type { get; }

        /// <summary>
        /// The game version for this versioned component.
        /// </summary>
        public Guid GameVersion { get; }

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
        public Guid Component { get; }

        /// <summary>
        /// The committed mappings for this versioned component.
        /// </summary>
        public ISet<Guid> Mappings { get; set; }

        /// <summary>
        /// The proposals for this versioned component.
        /// </summary>
        public ISet<Guid> Proposals { get; set; }

        /// <summary>
        /// The locked mapping types for this versioned component.
        /// </summary>
        public ISet<Guid> LockedMappingTypes { get; set; }

        /// <summary>
        /// The id of the package, class or method, this versioned component is in.
        /// If this represents the root package, then this field is null.
        /// In all other cases this is never null.
        /// </summary>
        public Guid? MemberOf { get; }

        /// <summary>
        /// The child packages of this package.
        /// </summary>
        /// <remarks>
        /// Only relevant for PACKAGE component types.
        /// </remarks>
        public ISet<Guid> ChildPackages { get; set; }

        /// <summary>
        /// The classes which are part of this package, or for which this class is the outer class for.
        /// </summary>
        /// <remarks>
        /// Only relevant for PACKAGE and CLASS component types.
        /// </remarks>
        public ISet<Guid> Classes { get; set; }

        /// <summary>
        /// The methods which are part of this class.
        /// </summary>
        /// <remarks>
        /// Only relevant for CLASS component types.
        /// </remarks>
        public ISet<Guid> Methods { get; set; }

        /// <summary>
        /// The fields which are part of this class.
        /// </summary>
        /// <remarks>
        /// Only relevant for CLASS component types.
        /// </remarks>
        public ISet<Guid> Fields { get; set; }

        /// <summary>
        /// The parameters of this method.
        /// </summary>
        /// <remarks>
        /// Only relevant for METHOD component types.
        /// </remarks>
        public ISet<Guid> Parameters { get; set; }

        /// <summary>
        /// Contains a list of class from which this class inherits.
        /// </summary>
        /// <remarks>
        /// Only relevant for CLASS ComponentTypes.
        /// </remarks>
        public ISet<Guid> InheritsFrom { get; set; }

        /// <summary>
        /// Contains a list of classes who inherit from this class.
        /// </summary>
        /// <remarks>
        /// Only relevant for CLASS component types.
        /// </remarks>
        public ISet<Guid> IsInheritedBy { get; set; }

        /// <summary>
        /// The outer classes of this class.
        /// </summary>
        /// <remarks>
        /// Only relevant for CLASS component types
        /// </remarks>
        public Guid? OuterClass { get; }

        /// <summary>
        /// Indicates if this field or method is static within a class.
        /// </summary>
        /// <remarks>
        /// Only relevant for METHOD and FIELD ComponentTypes.
        /// </remarks>
        public bool? IsStatic { get; }

        /// <summary>
        /// The descriptor of this method.
        /// </summary>
        /// <remarks>
        /// Only relevant for METHOD ComponentTypes.
        /// </remarks>
        public string Descriptor { get;  }

        /// <summary>
        /// The index of the parameter in a methods parameter list.
        /// </summary>
        /// <remarks>
        /// Only relevant for PARAMETER ComponentTypes.
        /// </remarks>
        public int Index { get; }
    }
}
