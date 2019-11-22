using System.ComponentModel.DataAnnotations;

namespace Mcms.Api.Data.Poco.Models.Mapping.Metadata
{
    /// <summary>
    /// Metadata for versioned components representing a field.
    /// </summary>
    public class FieldMetadata
        : ClassMemberMetadataBase
    {

        /// <summary>
        /// The type descriptor.
        /// </summary>
        [Required]
        public string Type { get; set; }
    }
}
