using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mcms.Api.Data.Poco.Models.Mapping.Metadata
{
    /// <summary>
    /// Metadata for a versioned component that represents a method.
    /// </summary>
    public class MethodMetadata
        : ClassMemberMetadataBase
    {
        /// <summary>
        /// The parameters of this method.
        /// </summary>
        public virtual List<ParameterMetadata> Parameters { get; set; }

        /// <summary>
        /// The descriptor of this method.
        /// </summary>
        [Required]
        public string Descriptor { get; set; }
    }
}
