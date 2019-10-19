namespace Mcms.Api.Business.Poco.Models.Mapping.Metadata
{
    /// <summary>
    /// Metadata for a versioned component representing a class.
    /// </summary>
    public class ClassMetadata
        : MetadataBase
    {
        /// <summary>
        /// The outer class of this class.
        /// </summary>
        public virtual ClassMetadata Outer { get; set; }

        /// <summary>
        /// The classes from which this class inherits.
        /// </summary>
        public virtual List<ClassMetadata> InheritsFrom { get; set; }

        /// <summary>
        /// The package of which this class is part of.
        /// </summary>
        public PackageMetadata Package { get; set; }

        /// <summary>
        /// The methods which are part of this class.
        /// </summary>
        public virtual List<MethodMetadata> Methods { get; set; }

        /// <summary>
        /// The fields which are part of this class.
        /// </summary>
        public virtual List<FieldMetadata> Fields { get; set; }
    }
}
