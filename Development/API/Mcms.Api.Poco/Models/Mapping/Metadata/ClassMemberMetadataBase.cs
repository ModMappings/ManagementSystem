using System.ComponentModel.DataAnnotations;

namespace Mcms.Api.Data.Poco.Models.Mapping.Metadata
{
    /// <summary>
    /// An abstract metadata type that was created for fields and methods that holds a reference to the class in which they belong.
    /// </summary>
    public class ClassMemberMetadataBase
        : MetadataBase
    {
        /// <summary>
        /// The metadata of which this class is a member of.
        /// </summary>
        [Required]
        public virtual ClassMetadata MemberOf { get; set; }

        /// <summary>
        /// Indicates if this is a static member or not.
        /// </summary>
        [Required]
        public bool IsStatic { get; set; }
    }
}
