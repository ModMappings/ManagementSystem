using System.ComponentModel.DataAnnotations;

namespace Mcms.Api.Data.Poco.Models.Mapping.Metadata
{
    /// <summary>
    /// Metadata for a versioned component representing a parameter.
    /// </summary>
    public class ParameterMetadata
        : MetadataBase
    {
        /// <summary>
        /// The method which this parameter is a parameter of.
        /// </summary>
        [Required]
        public virtual MethodMetadata ParameterOf { get; set; }

        /// <summary>
        /// The 0-based index of this parameter in the parameter list of the method.
        /// </summary>
        [Required]
        public int Index { get; set; }
    }
}
